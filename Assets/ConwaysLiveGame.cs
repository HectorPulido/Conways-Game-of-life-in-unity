using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConwaysLiveGame : MonoBehaviour {

    public static ConwaysLiveGame singleton;

    public Color runningColor;
    public Color pauseColor;
    public Vector2Int boardSize;
    public GameObject prefab;
    public float updatePeriod;

    private Camera mainCamera;
    private bool running = true;
    private MeshRenderer[, ] meshBoard;
    private GameObject[, ] goBoard;
    private bool[, ] booleanBoard;

    // Start is called before the first frame update
    private IEnumerator Start () {

        if (singleton != null) {
            Destroy (this);
        } else {
            singleton = this;

            mainCamera = Camera.main;

            meshBoard = new MeshRenderer[boardSize.x, boardSize.y];
            goBoard = new GameObject[boardSize.x, boardSize.y];
            booleanBoard = new bool[boardSize.x, boardSize.y];

            for (int i = 0; i < boardSize.x; i++) {
                for (int j = 0; j < boardSize.y; j++) {
                    var position = new Vector3 (i - boardSize.x / 2.0f, j - boardSize.y / 2.0f, 0);
                    goBoard[i, j] = Instantiate (prefab, position, Quaternion.identity);
                    meshBoard[i, j] = goBoard[i, j].GetComponent<MeshRenderer> ();
                    var cell = goBoard[i, j].GetComponent<Cell> ();
                    cell.position = new Vector2Int (i, j);
                }
            }

            while (true) {
                if (running) {
                    GameFrame ();
                }
                yield return new WaitForSeconds (updatePeriod);
            }
        }
    }

    private void Update () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            running = !running;

            mainCamera.backgroundColor = running ? runningColor : pauseColor;
        }
    }

    private int GetNeighbors (int x, int y) {
        var count = 0;

        if (y + 1 < boardSize.y) {
            if (booleanBoard[x, y + 1]) {
                count++;
            }
        }

        if (y - 1 >= 0) {
            if (booleanBoard[x, y - 1]) {
                count++;
            }
        }
        if (x + 1 < boardSize.x) {
            if (booleanBoard[x + 1, y]) {
                count++;
            }
        }
        if (x - 1 >= 0) {
            if (booleanBoard[x - 1, y]) {
                count++;
            }
        }

        if (y + 1 < boardSize.y && x + 1 < boardSize.x) {
            if (booleanBoard[x + 1, y + 1]) {
                count++;
            }
        }

        if (x - 1 >= 0 && y - 1 >= 0) {
            if (booleanBoard[x - 1, y - 1]) {
                count++;
            }
        }

        if (x - 1 >= 0 && y + 1 < boardSize.y) {
            if (booleanBoard[x - 1, y + 1]) {
                count++;
            }
        }

        if (x + 1 < boardSize.x && y - 1 >= 0) {
            if (booleanBoard[x + 1, y - 1]) {
                count++;
            }
        }

        return count;
    }

    private void GameFrame () {
        var nextBoolenBoard = (bool[, ]) booleanBoard.Clone ();

        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                var neighbors = GetNeighbors (i, j);
                //Any live cell with two or three live neighbors survives.
                if (booleanBoard[i, j] == true) {
                    if (neighbors == 2 || neighbors == 3) {
                        nextBoolenBoard[i, j] = true;
                        continue;
                    }
                }

                //Any dead cell with three live neighbors becomes a live cell.
                if (booleanBoard[i, j] == false) {
                    if (neighbors == 3) {
                        nextBoolenBoard[i, j] = true;
                        continue;
                    }
                }

                //All other live cells die in the next generation. Similarly, all other dead cells stay dead.
                nextBoolenBoard[i, j] = false;
            }
        }

        booleanBoard = (bool[, ]) nextBoolenBoard.Clone ();

        for (int i = 0; i < boardSize.x; i++) {
            for (int j = 0; j < boardSize.y; j++) {
                meshBoard[i, j].enabled = booleanBoard[i, j];
            }
        }
    }

    public void ToggleCell (int x, int y) {
        booleanBoard[x, y] = !booleanBoard[x, y];
        meshBoard[x, y].enabled = booleanBoard[x, y];
    }
}
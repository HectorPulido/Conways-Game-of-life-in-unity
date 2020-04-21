using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour {
    public Vector2Int position;
    private void OnMouseDown () {
        ConwaysLiveGame.singleton.ToggleCell(position.x, position.y);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportable : BodyComponent
{
    public GameObject movementDisplayPrefab;
    private BodyMovementArrowDisplay _displayController;

    public override void Init() {
        base.Init();
        GameObject display = Instantiate(movementDisplayPrefab, transform);
        _displayController = display.GetComponent<BodyMovementArrowDisplay>();
        Debug.Assert(_displayController != null, this.gameObject.name + " is missing a display prefab.");
    }

    void Update() {
        if (_body && _body.controllingPlayer != null && _displayController != null) {
            Vector3 to = SceneAccess.instance.grid.GetCellCenterWorld(_body.controllingPlayer.selectedCell);
            _displayController.UpdateDisplay(transform.position,to);
        }
    }
    
    public bool StartTeleporting(LocalPlayer player) {
        if (_body.controllingPlayer != null) {
            return false;
        }

        if (!SceneAccess.instance.bodySystem.ClaimControllershipOfBody(_body, player)) {
            return false;
        }
        
        _displayController.SetVisibility(true);
        _displayController.SetColor(_body.controllingPlayer.playerColor);
        Debug.Log("Starting teleport of " + gameObject.name);
        return true;
    }

    public bool CompleteTeleporting(Vector3Int cell) {
        return SceneAccess.instance.bodySystem.MoveBody(_body, cell);
    }

    public void CancelTeleporting() {
        _displayController.SetVisibility(false);
        SceneAccess.instance.bodySystem.RelinquishControllership(_body);
    }
}

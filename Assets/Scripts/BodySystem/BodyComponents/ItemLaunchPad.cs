using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class ItemLaunchPad : BodyComponent
{
    public GameObject movementDisplayPrefab;
    private BodyMovementArrowDisplay _displayController;
    private ItemInventory _itemInventory;

    public override void Init() {
        base.Init();
        GameObject display = Instantiate(movementDisplayPrefab, this.transform);
        _displayController = display.GetComponent<BodyMovementArrowDisplay>();
        _itemInventory = GetComponent<ItemInventory>();

        Debug.Assert(_displayController != null, this.gameObject.name + " is missing a display prefab.");
        Debug.Assert(_itemInventory != null, this.gameObject.name + " is missing an Item Inventory component.");
    }

    void Update() {
        if (_body && _body.controllingPlayer != null && _displayController != null) {
            Vector3 to = SceneAccess.instance.grid.GetCellCenterWorld(_body.controllingPlayer.selectedCell);
            _displayController.UpdateDisplay(transform.position,to);
        }
    }
    
    public bool StartSending(LocalPlayer player) {
        if (_body.controllingPlayer != null) {
            return false;
        }

        if (!_itemInventory.HasItemToTake()) {
            return false;
        }
        
        if (!SceneAccess.instance.bodySystem.ClaimControllershipOfBody(_body, player)) {
            return false;
        }
        
        _displayController.SetVisibility(true);
        _displayController.SetColor(_body.controllingPlayer.playerColor);
        Debug.Log("Sending from " + gameObject.name);
        return true;
    }

    public void CompleteSending() {
        _itemInventory.TakeItem();
    }
    
    public void CancelSending() {
        _displayController.SetVisibility(false);
        SceneAccess.instance.bodySystem.RelinquishControllership(_body);
    }

    public Item LaunchedItem() {
        return _itemInventory.currentItem;
    }
}
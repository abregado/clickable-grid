using System;
using UnityEngine;

public class ItemInventory : MonoBehaviour {
    public enum InventoryState {
        EMPTY,
        INCOMING,
        FULL
    };

    public InventoryState currentInventoryState = InventoryState.EMPTY;
    private Body _owner;
    private MeshRenderer _inventoryRenderer;

    private void Awake() {
        _owner = GetComponent<Body>();
        _inventoryRenderer = transform.Find("Model/Inventory").GetComponent<MeshRenderer>();
        UpdateInventoryVisuals();
    }
    
    public bool PutItem() {
        if (currentInventoryState != InventoryState.EMPTY) {
            return false;
        }

        currentInventoryState = InventoryState.FULL;
        UpdateInventoryVisuals();
        return true;
    }

    public bool TakeItem() {
        if (currentInventoryState != InventoryState.FULL) {
            return false;
        }

        currentInventoryState = InventoryState.EMPTY;
        UpdateInventoryVisuals();
        return true;
    }

    private void UpdateInventoryVisuals() {
        _inventoryRenderer.enabled = currentInventoryState == InventoryState.FULL;
    }
}

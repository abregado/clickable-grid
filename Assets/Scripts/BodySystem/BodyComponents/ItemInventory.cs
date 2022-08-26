using System;
using System.Linq;
using UnityEngine;

public class ItemInventory : MonoBehaviour {
    public enum InventoryState {
        EMPTY,
        RESERVED,
        INCOMING,
        FULL
    };

    public Item[] allowedItemTypes;
    public Item currentItem;
    
    [SerializeField]
    private InventoryState _currentInventoryState = InventoryState.EMPTY;
    private Body _owner;
    private SingleSpriteInventoryDisplay _inventoryDisplay;

    private void Awake() {
        _owner = GetComponent<Body>();
        _currentInventoryState = currentItem != null ? InventoryState.FULL: InventoryState.EMPTY;
        _inventoryDisplay = GetComponentInChildren<SingleSpriteInventoryDisplay>();
        _inventoryDisplay.Init();
        UpdateInventoryVisuals();
    }
    
    public bool PutItem(Item incomingItem, bool force = false) {
        if (!CouldAcceptItem(incomingItem) || force) {
            return false;
        }
        
        _currentInventoryState = InventoryState.FULL;
        currentItem = incomingItem;
        UpdateInventoryVisuals();
        return true;
    }

    public bool ReserveSpaceForItem(Item eventualItem, bool force = false) {
        if (!(CouldAcceptItem(eventualItem) || force)) {
            return false;
        }

        _currentInventoryState = InventoryState.INCOMING;
        currentItem = eventualItem;
        UpdateInventoryVisuals();
        return true;
    }

    public bool ReserveItemToTake(Item wantedItem) {
        if (!HasItemToTake(wantedItem)) {
            return false;
        }

        _currentInventoryState = InventoryState.RESERVED;
        UpdateInventoryVisuals();
        return true;
    }

    public bool TakeReservedItem(Item wantedItem) {
        Debug.Log(_currentInventoryState);
        Debug.Log(currentItem);
        Debug.Log(wantedItem);
        if (_currentInventoryState == InventoryState.RESERVED && currentItem == wantedItem) {
            Debug.Log("inventory giving reserved");
            _currentInventoryState = InventoryState.EMPTY;
            currentItem = null;
            UpdateInventoryVisuals();
        }

        return false;
    }

    public bool DeliverItem(Item itemType) {
        if (itemType != currentItem) {
            return false;
        }

        _currentInventoryState = InventoryState.FULL;
        UpdateInventoryVisuals();
        return true;
    }
    
    public Item TakeItem() {
        if (_currentInventoryState != InventoryState.FULL) {
            Debug.Log("didnt take item");
            return null;
        }

        Item itemToReturn = currentItem;
        _currentInventoryState = InventoryState.EMPTY;
        currentItem = null;
        UpdateInventoryVisuals();
        return itemToReturn;
    }

    public bool CouldAcceptItem(Item itemType) {
        return (allowedItemTypes.Contains(itemType) && _currentInventoryState == InventoryState.EMPTY) || (_currentInventoryState == InventoryState.INCOMING && currentItem == itemType);
    }

    public bool HasItemToTake() {
        return currentItem != null && _currentInventoryState == InventoryState.FULL;
    }

    public bool HasItemToTake(Item expectedItem) {
        return currentItem == expectedItem && (_currentInventoryState == InventoryState.FULL || _currentInventoryState == InventoryState.RESERVED) ;
    }

    private void UpdateInventoryVisuals() {
        if (currentItem != null) {
            _inventoryDisplay.UpdateIndicator(currentItem.sprite);
        }
        _inventoryDisplay.SetVisibility(_currentInventoryState);
    }
}

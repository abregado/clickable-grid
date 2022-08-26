using UnityEngine;

public class ItemLandingPad: BodyComponent {
    private ItemInventory _itemInventory;

    public override void Init() {
        base.Init();
        _itemInventory = GetComponent<ItemInventory>();
        Debug.Assert(_itemInventory != null, this.gameObject.name + " is missing an item inventory.");
    }

    
    
    public bool CouldLandItem(Item plannedItem) {
        if (_itemInventory.CouldAcceptItem(plannedItem)) {
            return true;
        }

        return false;
    }

    public bool ReserveLandingSpace(Item plannedItem) {
        if (!CouldLandItem(plannedItem)) {
            return false;
        }

        _itemInventory.ReserveSpaceForItem(plannedItem);
        return true;
    }

    public bool LandItem(Item landedItem) {
        if (_itemInventory.DeliverItem(landedItem)) {
            return true;
        }

        return false;
    }
}

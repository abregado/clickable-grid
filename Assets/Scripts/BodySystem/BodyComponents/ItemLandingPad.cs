using UnityEngine;

public class ItemLandingPad: BodyComponent {
    private ItemInventory _itemInventory;

    public override void Init() {
        base.Init();
        _itemInventory = GetComponent<ItemInventory>();
        Debug.Assert(_itemInventory != null, this.gameObject.name + " is missing an item inventory.");
    }

    public bool LandItem(Item landedItem) {
        if (_itemInventory.PutItem(landedItem)) {
            return true;
        }

        return false;
    }
}

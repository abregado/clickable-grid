using UnityEngine;

public class ItemLandingPad: BodyComponent {
    private ItemInventory _itemInventory;

    public override void Init() {
        base.Init();
        _itemInventory = GetComponent<ItemInventory>();
        Debug.Assert(_itemInventory != null, this.gameObject.name + " is missing an item inventory.");
    }

    public bool LandItem() {
        if (_itemInventory.PutItem()) {
            return true;
        }

        return false;
    }
}

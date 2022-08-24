using UnityEngine;

public class ItemFactory: BodyComponent, IWorldUpdateMember {
    public float respawnTime = 5f;
    public Item itemProduced;
    
    private float _nextUpdate;
    private ItemInventory _itemInventory;
    
    public override void Init() {
        base.Init();
        _itemInventory = GetComponent<ItemInventory>();
        Debug.Assert(itemProduced != null, "ItemFactory requires you to set an item to produce.");
        _nextUpdate = Time.time + respawnTime;
        SceneAccess.instance.worldSystem.RegisterMember(this);
    }

    public void WorldUpdate() {
        if (Time.time >= _nextUpdate) {
            if (_itemInventory.CouldAcceptItem(itemProduced)) {
                _itemInventory.PutItem(itemProduced);
                _nextUpdate += respawnTime;
            }
        }
    }
}

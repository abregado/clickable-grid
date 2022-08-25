using UnityEngine;

public class ItemEater: BodyComponent, IWorldUpdateMember {
    public float respawnTime = 5f;
    
    private float _nextUpdate;
    private ItemInventory _itemInventory;
    
    public override void Init() {
        base.Init();
        _itemInventory = GetComponent<ItemInventory>();
        _nextUpdate = Time.time + respawnTime;
        SceneAccess.instance.worldSystem.RegisterMember(this);
    }

    public void WorldUpdate() {
        if (Time.time >= _nextUpdate) {
            if (_itemInventory.HasItemToTake()) {
                _itemInventory.TakeItem();
            }
            _nextUpdate += respawnTime;
        }
    }
}

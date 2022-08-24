using UnityEngine;

public class SingleSpriteInventoryDisplay: MonoBehaviour {
    private SpriteRenderer _inventoryRenderer;

    public void Awake() {
        _inventoryRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    public void UpdateIndicator(Sprite sprite) {
        _inventoryRenderer.sprite = sprite;
    }

    public void SetVisibility(ItemInventory.InventoryState state) {
        switch (state) {
                case ItemInventory.InventoryState.FULL:
                    _inventoryRenderer.color = Color.white;
                    _inventoryRenderer.enabled = true;
                    break;
                case ItemInventory.InventoryState.INCOMING:
                    _inventoryRenderer.enabled = true;
                    _inventoryRenderer.color = new Color(255,125,0,50);
                    break;
                default:
                    _inventoryRenderer.enabled = false;
                    break;
        }
    }
    
}

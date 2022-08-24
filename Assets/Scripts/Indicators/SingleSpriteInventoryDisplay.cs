using UnityEngine;

public class SingleSpriteInventoryDisplay: MonoBehaviour {
    private SpriteRenderer _inventoryRenderer;

    public void Awake() {
        _inventoryRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    
    public void UpdateIndicator(Sprite sprite) {
        _inventoryRenderer.sprite = sprite;
    }

    public void SetVisibility(bool state) {
        _inventoryRenderer.enabled = state;
    }
    
}

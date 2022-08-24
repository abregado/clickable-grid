using UnityEngine;

public class RecipeCooker: BodyComponent, IWorldUpdateMember {
    private const float INVENTORY_CHECK_TIME = 2f;
    public Recipe[] recipes;
    public Recipe currentRecipe;
    
    private ItemInventory _itemInventory;
    private float _nextUpdate;

    public override void Init() {
        base.Init();
        _itemInventory = GetComponent<ItemInventory>();
        foreach (Recipe recipe in recipes) {
            if (!_itemInventory.CouldAcceptItem(recipe.ingredient)) {
                Debug.Log(gameObject.name + " has a recipe requiring " + recipe.ingredient.prettyName + " but its inventory cannot accept that item");
            }
        }
        SceneAccess.instance.worldSystem.RegisterMember(this);
    }


    public void WorldUpdate() {
        if (Time.time >= _nextUpdate) {
            if (currentRecipe) {
                _itemInventory.DeliverItem(currentRecipe.product);
                _nextUpdate += INVENTORY_CHECK_TIME;
                currentRecipe = null;
            }
            else {
                currentRecipe = FindRecipeToMakeWithInventory();
                if (currentRecipe != null) {
                    _itemInventory.TakeItem();
                    _itemInventory.ReserveSpaceForItem(currentRecipe.product,true);
                    _nextUpdate += currentRecipe.cookTime;
                }
                else {
                    _nextUpdate += INVENTORY_CHECK_TIME;
                }    
            }
            
        }
    }

    private Recipe FindRecipeToMakeWithInventory() {
        foreach (Recipe recipe in recipes) {
            if (_itemInventory.HasItemToTake(recipe.ingredient)) {
                return recipe;
            }
        }

        return null;
    }
}

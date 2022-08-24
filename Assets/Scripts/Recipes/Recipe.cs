using UnityEngine;

[CreateAssetMenu(fileName = "Recipe", menuName = "ScriptableObjects/Recipe", order = 1)]
public class Recipe : ScriptableObject {
    public string prettyName = "default";
    public Item ingredient;
    public Item product;
    public float cookTime = 5f;
}

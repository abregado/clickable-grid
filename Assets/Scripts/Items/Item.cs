using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects", order = 1)]
public class Item : ScriptableObject {
    public string prettyName = "default";
    public Sprite sprite;
}

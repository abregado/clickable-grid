using UnityEngine;

public class SceneAccess : MonoBehaviour {
    public static SceneAccess instance { get; private set; }
    
    public Grid grid { get; private set;}
    public BodySystem bodySystem { get; private set;}
    public WorldSystem worldSystem { get; private set;}
    
    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(this);
            return;
        }
        instance = this;
        grid = FindObjectOfType<Grid>();
        bodySystem = FindObjectOfType<BodySystem>();
        worldSystem = FindObjectOfType<WorldSystem>();
    }
}

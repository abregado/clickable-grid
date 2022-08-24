using UnityEngine;

public class ButtonAction : MonoBehaviour {
    protected LocalPlayer _player;
    
    public virtual void Init(LocalPlayer player, BodySystem bodySystem) {
        _player = player;
    }

    public virtual bool Interact(Vector3Int cell) {
        if (SceneAccess.instance.bodySystem.IsPlayerAlreadyControllingSomething(_player)) {
            return false;
        }
        
        Body target = SceneAccess.instance.bodySystem.GetBodyAtCell(cell);

        if (target != null) {
            Debug.Log("You interacted with " + target.gameObject.name);
            return true;
        }

        return true;
    }

    public virtual bool Cancel() {
        return false;
    }

    
}

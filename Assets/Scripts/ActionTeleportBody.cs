using UnityEngine;

public class ActionTeleportBody: ButtonAction {

    private Body _movedBody;
    private Teleportable _bodyInteraction;
    
    public override void Init(LocalPlayer player, BodySystem bodySystem) {
        base.Init(player, bodySystem);
    }

    public override bool Interact(Vector3Int cell) {
        if (_movedBody) {
            Body target = SceneAccess.instance.bodySystem.GetBodyAtCell(cell);

            if (target == null && _bodyInteraction.CompleteTeleporting(cell)){
                Cancel();
                return true;
            }
        }
        else {
            if (SceneAccess.instance.bodySystem.IsPlayerAlreadyControllingSomething(_player)) {
                Debug.Log("Player is already interacting with something.");
                return false;
            }
            
            Body target = SceneAccess.instance.bodySystem.GetBodyAtCell(cell);

            if (target != null && target.CanBeInteractedWith()) {
                Teleportable neededComponent = target.GetComponent<Teleportable>();
                if (neededComponent != null) {
                    _movedBody = target;
                    _bodyInteraction = neededComponent;
                    return _bodyInteraction.StartTeleporting(_player);
                }
            }
        }

        return false;
    }

    public override bool Cancel() {
        if (_movedBody != null) {
            _bodyInteraction.CancelTeleporting();
            _movedBody = null;
            _bodyInteraction = null;
            return true;
        }

        return false;
    }
}

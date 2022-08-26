using UnityEngine;

public class ActionSendItem: ButtonAction {

    private Body _fromBody;
    private ItemLaunchPad _launchPad;
    
    public override void Init(LocalPlayer player, BodySystem bodySystem) {
        base.Init(player, bodySystem);
    }

    public override bool Interact(Vector3Int cell) {
        if (_fromBody) {
            Body target = SceneAccess.instance.bodySystem.GetBodyAtCell(cell);

            if (target != null) {
                ItemLandingPad landingPad = target.GetComponent<ItemLandingPad>();
                if (landingPad != null && landingPad.CouldLandItem(_launchPad.LaunchedItem())) {
                    if (SceneAccess.instance.postalSystem.RequestDelivery(_fromBody.bodyCell, target.bodyCell,
                            _launchPad.LaunchedItem())) {
                        _launchPad.ClaimItemForShipping(_launchPad.LaunchedItem());
                        landingPad.ReserveLandingSpace(_launchPad.LaunchedItem());
                        Cancel();
                        return true;    
                    }
                    
                }
            }
        }
        else {
            if (SceneAccess.instance.bodySystem.IsPlayerAlreadyControllingSomething(_player)) {
                Debug.Log("Player "+_player.rewiredPlayerID+"is already interacting with something.");
                return false;
            }
            
            Body target = SceneAccess.instance.bodySystem.GetBodyAtCell(cell);

            if (target != null) {
                ItemLaunchPad itemLaunchPad = target.GetComponent<ItemLaunchPad>();
                if (itemLaunchPad != null && itemLaunchPad.StartSending(_player)) {
                    _fromBody = target;
                    _launchPad = itemLaunchPad;
                    return true;
                }
            }
        }

        return false;
    }

    public override bool Cancel() {
        if (_fromBody != null) {
            _launchPad.CancelSending();
            _fromBody = null;
            _launchPad = null;
            return true;
        }

        return false;
    }
}
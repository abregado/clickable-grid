using DG.Tweening;
using UnityEngine;

public class PostalWorker: MonoBehaviour {
    private const float ALTITUDE = 2f;
    private const float MOVE_SPEED_EMPTY = 5f;
    private const float MOVE_SPEED_FULL = 1f;
    private const float TAKE_OFF_EMPTY_TIME = 0.5f;
    private const float TAKE_OFF_FULL_TIME = 1f;
    private const float LAND_EMPTY_TIME = 0.5f;
    private const float LAND_FULL_TIME = 1f;
    private const float LANDING_WAIT = 0.5f;
    
    private Vector3Int _source;
    private Vector3Int _dest;
    public Vector3Int home;
    public bool idle;

    private Sequence _moveTween;
    
    public void Init() {
        home = SceneAccess.instance.grid.WorldToCell(transform.position);
        idle = true;
    }

    public float SendOnRoute(Vector3Int sourceCell, Vector3Int destCell) {
        Vector3 source = SceneAccess.instance.grid.GetCellCenterWorld(sourceCell);
        Vector3 dest = SceneAccess.instance.grid.GetCellCenterWorld(destCell);
        Vector3 home = SceneAccess.instance.grid.GetCellCenterWorld(this.home);

        float leg1Time = Vector3.Distance(home, source) / MOVE_SPEED_EMPTY;
        float leg2Time = Vector3.Distance(source, dest) / MOVE_SPEED_FULL;
        float leg3Time = Vector3.Distance(dest, home) / MOVE_SPEED_EMPTY;

        float totalTime = leg1Time + leg2Time + leg3Time + TAKE_OFF_FULL_TIME + TAKE_OFF_EMPTY_TIME +
                          (2 * LAND_EMPTY_TIME + TAKE_OFF_EMPTY_TIME + LANDING_WAIT);
        
        Transform tform = transform;
        
        _moveTween = DOTween.Sequence();
        _moveTween.Append(tform.DOMove(home + (Vector3.up * ALTITUDE), TAKE_OFF_EMPTY_TIME))
            .Append(tform.DOMove(source + (Vector3.up * ALTITUDE), leg1Time).SetEase(Ease.InOutQuad))
            .Append(tform.DOMove(source, LAND_EMPTY_TIME).SetEase(Ease.Linear).SetDelay(LANDING_WAIT))
            .AppendCallback(MakePickup)
            .Append(tform.DOMove(source + (Vector3.up * ALTITUDE), TAKE_OFF_FULL_TIME).SetEase(Ease.Linear))
            .Append(tform.DOMove(dest + (Vector3.up * ALTITUDE), leg2Time).SetEase(Ease.InOutQuad))
            .Append(tform.DOMove(dest, LAND_FULL_TIME).SetEase(Ease.Linear).SetDelay(LANDING_WAIT))
            .AppendCallback(MakeDropOff)
            .Append(tform.DOMove(dest + (Vector3.up * ALTITUDE), TAKE_OFF_EMPTY_TIME).SetEase(Ease.Linear))
            .Append(tform.DOMove(home + (Vector3.up * ALTITUDE), leg3Time).SetEase(Ease.InOutQuad))
            .Append(tform.DOMove(home, LAND_EMPTY_TIME).SetEase(Ease.Linear))
            .AppendCallback(FinishRoute)
            .Play();

        idle = false;
        return totalTime;
    }

    private void MakeDropOff() {
        SceneAccess.instance.postalSystem.MakeDropOff(this);
    }
    
    private void MakePickup() {
        Debug.Log("made pickup");
        SceneAccess.instance.postalSystem.MakePickup(this);
    }
    
    private void FinishRoute() {
        SceneAccess.instance.postalSystem.RouteComplete(this);
        idle = true;
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class Body: MonoBehaviour {

    public Vector3Int bodyCell;
    public  LocalPlayer controllingPlayer;

    public virtual void Init(Vector3Int bodyCell) {
        this.bodyCell = bodyCell;

        BodyComponent[] bodyComponents = GetComponents<BodyComponent>();
        foreach (BodyComponent bodyComponent in bodyComponents) {
            bodyComponent.Init();
        }
        
        CenterOnCell();
    }
    
    private void CenterOnCell() {
        transform.position = FindObjectOfType<Grid>().GetCellCenterWorld(bodyCell);
    }

    public void Move(Vector3Int dest) {
        bodyCell = dest;
        CenterOnCell();
    }

    public bool CanBeInteractedWith() {
        return controllingPlayer == null;
    }
}


using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class WorldSystem: MonoBehaviour {
    private List<IWorldUpdateMember> _members;

    void Awake() {
        _members = new List<IWorldUpdateMember>();
    }

    public void RegisterMember(IWorldUpdateMember newMember) {
        if (_members.Contains(newMember)) {
            Debug.LogError("Attempt made by " + newMember + " to register a WorldUpdateMember twice.");
            return;
        }
        
        _members.Add(newMember);
    }

    public void DeregisterMember(IWorldUpdateMember member) {
        _members.Remove(member);
    }

    public void Update() {
        foreach (IWorldUpdateMember worldUpdateMember in _members) {
            worldUpdateMember.WorldUpdate();
        }
    }
}

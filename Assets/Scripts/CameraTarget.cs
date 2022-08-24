using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTarget : MonoBehaviour {
    public float defaultWeight = 1f;
    public float defaultRadius = 3f;
    
    private CinemachineTargetGroup _targetGroup;
    private Transform _levelCenter;

    private void Awake() {
        _levelCenter = GameObject.Find("LevelCenter").transform;
        _targetGroup = GetComponent<CinemachineTargetGroup>();
        _targetGroup.AddMember(_levelCenter,defaultWeight,defaultRadius);
    }

    public void AddMember(Transform member) {
        _targetGroup.AddMember(member,defaultWeight,defaultRadius);
        if (_targetGroup.m_Targets.Length == 2 && _targetGroup.FindMember(_levelCenter) > -1) {
            RemoveMember(_levelCenter);
        }
    }

    public void RemoveMember(Transform member) {
        _targetGroup.RemoveMember(member);
        if (_targetGroup.m_Targets.Length == 0) {
            _targetGroup.AddMember(_levelCenter,1f,1f);
        }
    }

    public void ModifyMember(Transform member, float weight, float radius) {
        int memberIndex = _targetGroup.FindMember(member);

        if (memberIndex == -1) {
            return;
        }
        
        var targets = _targetGroup.m_Targets;
        targets[memberIndex].radius = radius;
        targets[memberIndex].weight = weight;
        _targetGroup.m_Targets = targets;
    }
}

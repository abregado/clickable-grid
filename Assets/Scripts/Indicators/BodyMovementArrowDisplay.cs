using System;
using UnityEngine;
using UnityEngine.VFX;

public class BodyMovementArrowDisplay : MonoBehaviour {
    private LineRenderer _lineRenderer;
    private VisualEffect _effect;
    private Transform _pos1;
    private Transform _pos2;
    private Transform _pos3;
    
    private void Awake() {
        _lineRenderer = GetComponent<LineRenderer>();
        // _effect = transform.Find("VFXLine/vfxLine").GetComponent<VisualEffect>();
        // _pos1 = transform.Find("VFXLine/pos1");
        // _pos2 = transform.Find("VFXLine/pos2");
        // _pos3 = transform.Find("VFXLine/pos3");
        SetVisibility(false);
    }

    public void SetVisibility(bool visible) {
        if (visible) {
            //_effect.enabled = true;
            _lineRenderer.enabled = true;
            _lineRenderer.SetPositions(new Vector3[]{transform.position,transform.position});
            return;
        }

        //_effect.enabled = false;
        _lineRenderer.enabled = false;
        _lineRenderer.SetPositions(new Vector3[]{});
    }

    public void UpdateDisplay(Vector3 from, Vector3 to) {
        _lineRenderer.SetPosition(0,from);
        _lineRenderer.SetPosition(1,to);
        
        // Vector3 vector = (to - from).normalized;
        // float distance = Vector3.Distance(from, to);
        //
        // _pos1.position = from + (vector * distance * 0.25f) + Vector3.up;
        // _pos2.position = from + (vector * distance * 0.75f) + Vector3.up;
        //
        // _pos3.position = to;
    }

    public void SetColor(Color indicatorColor) {
        _lineRenderer.startColor = indicatorColor;
        _lineRenderer.endColor = indicatorColor;
        //_effect.SetVector4("particleColor",indicatorColor);
    }
}

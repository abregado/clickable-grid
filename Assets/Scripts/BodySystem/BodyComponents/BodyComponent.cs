using UnityEngine;

public class BodyComponent: MonoBehaviour {
    protected Body _body;

    public virtual void Init() {
        _body = GetComponent<Body>();
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PostalSystem: MonoBehaviour {
    private List<PostalWorker> _workers;
    private Dictionary<PostalWorker, PostalRoute> _activeRoutes;

    void Awake() {
        _workers = new List<PostalWorker>();
        
    }

    private void Start() {
        PostalWorker[] sceneWorkers = FindObjectsOfType<PostalWorker>();

        foreach (PostalWorker worker in sceneWorkers) {
            RegisterWorker(worker);
        }

        _activeRoutes = new Dictionary<PostalWorker, PostalRoute>();
    }

    private void RegisterWorker(PostalWorker newWorker) {
        if (_workers.Contains(newWorker)) {
            return;
        }
        
        _workers.Add(newWorker);
        newWorker.Init();
    }

    public bool RequestDelivery(Vector3Int source, Vector3Int dest, Item deliveryType) {
        float shortestDistance = 999f;
        PostalWorker closestWorker = null;
        
        foreach (PostalWorker postalWorker in _workers) {
            if (postalWorker.idle) {
                float thisDist = Vector3Int.Distance(postalWorker.home, source);
                if (thisDist < shortestDistance) {
                    closestWorker = postalWorker;
                    shortestDistance = thisDist;
                }
            }
        }

        if (closestWorker == null) {
            return false;
        }
        
        _activeRoutes.Add(closestWorker,new PostalRoute(deliveryType,source,dest));
        closestWorker.SendOnRoute(source,dest);
        return true;
    }

    private class PostalRoute {
        public Item deliveryType;
        public Vector3Int source;
        public Vector3Int dest;

        public PostalRoute(Item deliveryType, Vector3Int source, Vector3Int dest) {
            this.deliveryType = deliveryType;
            this.source = source;
            this.dest = dest;
        }
    }

    public void MakePickup(PostalWorker worker) {
        Debug.Log("system registering pickup");
        Debug.Assert(_activeRoutes.ContainsKey(worker), "Worker making a pickup on an inactive route");
        PostalRoute route = _activeRoutes[worker];
        ItemLaunchPad pad = SceneAccess.instance.bodySystem.GetBodyAtCell(route.source)?.GetComponent<ItemLaunchPad>();
        if (pad != null) {
            pad.CompleteSending(route.deliveryType);
        }
    }

    public void MakeDropOff(PostalWorker worker) {
        Debug.Assert(_activeRoutes.ContainsKey(worker), "Worker making a dropoff on an inactive route");
        PostalRoute route = _activeRoutes[worker];
        ItemLandingPad pad = SceneAccess.instance.bodySystem.GetBodyAtCell(route.dest)?.GetComponent<ItemLandingPad>();
        if (pad != null) {
            pad.LandItem(route.deliveryType);
        }
    }

    public void RouteComplete(PostalWorker worker) {
        Debug.Assert(_activeRoutes.ContainsKey(worker), "Worker reporting complete on an inactive route");
        _activeRoutes.Remove(worker);
    }
}

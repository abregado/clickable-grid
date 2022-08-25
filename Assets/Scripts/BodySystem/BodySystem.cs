using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class BodySystem : MonoBehaviour {
    private Dictionary<Vector3Int, Body> _bodies;
    private List<LocalPlayer> _interactingPlayers;

    void Awake() {
        _bodies = new Dictionary<Vector3Int, Body>();
        _interactingPlayers = new List<LocalPlayer>();
    }

    private void Start() {
        Body[] sceneBodies = FindObjectsOfType<Body>();

        foreach (Body sceneBody in sceneBodies) {
            RegisterBody(sceneBody);
        }
    }

    [CanBeNull]
    public Body GetBodyAtCell(Vector3Int cell) {
        if (_bodies.ContainsKey(cell)) {
            return _bodies[cell]; 
        }

        return null;
    }

    public void RegisterBody(Body unregisteredBody) {
        //Check that this Body is not already registered
        foreach (KeyValuePair<Vector3Int,Body> keyValuePair in _bodies) {
            if (keyValuePair.Value == unregisteredBody) {
                Debug.LogError("The Body on " + unregisteredBody.gameObject.name + " is trying to registered itself, although it is already registered.");
                return;
            }
        }
        //check there is not another Body registered at this location
        Vector3Int bodyCell = SceneAccess.instance.grid.WorldToCell(unregisteredBody.transform.position);
        if (_bodies.ContainsKey(bodyCell)) {
            Debug.LogError("The Body on " + unregisteredBody.gameObject.name + " is trying to register itself, although another Body is already registered at the same location (" + bodyCell.ToString() + ")" );
            return;
        }
        
        //register and initialize the Body
        _bodies.Add(bodyCell,unregisteredBody);
        unregisteredBody.Init(bodyCell);
    }

    public bool MoveBody(Body movedBody, Vector3Int dest) {
        if (_bodies.ContainsKey(dest)) {
            return false;
        }

        if (CheckBodyIsRegistered(movedBody)) {
            Debug.Log("Registered body was moved");
            Body removedBody;
            _bodies.Remove(movedBody.bodyCell, out removedBody);
            _bodies.Add(dest,removedBody);
            removedBody.Move(dest);
            return true;
        }

        Debug.Log("Unregistered body was moved");
        _bodies.Add(dest,movedBody);
        movedBody.Init(dest);
        return true;
        
    }

    private bool CheckBodyIsRegistered(Body subject) {
        return _bodies.ContainsValue(subject);
    }

    public bool ClaimControllershipOfBody(Vector3Int cell, LocalPlayer player) {
        if (!_bodies.ContainsKey(cell)) {
            return false;
        }
        
        if (_interactingPlayers.Contains(player)) {
            return false;
        }

        if (_bodies[cell].controllingPlayer != null) {
            return false;
        }

        _bodies[cell].controllingPlayer = player;
        _interactingPlayers.Add(player);
        return true;
    }
    
    public bool ClaimControllershipOfBody(Body body, LocalPlayer player) {
        if (!CheckBodyIsRegistered(body)) {
            Debug.Log("Body " + body.gameObject.name + " tried to give control to player "+player.rewiredPlayerID+" but it was not registered.");
            return false;
        }

        if (_interactingPlayers.Contains(player)) {
            Debug.Log("Body " + body.gameObject.name + " tried to give control to player "+player.rewiredPlayerID+" but the player was already controlling something.");
            return false;
        }

        if (body.controllingPlayer != null) {
            Debug.Log("Body " + body.gameObject.name + " tried to give control to player "+player.rewiredPlayerID+" but was already being controlled by player " + body.controllingPlayer.rewiredPlayerID);
            return false;
        }

        Debug.Log("Player " + player.rewiredPlayerID + " gained control of " + body.gameObject.name );
        body.controllingPlayer = player;
        _interactingPlayers.Add(player);
        return true;
    }
    
    public bool RelinquishControllership(Body body) {
        if (!_interactingPlayers.Contains(body.controllingPlayer)) {
            Debug.Log("Body " + body.gameObject.name + " tried to relinquish control for player "+body.controllingPlayer.rewiredPlayerID+" but was not controlling anything.");
            return false;
        }
        
        if (!CheckBodyIsRegistered(body)) {
            Debug.Log("Body " + body.gameObject.name + " tried to relinquish control for player "+body.controllingPlayer.rewiredPlayerID+" but was not registered.");
            return false;
        }
        
        Debug.Log("Relinquishing control of " + body.gameObject.name + body.controllingPlayer.rewiredPlayerID);
        _interactingPlayers.Remove(body.controllingPlayer);
        body.controllingPlayer = null;
        return true;
    }

    public bool IsPlayerAlreadyControllingSomething(LocalPlayer player) {
        return _interactingPlayers.Contains(player);
    }
}

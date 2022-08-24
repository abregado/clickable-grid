using System;
using Cinemachine;
using Rewired;
using Unity.VisualScripting;
using UnityEngine;

public class LocalPlayer : MonoBehaviour {
    public int rewiredPlayerID = -1;
    public float moveSpeed = 3.0f;
    public float fastMod = 3.0f;
    public Vector3Int selectedCell;
    public Color playerColor;

    private Camera _camera;
    private Grid _gameGrid;
    private BodySystem _bodySystem;
    private bool _isActive;
    private Player _rewiredPlayer;
    private Vector3 _inputVector;
    private bool _interactInput;
    private bool _backInput;
    private bool _speedInput;
    private bool _joinInput;
    private Transform _selectionTransform;
    private SpriteRenderer _selectionRenderer;
    private SpriteRenderer _cursorRenderer;
    private CameraTarget _cameraTarget;

    private ButtonAction _buttonAction;
    
    // Start is called before the first frame update
    void Awake()
    {
        Debug.Assert(rewiredPlayerID > -1,"Player script is missing a rewired ID.");
        _rewiredPlayer = ReInput.players.GetPlayer(rewiredPlayerID);
        _gameGrid = FindObjectOfType<Grid>();
        _selectionTransform = transform.Find("Selected");
        _selectionRenderer = _selectionTransform.GetComponent<SpriteRenderer>();
        _selectionRenderer.color = playerColor;
        _cursorRenderer = transform.Find("Cursor").GetComponent<SpriteRenderer>();
        _cursorRenderer.color = playerColor;
        _cameraTarget = FindObjectOfType<CameraTarget>();
        _buttonAction = GetComponent<ButtonAction>();
        _bodySystem = FindObjectOfType<BodySystem>();
        if (_buttonAction != null) {
            _buttonAction.Init(this, _bodySystem);
        }
        _camera = Camera.main;
    }

    private void Start() {
        SetPlayerInactive();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        if (_isActive) {
            ProcessInput();
            UpdateSelectedCell();
            UpdateZoom();
        }
        else {
            CheckForJoin();
        }
        
    }


    private void GetInput() {
        _inputVector.x = _rewiredPlayer.GetAxis("MoveHorizontal");
        _inputVector.z = _rewiredPlayer.GetAxis("MoveVertical");
        _interactInput = _rewiredPlayer.GetButtonDown("Interact");
        _backInput = _rewiredPlayer.GetButtonDown("Back");
        _joinInput = _rewiredPlayer.GetButtonDown("Join");
        _speedInput = _rewiredPlayer.GetButton("Speed");
    }

    private void ProcessInput() {
        if (_joinInput) {
            SetPlayerInactive();
            return;
        }

        if (_interactInput && _buttonAction != null) {
            _buttonAction.Interact(selectedCell);
        }
        
        if (_backInput && _buttonAction != null) {
            _buttonAction.Cancel();
        }
        
        float tempMoveSpeed = moveSpeed;
        if (_speedInput) {
            tempMoveSpeed *= fastMod;
        }
        
        if (_inputVector.x != 0.0f || _inputVector.y != 0.0f) {
            Vector3 worldDir = _camera.transform.TransformDirection(new Vector3(_inputVector.x,_inputVector.z,0));
            transform.position += (worldDir * tempMoveSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
    }

    private void UpdateSelectedCell() {
        if (_isActive) {
            selectedCell = _gameGrid.WorldToCell(transform.position);
            _selectionTransform.position = _gameGrid.GetCellCenterWorld(selectedCell);
            _selectionRenderer.enabled = !_speedInput;
        }
    }

    private void SetPlayerActive() {
        _isActive = true;
        _cursorRenderer.enabled = true;
        selectedCell = Vector3Int.zero;
        transform.position = _gameGrid.GetCellCenterWorld(selectedCell);
        _cameraTarget.AddMember(transform);
    }

    private void SetPlayerInactive() {
        _isActive = false;
        _cursorRenderer.enabled = false;
        _selectionRenderer.enabled = false;
        _cameraTarget.RemoveMember(transform);
    }

    private void CheckForJoin() {
        if (_joinInput || _interactInput || _backInput || _inputVector.x > 0.5f || _inputVector.y > 0.5f) {
            SetPlayerActive();
        }
    }

    private void UpdateZoom() {
        bool startIn = _rewiredPlayer.GetButtonDown("ZoomIn");
        bool startOut = _rewiredPlayer.GetButtonDown("ZoomOut");
        bool endIn = _rewiredPlayer.GetButtonUp("ZoomIn");
        bool endOut = _rewiredPlayer.GetButtonUp("ZoomOut");
        bool stateIn = _rewiredPlayer.GetButton("ZoomIn");
        bool stateOut = _rewiredPlayer.GetButton("ZoomOut");

        if ((endIn && !stateOut) || (endOut && !stateIn)) {
            //reset zoom
            ChangeCameraTargetSettings(1f,3f);
            return;
        }

        if (startIn) {
            //zoom in
            ChangeCameraTargetSettings(5f,0.1f);
            return;
        }

        if (startOut) {
            //zoom out
            ChangeCameraTargetSettings(1f,10f);
        }
    }

    private void ChangeCameraTargetSettings(float weight, float radius) {
        _cameraTarget.ModifyMember(transform,weight,radius);
    }
}

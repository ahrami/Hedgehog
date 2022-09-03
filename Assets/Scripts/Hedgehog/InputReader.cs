using UnityEngine;

[DisallowMultipleComponent]

public class InputReader : MonoBehaviour {

    [SerializeField] private KeyCode 
        _upKey = KeyCode.W;
    [SerializeField] private KeyCode 
        _downKey = KeyCode.S;
    [SerializeField] private KeyCode 
        _leftKey = KeyCode.A;
    [SerializeField] private KeyCode 
        _rightKey = KeyCode.D;

    [SerializeField] private KeyCode 
        _actionKey = KeyCode.Q;
    [SerializeField] private KeyCode 
        _curlKey = KeyCode.C;
    [SerializeField] private KeyCode 
        _reverseKey = KeyCode.Space;

    private bool _up = false;
    private bool _down = false;
    private bool _left = false;
    private bool _right = false;

    private bool _reverse = false;

    private Vector2Int _inputVector;

    private void Start() {
        _inputVector = Vector2Int.zero;
    }

    private void Update() {

		if (Input.GetKeyDown(_upKey)) {
            _up = true;
            _inputVector.y = 1;
        }
        if (Input.GetKeyUp(_upKey)) {
            _up = false;
            if (_inputVector.y == 1) {
                if (_down) {
                    _inputVector.y = -1;
                } else {
                    _inputVector.y = 0;
                }
            }
        }

        if (Input.GetKeyDown(_downKey)) {
            _down = true;
            _inputVector.y = -1;
        }
        if (Input.GetKeyUp(_downKey)) {
            _down = false;
            if (_inputVector.y == -1) {
                if (_up) {
                    _inputVector.y = 1;
                } else {
                    _inputVector.y = 0;
                }
            }
        }

        if (Input.GetKeyDown(_leftKey)) {
            _left = true;
            _inputVector.x = -1;
        }
        if (Input.GetKeyUp(_leftKey)) {
            _left = false;
            if (_inputVector.x == -1) {
                if (_right) {
                    _inputVector.x = 1;
                } else {
                    _inputVector.x = 0;
                }
            }
        }

        if (Input.GetKeyDown(_rightKey)) {
            _right = true;
            _inputVector.x = 1;
        }
        if (Input.GetKeyUp(_rightKey)) {
            _right = false;
            if (_inputVector.x == 1) {
                if (_left) {
                    _inputVector.x = -1;
                } else {
                    _inputVector.x = 0;
                }
            }
        }
    }

    public Vector2Int GetInputVector() {
        if (_reverse) {
            return _inputVector * (-1);
        }
        return _inputVector;
    }
    public void Backwards() {
        _reverse = true;
	}
    public void Forward() {
        _reverse = false;
    }
    public bool GetCurlKeyDown() {
        return Input.GetKeyDown(_curlKey);
	}
    public bool GetActionKey() {
        return Input.GetKey(_actionKey);
    }
    public bool GetReverse() {
        return Input.GetKey(_reverseKey);
    }
}

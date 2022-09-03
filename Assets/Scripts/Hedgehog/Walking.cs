using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Rigidbody))]

public class Walking : MonoBehaviour {
    [Range(0.0f, 10.0f)]
    [SerializeField] private float _maxSpeed = 5f;
    [Range(0.0f, 40.0f)]
    [SerializeField] private float _acceleration = 5f;
    [Range(0.0f, 40.0f)]
    [SerializeField] private float _deceleration = 5f;
    [Range(0.0f, 5.0f)]
    [SerializeField] private float _drag = 1f;

    private float _speed;
    private float _speedMultiplier;
    private bool _moving;

    private Vector2 _inputVector;
    private Vector3 _velocity;

    private InputReader _inputReader;
    private Rigidbody _rigidbody;

    private void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _inputReader = gameObject.GetComponent<InputReader>();

        _speed = 0f;
        _speedMultiplier = 0f;
        _moving = false;

        _inputVector = Vector2.zero;
        _velocity = Vector3.zero;
    }

    private void FixedUpdate() {
        _inputVector = _inputReader.GetInputVector();
        _velocity = Quaternion.Inverse(_rigidbody.rotation) * _rigidbody.velocity;
        _velocity.x -= _velocity.x * _maxSpeed * _drag * Time.deltaTime;
        _speed = _velocity.z;
        if (_inputVector.magnitude != 0 && _moving) {
            Accelerate();
        } else {
            Decelerate();
        }
        _velocity.z = _speed;
        _velocity = _rigidbody.rotation * _velocity;
        if (_moving) {
            _rigidbody.velocity = _velocity;
        }
    }

    private void Accelerate() {
        float maxVelocity = _maxSpeed * _speedMultiplier;
        if (maxVelocity >= 0f) {
            if (_speed < maxVelocity) {
                _speed += _acceleration * Time.deltaTime;
                if (_speed > maxVelocity) {
                    Decelerate();
                    if (_speed < maxVelocity) {
                        _speed = maxVelocity;
                    }
                }
            }
        } else {
            if (_speed > maxVelocity) {
                _speed -= _acceleration * Time.deltaTime;
                if (_speed < maxVelocity) {
                    Decelerate();
                    if (_speed > maxVelocity) {
                        _speed = maxVelocity;
                    }
                }
            }
        }
    }

    private void Decelerate() {
        if (_speed > 0f) {
            _speed -= _deceleration * Time.deltaTime;
            if (_speed < 0f) {
                _speed = 0f;
            }
        } else if (_speed < 0f) {
            _speed += _deceleration * Time.deltaTime;
            if (_speed > 0f) {
                _speed = 0f;
            }
        }
    }

    public void SetMultiplier(float multiplier) {
        _speedMultiplier = multiplier;
	}

    public void EnableWalking() {
        _moving = true;
    }

    public void DisableWalking() {
        _moving = false;
    }

    public float GetWalkSpeed() {
        return _speed;
	}
}

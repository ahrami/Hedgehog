using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Rigidbody))]

public class Turning : MonoBehaviour {

    [Range(0.0f, 20.0f)]
    [SerializeField] private float _maxAngularSpeed = 10f;
    [Range(0.0f, 50.0f)]
    [SerializeField] private float _angularAcceleration = 30f;
    [Range(0.0f, 50.0f)]
    [SerializeField] private float _angularDeceleration = 30f;
    [Range(0.0f, 0.5f)]
    [SerializeField] private float _slowingDownInterval = 0.4f;
    [Range(0.0f, 10f)]
    [SerializeField] private float _drag = 1f;

    private float _slowingDownStartVelocity;
    private float _maxAngularVelocity;
    private float _targetAngle;
    private float _turningSector;
    private float _remainingSector;
    private float _remainingSectorFraction;
    private float _turningDirection;
    private float _angularSpeedMultiplier;
    private bool _approachingTargetAngle;
    private bool _moving;
    
    private Vector2 _inputVector;
    private Vector3 _localTargetVector;
    private Vector3 _localAngularVelocity;

    private InputReader _input;
    private Rigidbody _rigidbody;

    private void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _input = gameObject.GetComponent<InputReader>();

        _inputVector = Vector2.zero;
        _localTargetVector = Vector3.zero;
        _localAngularVelocity = Vector3.zero;

        _moving = false;
        _approachingTargetAngle = false;
        _slowingDownStartVelocity = 0f;
        _maxAngularVelocity = 0f;
        _targetAngle = 0f;
        _turningSector = 1f; //1 is to make the first turn
        _remainingSector = 0f;
        _remainingSectorFraction = 0f;
        _turningDirection = 0f;
        _angularSpeedMultiplier = 1f;
    }

    private void FixedUpdate() {
        _inputVector = _input.GetInputVector();

        //Angular velocity relative to hedgehog
        _localAngularVelocity = 
            Quaternion.Inverse(_rigidbody.rotation) 
            * _rigidbody.angularVelocity;

        if (_inputVector.magnitude != 0 && _moving) {
            float oldTargetAngle = _targetAngle;

            //Target angle
            _localTargetVector = new Vector3(_inputVector.x, 0, _inputVector.y);
            _targetAngle = Quaternion.FromToRotation(
                Vector3.forward, _localTargetVector
                ).eulerAngles.y;

            //Remaining sector
            _remainingSector = _targetAngle - _rigidbody.rotation.eulerAngles.y;

            if (_remainingSector < -180) {
                _remainingSector += 360;
            } else if (_remainingSector > 180) {
                _remainingSector -= 360;
            }

            //Whole sector
            if (oldTargetAngle != _targetAngle) {
                _turningSector = _remainingSector;
            }
            if(_turningSector * _remainingSector < 0) {
                _turningSector = _remainingSector;
            }

            //Calculate turning direction
            _turningDirection = Mathf.Sign(_remainingSector);

            //Calculate multiplier
            if(Mathf.Abs(_turningSector) < 90f) {
                _angularSpeedMultiplier = Mathf.Abs(_turningSector) / 90f;
            } else {
                _angularSpeedMultiplier = 1f;
            }
            
            if (Mathf.Abs(_turningSector) > 0f) {

                _remainingSectorFraction = _remainingSector / _turningSector;

                //Accelerate if more than N% is left
                if (_remainingSectorFraction > _slowingDownInterval) {

                    _approachingTargetAngle = false;

                    float oldVelocityDirection = Mathf.Sign(_localAngularVelocity.y);

                    _maxAngularVelocity = 
                        _maxAngularSpeed 
                        * _turningDirection
                        * _angularSpeedMultiplier;

                    if (_turningDirection < 0f) {
                        _localAngularVelocity.y -= 
                            _angularAcceleration * Time.deltaTime;
                        if (_localAngularVelocity.y < _maxAngularVelocity) {
                            _localAngularVelocity.y = _maxAngularVelocity;
                        }
                    } else if (_turningDirection > 0f) {
                        _localAngularVelocity.y += 
                            _angularAcceleration * Time.deltaTime;
                        if (_localAngularVelocity.y > _maxAngularVelocity) {
                            _localAngularVelocity.y = _maxAngularVelocity;
                        }
                    }

                    //Recalculate turning sector if we were decelerating
                    if (_localAngularVelocity.y != 0) {
                        if (Mathf.Sign(_localAngularVelocity.y) != _turningDirection) {
                            _turningSector = _remainingSector;
                        } else if (Mathf.Sign(_localAngularVelocity.y) != oldVelocityDirection) {
                            _turningSector = _remainingSector;
                        }
                    } else {
                        _turningSector = _remainingSector;
                    }
                } else {
                    //now we decelerate
                    if(_approachingTargetAngle == false) {
                        _approachingTargetAngle = true;
                        _slowingDownStartVelocity = _localAngularVelocity.y;
					}
                    _localAngularVelocity.y = 
                        _slowingDownStartVelocity
                        * _remainingSectorFraction
                        / _slowingDownInterval;
                }
            }
            
        } else {
            //Decelerate if no input
            if (_localAngularVelocity.y < 0f) {
                _localAngularVelocity.y += 
                    _angularDeceleration * Time.deltaTime;
                if (_localAngularVelocity.y > 0f) {
                    _localAngularVelocity.y = 0f;
                }
            } else if (_localAngularVelocity.y > 0f) {
                _localAngularVelocity.y -= 
                    _angularDeceleration * Time.deltaTime;
                if (_localAngularVelocity.y < 0f) {
                    _localAngularVelocity.y = 0f;
                }
            }
        }

        //drag
        _localAngularVelocity.x -= 
            _localAngularVelocity.x * _drag * Time.deltaTime;
        _localAngularVelocity.z -= 
            _localAngularVelocity.z * _drag * Time.deltaTime;

        //apply
        _rigidbody.angularVelocity = 
            _rigidbody.rotation * _localAngularVelocity;
    }

    public float GetAbsRemainingSector() {
        return Mathf.Abs(_remainingSector);
	}

    public void EnableMovement() {
        _moving = true;
    }

    public void DisableMovement() {
        _moving = false;
    }
}

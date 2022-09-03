using UnityEngine;

[DisallowMultipleComponent]

[RequireComponent(typeof(Turning))]
[RequireComponent(typeof(Walking))]
[RequireComponent(typeof(Rolling))]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(GroundCheck))]
[RequireComponent(typeof(Inventory))]

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]

public class Moving : MonoBehaviour {

    [Range(0.01f, 1.0f)]
    [SerializeField] private float _walkigRestrictionStrength = 1f;
    [Range(0.1f, 90.0f)]
    [SerializeField] private float _fallingAngle = 45f;
    [Range(0.1f, 90.0f)]
    [SerializeField] private float _slowdownAngle = 45f;

    private float _normalAngle;
    private float _speedMultiplier;
    private bool _rolling;
    private bool _canReflexCurl;

    private Walking _walkingComponent;
    private Turning _turningComponent;
    private Rolling _rollingComponent;
    private InputReader _input;
    private GroundCheck _groundCheckComponent;
    private Inventory _inventoryComponent;

    private Rigidbody _rigidbody;
    private Animator _animator;

    private Vector3 _centerOfMass;

    private void Start() {

        _walkingComponent = GetComponent<Walking>();
        _turningComponent = GetComponent<Turning>();
        _rollingComponent = GetComponent<Rolling>();
        _input = GetComponent<InputReader>();
        _groundCheckComponent = GetComponent<GroundCheck>();
        _inventoryComponent = GetComponent<Inventory>();

        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _animator = gameObject.GetComponent<Animator>();

        _walkingComponent.EnableWalking();
        _turningComponent.EnableMovement();

        _normalAngle = 0f;
        _speedMultiplier = 1f;
        _rolling = false;
        _canReflexCurl = true;
        
        _centerOfMass = GameObject.Find("Hedgehog/CenterOfMass").transform.localPosition;

        _rigidbody.centerOfMass = _centerOfMass;
    }

    private void Update() {

        //Angle of tilt
        _normalAngle = Quaternion.Angle(
            Quaternion.FromToRotation(
                _rigidbody.rotation * Vector3.up, 
                Vector3.up), 
            Quaternion.identity);

        if (_normalAngle < _fallingAngle 
            && _groundCheckComponent.IsGrounded() 
            && !_rolling) {

            //reset Reflex Curl
            _canReflexCurl = true;

            //We can move
            _walkingComponent.EnableWalking();
            _turningComponent.EnableMovement();

            //If we press Spacebar we walk backwards
            if (_input.GetReverse()) {
                _speedMultiplier = -0.5f;
                _input.Backwards();
            } else {
                _speedMultiplier = 1f;
                _input.Forward();
            }

            //Slow down if we turn too hard
            _speedMultiplier *= 1
                - _turningComponent.GetAbsRemainingSector() 
                * _walkigRestrictionStrength
                / 180f;

            //Slowing down if tilted
            if (_normalAngle > _slowdownAngle) {
                _speedMultiplier *=
                    1f - (_normalAngle - _slowdownAngle) / _fallingAngle;
            }

            //Apply slowing
            _walkingComponent.SetMultiplier(_speedMultiplier);

            //Set walk animation speed
            _animator.SetFloat("Walk Speed", _walkingComponent.GetWalkSpeed());
        } else {
            //We can't walk
            _input.Forward();
            _walkingComponent.DisableWalking();
            _turningComponent.DisableMovement();
            _animator.SetFloat("Walk Speed", 0f);
        }

        //If tilted enough ant can curl automatically
        if (_normalAngle >= _fallingAngle && _canReflexCurl) {
            CurlUp();
        }

        //Curl or uncurl using key
        if (_input.GetCurlKeyDown()) {
            if (_rolling) {
                _canReflexCurl = false;
                UnCurl();
            } else {
                CurlUp();
            }
        }
    }

	private void CurlUp() {
        _animator.SetBool("Curl", true);
        _rolling = true;
        _rollingComponent.EnableRolling();
        _inventoryComponent.DisableInventory();
        _rigidbody.ResetCenterOfMass();
    }

    private void UnCurl() {
        _animator.SetBool("Curl", false);
        _rolling = false;
        _rollingComponent.DisableRolling();
        _inventoryComponent.EnableInventory();
        _rigidbody.centerOfMass = _centerOfMass;
    }
}

using UnityEngine;

[DisallowMultipleComponent]

public class GroundCheck : MonoBehaviour {

    [Range(0.0f, 0.3f)]
    [SerializeField] private float _checkRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer;

    private Transform _frontLeftFoot;
    private Transform _frontRightFoot;
    private Transform _backLeftFoot;
    private Transform _backRightFoot;

    private bool _frontLeftFootGrounded;
    private bool _frontRightFootGrounded;
    private bool _backLeftFootGrounded;
    private bool _backRightFootGrounded;
    private bool _grounded;

    private void Start() {
        _frontLeftFoot = 
            GameObject.Find("Hedgehog/FrontLeftFoot").
            GetComponent<Transform>();
        _frontRightFoot = 
            GameObject.Find("Hedgehog/FrontRightFoot").
            GetComponent<Transform>();
        _backLeftFoot = 
            GameObject.Find("Hedgehog/BackLeftFoot").
            GetComponent<Transform>();
        _backRightFoot = 
            GameObject.Find("Hedgehog/BackRightFoot").
            GetComponent<Transform>();
    }

    private void FixedUpdate() {
        _frontLeftFootGrounded = 
            Physics.OverlapSphere(
                _frontLeftFoot.position, 
                _checkRadius, 
                _groundLayer.value).Length > 0;
        _frontRightFootGrounded = 
            Physics.OverlapSphere(
                _frontRightFoot.position, 
                _checkRadius, 
                _groundLayer.value).Length > 0;
        _backLeftFootGrounded = 
            Physics.OverlapSphere(
                _backLeftFoot.position, 
                _checkRadius, 
                _groundLayer.value).Length > 0;
        _backRightFootGrounded = 
            Physics.OverlapSphere(
                _backRightFoot.position, 
                _checkRadius, 
                _groundLayer.value).Length > 0;
        if(_frontLeftFootGrounded && _backRightFootGrounded
            || _frontRightFootGrounded && _backLeftFootGrounded) {
            _grounded = true;
		} else {
            _grounded = false;
		}
    }

    public bool IsGrounded() {
        return _grounded;
	}
}

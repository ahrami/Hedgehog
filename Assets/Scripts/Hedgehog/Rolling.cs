using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Rigidbody))]

public class Rolling : MonoBehaviour {

    [Range(0.0f, 100.0f)]
    [SerializeField] private float _force = 40f;

    private bool _moving;

    private Vector2 _inputVector;

    private InputReader _input;
    private Rigidbody _rigidbody;

    private void Start() {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
        _input = gameObject.GetComponent<InputReader>();
    }

    private void FixedUpdate() {
        if (_moving) {
            _inputVector = _input.GetInputVector();
            _rigidbody.AddForce(
                new Vector3(_inputVector.x, 0f, _inputVector.y) 
                * _force);
        }
    }

	public void EnableRolling() {
        _moving = true;
    }

    public void DisableRolling() {
        _moving = false;
    }
}

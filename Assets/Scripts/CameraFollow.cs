using UnityEngine;

[DisallowMultipleComponent]

public class CameraFollow : MonoBehaviour {

    [SerializeField] private GameObject _followObject;
    [SerializeField] private float _followTime;
    [Range(0f, 1f)]
    [SerializeField] private float _zoomStrength = 0.5f;

    private Transform _transform;
    private Transform _followTransform;
    private Transform _cameraTransform;

    private float _initialZoom;

    private void Start() {
        _followTime = 10 / _followTime;
        _transform = GetComponent<Transform>();
        _followTransform = _followObject.GetComponent<Transform>();
        _cameraTransform = _transform.GetChild(0);
        _initialZoom = _cameraTransform.localPosition.z;
    }

    private void FixedUpdate() {
        _transform.position = 
            Vector3.Lerp(
                _transform.position, 
                _followTransform.position,
                _followTime * Time.deltaTime);
        _cameraTransform.localPosition = 
            Vector3.Lerp(
                _cameraTransform.localPosition,
                new Vector3(0f, 0f,
                    (_transform.position - _followTransform.position)
                    .magnitude 
                    * _zoomStrength 
                    * _initialZoom 
                    + _initialZoom),
                _zoomStrength * _followTime * Time.deltaTime);
    }
}

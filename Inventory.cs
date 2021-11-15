using UnityEngine;

[DisallowMultipleComponent]

public class Inventory : MonoBehaviour {

    [Range(0.0f, 2.0f)]
    [SerializeField] private float _pickupRadius = 1f;
    [Range(0.0f, 2.0f)]
    [SerializeField] private float _pickupPause = 1f;
    [SerializeField] private int _inventorySize = 4;

    [SerializeField] private GameObject[] _places;
    [SerializeField] private LayerMask _pickableLayerMask;
    [SerializeField] private LayerMask _carryLayerMask;
    
    private int _pickableLayer;
    private int _carryLayer;

    private bool _inventoryEnabled;
    private float _deltaTime;
    private float _freeSlots;

    private Transform _transform;

    public struct Slot {
        public GameObject place;
        public bool free;
        public GameObject item;
	};

    private Slot[] _slots;

    private void Start() {
        Random.InitState(System.DateTime.Now.Millisecond);

        _transform = gameObject.GetComponent<Transform>();

        _deltaTime = 0f;
        _inventoryEnabled = true;

        if(_inventorySize > _places.Length) {
            _inventorySize = _places.Length;
        }

        _freeSlots = _inventorySize;

        _pickableLayer = 0;
        _carryLayer = 0;

        long carryLayerMask = _carryLayerMask;
        long pickableLayerMask = _pickableLayerMask;

        while (true) {
            if ((carryLayerMask = carryLayerMask >> 1) > 0) {
                ++_carryLayer;
            } else {
                break;
            }
        }
        while (true) {
            if ((pickableLayerMask = pickableLayerMask >> 1) > 0) {
                ++_pickableLayer;
            } else {
                break;
            }
        }

        _slots = new Slot[_places.Length];

        for (int i = 0; i < _slots.Length; ++i) {
            _slots[i].place = _places[i];
            _slots[i].free = true;
            _slots[i].item = null;
        }

    }

    private void PickUp(Collider collider) {
        while (true) {
            int k = Mathf.RoundToInt(
                Random.value * (_slots.Length - 1)
                );
            if (_slots[k].free) {
                _slots[k].free = false;
                _slots[k].item = collider.gameObject;
                collider.gameObject.transform.rotation = 
                    Random.rotation;
                collider.gameObject.transform.parent = 
                    _slots[k].place.transform;
                collider.gameObject.transform.position =
                    _slots[k].place.transform.position
                    - collider.gameObject.transform.rotation
                    * collider.gameObject.
                        GetComponent<Rigidbody>().centerOfMass;
                collider.gameObject.layer = 
                    _carryLayer;
                collider.gameObject.
                    GetComponent<Rigidbody>().isKinematic = true;
                --_freeSlots;
                _deltaTime = 0f;
                break;
            }
        }
    }

    private void FixedUpdate() {
        _deltaTime += Time.deltaTime;
    }

    public void EnableInventory() {
        _inventoryEnabled = true;
	}

    public void DisableInventory() {
        _inventoryEnabled = false;
        if (_freeSlots < _inventorySize) {
            for (int k = 0; k < _slots.Length; ++k) {
                if (!_slots[k].free) {
                    _slots[k].free = true;
                    _slots[k].item.gameObject.
                        transform.parent = null;
                    _slots[k].item.gameObject.
                        GetComponent<Rigidbody>().isKinematic = false;
                    _slots[k].item.gameObject.
                        layer = _pickableLayer;
                    _slots[k].item = null;
                    ++_freeSlots;
                }
            }
        }
    }

    public bool TryToPickUp() {
        if (_deltaTime > _pickupPause
            && _inventoryEnabled
            && _freeSlots > 0) {
            Collider[] collider = Physics.OverlapSphere(
                _transform.position,
                _pickupRadius,
                _pickableLayerMask.value);
            if (collider.Length > 0) {
                PickUp(collider[
                    Mathf.RoundToInt(
                        Random.value * (collider.Length - 1)
                        )]);
                return true;
            }
        }
        return false;
    }
}

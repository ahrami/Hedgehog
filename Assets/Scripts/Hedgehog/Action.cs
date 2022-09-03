using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(InputReader))]
[RequireComponent(typeof(Inventory))]

public class Action : MonoBehaviour {

    private InputReader _inputReader;
    private Inventory _inventory;

    private void Start() {
        _inputReader = gameObject.GetComponent<InputReader>();
        _inventory = gameObject.GetComponent<Inventory>();
    }

    private void FixedUpdate() {
		if (_inputReader.GetActionKey()) {
			if (_inventory.TryToPickUp()) {

			} else {
                //do some other action
			}
		}
    }
}

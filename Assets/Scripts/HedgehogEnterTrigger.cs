using UnityEngine;

[DisallowMultipleComponent]

public class HedgehogEnterTrigger : Trigger {

	[SerializeField] LayerMask _playerLayerMask;

	private void OnTriggerEnter(Collider collision) {
		if (Mathf.Pow(2, collision.gameObject.layer) == _playerLayerMask.value) {
			Activate();
		}
	}
}

using UnityEngine;

[DisallowMultipleComponent]

public class ShroomCounter : MonoBehaviour {

	[SerializeField] private int _shroomCount;
	[SerializeField] private int _goldenFungusCount;

	[SerializeField] private string _pickupableLayer;
	[SerializeField] private string _shroomTag;
	[SerializeField] private string _goldenFungusTag;

	private void OnTriggerEnter(Collider other) {
		if (other.gameObject.layer == 
			LayerMask.NameToLayer(_pickupableLayer)) {
			if (other.gameObject.tag == _shroomTag) {
				other.gameObject.GetComponent<Mushroom>().DeleteAndBoom();
				++_shroomCount;
			} else if (other.gameObject.tag == _goldenFungusTag) {
				other.gameObject.GetComponent<Mushroom>().DeleteAndBoom();
				++_goldenFungusCount;
			}
		}
	}
}

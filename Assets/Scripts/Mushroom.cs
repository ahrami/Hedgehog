using UnityEngine;

[DisallowMultipleComponent]

public class Mushroom : MonoBehaviour {

    private void Start() {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            if (gameObject.transform.childCount > 1) {
                GameObject particles = 
                    gameObject.transform.GetChild(1).gameObject;
                particles.transform.parent = null;
                particles.GetComponent<ParticleSystem>().Play();
            }
            }
    }

	public void DeleteAndBoom() {
        if (gameObject.transform.childCount > 0) {
            GameObject particles = 
                gameObject.transform.GetChild(0).gameObject;
            particles.transform.parent = null;
            particles.GetComponent<ParticleSystem>().Play();
        }
        Destroy(gameObject);
    }
}

using UnityEngine;
using System.Collections;

public class BombController : MonoBehaviour {

    public ParticleSystem explosion;
    public float splashRadius = 150f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnCollisionEnter(Collision collision)
    {
        //move and play explosion effect
        explosion.transform.position = transform.position;
        explosion.Play();
        ExplosionDamage(transform.position, splashRadius);
        gameObject.SetActive(false);
    }

    //modified from unity doc example
    void ExplosionDamage(Vector3 center, float radius) {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius);
        Debug.Log(hitColliders.Length);
        int i = 0;
        while (i < hitColliders.Length) {
            hitColliders[i].SendMessage("ExplosionPush", transform.position, SendMessageOptions.DontRequireReceiver);
            Debug.Log(hitColliders[i].tag);
            i++;
        }
    }
}

using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnEnable()
    {
        BombController.Boom += ExplosionPush;
    }


    void OnDisable()
    {
        BombController.Boom -= ExplosionPush;
    }
    
    void ExplosionPush(Vector3 bombPos)
    {
        float distance = Vector3.Distance(bombPos, transform.position);
        rigidbody.AddForce(Vector3.up * (150f - distance), ForceMode.Impulse);
        rigidbody.AddExplosionForce(Mathf.Pow(150f - distance, 2)/10f, bombPos, 300f, 3f); 
        
    }
}

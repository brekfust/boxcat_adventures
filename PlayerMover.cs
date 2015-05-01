using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {


	public float speed = 6f;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
	int floorMask; //why did I add this? figure it out.

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		playerRigidbody = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		float h = Input.GetAxisRaw ("Horizontal");
		float v = Input.GetAxisRaw ("Vertical");
        float pounce = Input.GetAxisRaw("Fire1");
        float whip = Input.GetAxisRaw("Jump");
		
		Move (h, v);
        Turning(h, v);
		animating (h, v, pounce, whip);

	}

	void Move (float h, float v)
	{
		movement.Set (h, 0f, v);
		movement = movement.normalized * speed * Time.deltaTime;
		playerRigidbody.MovePosition(playerRigidbody.position + movement);
	}

    void Turning(float h, float v)
    {
        if (h == 0 && v == 0)
            return;
        
        Vector3 lookSpot;
        lookSpot = new Vector3(h * 10, 0, v * 10); //take movement direction, add 10 to give distance from player

        Quaternion lookDirection = new Quaternion();
        //lookDirection = Quaternion.LookRotation(playerRigidbody.position + lookSpot);
        lookDirection.SetLookRotation(lookSpot);
        lookDirection *= Quaternion.Euler(0f, 90f, 0f); //compensates for original rotation. Mesh was 90 degrees off.
        playerRigidbody.MoveRotation(lookDirection);
    }


	void animating(float h, float v, float pounce, float whip)
	{
		bool walking = h!= 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);
        bool pouncing = pounce != 0f;
        anim.SetBool("IsPouncing", pouncing);
        bool whipping = whip != 0f;
        anim.SetBool("IsWhipping", whipping);
	}

    void ExplosionPush(Vector3 bombPos)
    {
        float distance = Vector3.Distance(bombPos, transform.position);
        Quaternion rotate = Quaternion.Inverse(Quaternion.LookRotation(bombPos));
        playerRigidbody.AddTorque(rotate.eulerAngles);
        playerRigidbody.AddForce(rotate.eulerAngles * distance * 100000);
    }
}

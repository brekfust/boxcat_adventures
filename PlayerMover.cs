using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerMover : MonoBehaviour {


	public float speed = 6f;
    public float gravity = 5f;
    public float pounceSpeed = 30f;
    public Text text1;
    public Text text2;

	Vector3 movement;
	Animator anim;
	Rigidbody playerRigidbody;
    //bool isPouncing = false;
    bool canWalk;
    bool isAttacking = false;

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

        //determine if we're allowed to move
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") || isAttacking == true) //let intro animation play before allowing movement
        {
            canWalk = false;
        }
        else
        {
            canWalk = true; 
        }

        if (canWalk == true)
        {
            Move(h, v);
            Turning(h, v);
            animating(h, v, pounce, whip);
            //Gravity(gravity);
            Attacking(pounce, whip);
        }

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

    //void Gravity(float gravity)
    //{ //not working, and not needed with rigidbody gravity
    //    Vector3 smoothedGravity = new Vector3(0, -(gravity * Time.deltaTime), 0);
    //    playerRigidbody.MovePosition(smoothedGravity);
    //}

	void animating(float h, float v, float pounce, float whip)
	{
		bool walking = h!= 0f || v != 0f;
		anim.SetBool ("IsWalking", walking);
        if (isAttacking == false)
        {
            if (pounce != 0f)
            {
                anim.SetTrigger("IsPouncing");
            }
            if (whip != 0f)
                anim.SetTrigger("IsWhipping");
        }
	}

    void Attacking(float pounce, float whip)
    {
        if (pounce != 0 && isAttacking == false)
        {
            isAttacking = true;
            //anim.SetTrigger("IsPouncing");
            StartCoroutine("Pounce");
        }
    }

    IEnumerator Pounce()
    {

        Vector3 actualForward;
        actualForward = -Vector3.Cross(playerRigidbody.transform.up, playerRigidbody.transform.forward); //hack to fix models incorrect rotation
        bool isAnimStarted = false;
        while(isAttacking == true)
        {
            AnimatorStateInfo pounceAnim;
            pounceAnim = anim.GetCurrentAnimatorStateInfo(0);
            canWalk = false;

            Vector3 pounceMovement = actualForward * pounceSpeed * Time.deltaTime;
            if (pounceAnim.normalizedTime < .5f)
                pounceMovement = pounceMovement + Vector3.up * pounceSpeed * Time.deltaTime;
            playerRigidbody.MovePosition(playerRigidbody.position + pounceMovement);

            //Can't check for pounce animation to finish right away, so check name and isAnimStarted bool
            if (pounceAnim.IsName("Pounce"))
                isAnimStarted = true;
            if (isAnimStarted && !pounceAnim.IsName("Pounce"))
                isAttacking = false;
            yield return null;
        }
    }

    void ExplosionPush(Vector3 bombPos)
    {
        //old bad code. Fix this.
        float distance = Vector3.Distance(bombPos, transform.position);
        Quaternion rotate = Quaternion.Inverse(Quaternion.LookRotation(bombPos));
        playerRigidbody.AddTorque(rotate.eulerAngles);
        playerRigidbody.AddForce(rotate.eulerAngles * distance * 100000);
    }
}

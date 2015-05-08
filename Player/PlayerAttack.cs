using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

    public PlayerMover.AttackType attackType;
    public float damage;

    bool inAttackRange = false;
    bool alreadyAttacked = false;
    GameObject enemy;
    bool attackTypeState;

    //try this, create struct, create struct array that will hold status info of each collision. coroutine should foreach the array to
    //decide whether to stop looping or not.

	// Use this for initialization
	void Start () {
        //check if this works, trying to create reference to PlayerMover.isPouncing
        //does not work. Do the struct or something else.
        if (attackType == PlayerMover.AttackType.Pounce)
        {
            attackTypeState = PlayerMover.isPouncing;
        }
	}
	
	// Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision from" + attackType.ToString() + " to " + other.gameObject.tag);
        if (other.gameObject.CompareTag("Enemy"))
        {
            inAttackRange = true;
            enemy = other.gameObject;
            StartCoroutine("ReadyToSendAttack", enemy);
            Debug.Log("In Pounce Range");
        }
    }

    void onCollisionExit(Collision other)
    {
        if (other.collider == enemy) //this will not scale. update struct  array described on top instead.
            inAttackRange = false;

    }

    IEnumerator ReadyToSendAttack(GameObject _enemy)
    {
        while(inAttackRange == true) //also will not scale
        {
            if (!alreadyAttacked && PlayerMover.isPouncing == true && attackType == PlayerMover.AttackType.Pounce) //take pounce damage
            {
                enemy.SendMessage("TakeDamage", damage);
                alreadyAttacked = true;
            }
            if (PlayerMover.isPouncing == false)
                alreadyAttacked = false;
            yield return null;
        }
    }
}

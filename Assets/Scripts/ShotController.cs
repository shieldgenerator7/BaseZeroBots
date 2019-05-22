using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 1;
    public Instruction replaceInstruction;

    public BotController owner;

    private float lifeStartTime;

    private void Start()
    {
        lifeStartTime = Time.time;
    }

    private void Update()
    {
        if (Time.time > lifeStartTime + lifetime)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        BotController bc = coll.gameObject.GetComponent<BotController>();
        if (bc && bc != owner)
        {
            //Delete target instructions
            bc.damage(damage);
            //Destroy shot
            Destroy(gameObject);
        }
    }
}

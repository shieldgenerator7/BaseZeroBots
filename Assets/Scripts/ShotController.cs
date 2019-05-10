using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotController : MonoBehaviour
{
    public int damage = 1;
    public float lifetime = 1;
    public Instruction replaceInstruction;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BotController bc = collision.gameObject.GetComponent<BotController>();
        if (bc)
        {
            //Delete target instructions
            for (int i = 0; i < damage; i++)
            {
                int randomIndex = Random.Range(0, bc.instructions.Count);
                bc.destroyInstruction(randomIndex, replaceInstruction);
            }
            //Destroy shot
            Destroy(gameObject);
        }
    }
}

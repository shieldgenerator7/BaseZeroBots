using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "Instruction/Shoot", order = 1)]
public class Shoot : Instruction
{

    public GameObject projectilePrefab;

    public override void doAction(ProcessContext context)
    {
        BotController bc = context.botController;
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.up = bc.transform.up;
        projectile.transform.localPosition = bc.transform.localPosition + bc.transform.up;
        projectile.GetComponent<Rigidbody2D>().velocity = projectile.transform.up;
        projectile.GetComponent<ShotController>().owner = bc;
    }
}

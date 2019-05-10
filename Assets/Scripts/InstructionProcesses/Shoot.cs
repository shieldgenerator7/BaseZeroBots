using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shoot", menuName = "Instruction/Shoot", order = 1)]
public class Shoot : Instruction
{

    public GameObject projectilePrefab;

    public override void doAction(BotController bc, int currentIndex, List<Instruction> instructions)
    {
        GameObject projectile = Instantiate(projectilePrefab);
        projectile.transform.up = bc.transform.up;
        projectile.transform.localPosition = bc.transform.localPosition + Vector3.up;
        projectile.GetComponent<Rigidbody2D>().velocity = Vector2.up;
    }
}

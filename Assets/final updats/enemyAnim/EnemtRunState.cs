using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemtRunState : EnemyBaseState
{
    public override void EnterState(player player)
    {
        player.smallEnimySpawning.anim.SetBool("isRunning", true);
        player.smallEnimySpawning.anim.SetBool("isAttacking", false);
    }

    public override void UpdateState(player player)
    {
        float distanceToPlayer = Vector3.Distance(player.transform.position, player.smallEnimySpawning.transform.position);

        if (distanceToPlayer <= player.smallEnimySpawning.attackRange)
        {
            player.SwitchState(player.eAttack);
        }
    }
}

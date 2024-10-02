using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : MoveBaseState
{

    public override void EnterState(movementStateManager movement)
    {
        movement.anim.SetBool("Running", true);
        movement.moveSpead = 8f;
    }

    public override void UpdateState(movementStateManager movement)
    {
        if(Input.GetKeyUp(KeyCode.LeftShift)) ExitState(movement,movement.Walk);
        else if(movement.dir.magnitude < 0.1f) ExitState(movement,movement.Idle);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            ExitState(movement,movement.jump);
        }
    }
    void ExitState(movementStateManager movement,MoveBaseState state){
        movement.anim.SetBool("Running",false);
        movement.SwitchState(state);
        movement.moveSpead = 3f;
    }
}

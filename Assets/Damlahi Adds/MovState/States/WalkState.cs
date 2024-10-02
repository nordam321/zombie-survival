using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MoveBaseState
{
    public override void EnterState(movementStateManager movement)
    {
        movement.anim.SetBool("Walking", true);
    }

    public override void UpdateState(movementStateManager movement)
    {
        if(Input.GetKey(KeyCode.LeftShift)) ExitState(movement,movement.Run);
        else if(Input.GetKeyDown(KeyCode.C)) ExitState(movement,movement.crouch);
        else if(movement.dir.magnitude < 0.1f) ExitState(movement,movement.Idle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            movement.previousState = this;
            ExitState(movement,movement.jump);
        }
    }
    void ExitState(movementStateManager movement,MoveBaseState state){
        movement.anim.SetBool("Walking",false);
        movement.SwitchState(state);

    }
}

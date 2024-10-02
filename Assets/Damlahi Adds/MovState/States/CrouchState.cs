using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrouchState : MoveBaseState
{
    public override void EnterState(movementStateManager movement)
    {
        movement.anim.SetBool("Crouching", true);
    }

    public override void UpdateState(movementStateManager movement)
    {
        if(Input.GetKey(KeyCode.LeftShift)) ExitState(movement,movement.Run);
        if(Input.GetKeyUp(KeyCode.C))
        {
            
        ExitState(movement,movement.Walk);
        }
    }

    void ExitState(movementStateManager movement,MoveBaseState state){
        movement.anim.SetBool("Crouching", false);
        movement.SwitchState(state);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swinging : StateMachineBehaviour
{
    private WeaponBehaviour wb;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wb = animator.GetComponent<WeaponBehaviour>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        wb.isSwinging = false;
        animator.SetBool("IsSwinging", false);
    }

}

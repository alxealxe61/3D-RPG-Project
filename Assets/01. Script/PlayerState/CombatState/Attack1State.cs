using UnityEngine;

namespace _01._Script
{
    public class Attack1State : CombatAttackState
    {
        public Attack1State
            (PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            Debug.Log("Attack1");
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            if (Input.GetKeyDown(KeyCode.Space) && GetNormalizedTime() >= 0.6f && comboPossible == false)
            {
                comboPossible = true;
            }
            
            if (GetNormalizedTime() >= 0.9f)
            {
                if(comboPossible)
                    stateMachine.ChangeState(player.attack2State);
                else
                    stateMachine.ChangeState(player.combatIdleState);
            }
        }
        
        public override void Exit()
        {
            base.Exit();
            //player.ani.ResetTrigger(triggerName);
        }
    }
}
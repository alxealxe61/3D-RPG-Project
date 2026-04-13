
using UnityEngine;

namespace _01._Script
{
    public class CombatAttackState : PlayerState
    {
        public int combat;
        
        private const float InputEnter = 0f;
        private const float InputExit = 1f; 
        public CombatAttackState(PlayerController player, PlayerStateMachine stateMachine, string animName) 
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            Debug.Log("Attack");
            combat = 1;
        }
        
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            AnimatorStateInfo stateInfo = player.ani.GetCurrentAnimatorStateInfo(0);

            float progress = stateInfo.normalizedTime % 1.0f;

            if (progress >= InputEnter && InputExit >= progress)
            {
                if (Input.GetMouseButtonDown(0) && combat == 1)
                {
                    combat++;
                    player.ani.SetTrigger("CombatAttack");
                }
                else if (Input.GetMouseButtonDown(0) && combat == 2)
                {
                    player.ani.SetTrigger("CombatAttack");
                }
                else
                {
                    stateMachine.ChangeState(player.combatIdleState);
                }
            }
            
        }

        public override void Exit()
        {
            base.Exit();
            combat = 0;
        }
    }
}
using UnityEngine;

namespace _01._Script
{
    public class Attack3State : CombatAttackState
    {
        public Attack3State
            (PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }
        
        public override void Enter()
        {
            base.Enter();
            // 마지막 타수이므로 콤보 가능 여부를 미리 꺼둠
            Debug.Log("Attack3");
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            
            if (GetNormalizedTime() >= 0.9f)
            {
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
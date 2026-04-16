
using UnityEngine;

namespace _01._Script
{
    public abstract class CombatAttackState : PlayerState
    {
        //protected float lastInputTime;
        protected bool comboPossible;
        public CombatAttackState(PlayerController player, PlayerStateMachine stateMachine, string animName)
            : base(player, stateMachine, animName) { }

        public override void Enter()
        {
            base.Enter();
            comboPossible = false;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();

            // 선입력 버퍼링: 언제든 클릭하면 다음 콤보 예약
            
            if (Input.GetMouseButtonDown(1))
            {
                stateMachine.BoolChangeState(player.combatGuardState);
            }
        }

        public override void Exit()
        {
            base.Exit();
            // 공격 상태 종료 시 판정 강제 종료
            player.hitBox.AttackStop();
        }
    }
}
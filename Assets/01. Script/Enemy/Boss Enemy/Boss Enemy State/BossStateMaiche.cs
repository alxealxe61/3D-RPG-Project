using _01._Script.StataPattern;
using UnityEngine;

namespace _01._Script.Enemy.Boss_Enemy.Boss_Enemy_State
{
    public class BossStateMachine : StateMachine<BossController>
    {
        public override void ChangeState(State<BossController> newState)
        {
            base.ChangeState(newState);
            
            Debug.Log($"보스 몬스터 상태 변경: {newState.GetType().Name}");
        }
    }
}
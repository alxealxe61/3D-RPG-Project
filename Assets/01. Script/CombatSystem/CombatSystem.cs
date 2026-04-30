using System.Collections.Generic;
using _01._Script.Data;
using UnityEngine;

namespace _01._Script.CombatSystem
{
    public class CombatSystem : SingletonBase<CombatSystem>
    {
        private const int EventProcessPerFrame = 10;
        
        private Dictionary<Collider, HurtBox> _hurtBoxDic;
        private Queue<CombatEvent> CombatEventQueue { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            _hurtBoxDic = new Dictionary<Collider, HurtBox>();
            CombatEventQueue = new Queue<CombatEvent>();
        }

        private void Update()
        {
            for (int i = 0; i < EventProcessPerFrame; i++)
            {
                if (CombatEventQueue.Count == 0) break;
                var combatEvent = CombatEventQueue.Dequeue();
                HandleCombatEvent(combatEvent);
            }
        }
        public void AddCombatEvent(CombatEvent combatEvent)
        {
            CombatEventQueue.Enqueue(combatEvent);
        }

        private void HandleCombatEvent(CombatEvent combatEvent)
        {
            if (combatEvent.Sender == combatEvent.Receiver) return;

            combatEvent.Receiver.TakeDamage(combatEvent.Damage);
            
            if (combatEvent.HitInfo.Stun) combatEvent.Receiver.Stun();
            if (combatEvent.HitInfo.Pull) combatEvent.Receiver.Pull();
            if (combatEvent.Sender is PlayerStats player) player.AddSkillPoint(1);
        }
        
        public void AddHurtBox(Collider col, HurtBox hurtBox)
        {
            _hurtBoxDic.TryAdd(col, hurtBox);
        }
        
        public bool HasHurtBox(Collider col)
        {
            return _hurtBoxDic.ContainsKey(col);
        }

        public HurtBox GetHurtBox(Collider col)
        {
            return _hurtBoxDic[col];
        }
    }
}
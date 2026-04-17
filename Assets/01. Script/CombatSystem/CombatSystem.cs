using System.Collections.Generic;
using UnityEngine;

namespace _01._Script.CombatSystem
{
    public class CombatSystem : SingletonBase<CombatSystem>
    {
        private const int EVENT_PROCESS_PER_FRAME = 10;
        
        private Dictionary<Collider, HurtBox> HurtBoxDic;
        private Queue<CombatEvent> CombatEventQueue { get; set; }
        
        protected override void Awake()
        {
            base.Awake();
            HurtBoxDic = new Dictionary<Collider, HurtBox>();
            CombatEventQueue = new Queue<CombatEvent>();
        }

        private void Update()
        {
            for (int i = 0; i < EVENT_PROCESS_PER_FRAME; i++)
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

            if (combatEvent.Sender is PlayerStats player)
            {
                player.AddSkillPoint(0.5f);
            }

            Debug.Log($"Receiver : {combatEvent.Receiver},Damage : {combatEvent.Damage}");
        }
    

        public void AddHurtBox(Collider col, HurtBox hurtBox)
        {
            HurtBoxDic.TryAdd(col, hurtBox);
        }

        public void RemoveHurtBox(Collider col, HurtBox hurtBox)
        {
            if (HurtBoxDic.ContainsKey(col) ==  false) return;
            HurtBoxDic.Remove(col);
        }

        public bool HasHurtBox(Collider col)
        {
            return HurtBoxDic.ContainsKey(col);
        }

        public HurtBox GetHurtBox(Collider col)
        {
            return HurtBoxDic[col];
        }
    }
}
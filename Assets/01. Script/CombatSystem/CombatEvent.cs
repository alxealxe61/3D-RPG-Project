namespace _01._Script.CombatSystem
{
    public struct CombatEvent
    {
        public ICombatAgent Sender;
        public ICombatAgent Receiver;
        
        public int Damage;
        public int SkillPoitnt;
        public HitInfo HitInfo;
    }
}
using System.Collections.Generic;
using _01._Script.Enemy_Data;
using _01._Script.Enemy;
using _01._Script.Enemy.EnemyState.Melee_EnemyState;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.AttackPatten1;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.Pattern2;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.PeaceState;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MeleeEnemyController : MonoBehaviour
{
    public MeleeEnemyStats meleeEnemyStats;
        
    public DetectionRange detectionRange => _detectionRange;
    public AttackRange attackRange => _attackRange;
    
    [SerializeField] private DetectionRange _detectionRange;
    [SerializeField] private AttackRange _attackRange;
    
    public float moveSpeed => meleeEnemyStats.moveSpeed;

    [SerializeField] public Animator ani;
    public NavMeshAgent Agent;
    
    public LHitBox lHitBox;
    
    public RHitBox rHitBox;

    public bool isAttacking = false;
    
    public List<MeleeEnemyState> patterns = new List<MeleeEnemyState>();
    
    public Transform Target => _detectionRange.detectedTarget;
    
    private MeleeEnemyStateMachine StateMachine { get; set; }
    
    public EnemyCombatIdleState CombatIdleState { get; private set; }
    public EnemyCombatMovestate CombatMovestate { get; private set; }
    public EnemyCombatStunState CombatStunState { get; private set; }
    public Pattern1Attack1 Pattern1Attack1 { get; private set; }
    public Pattern1Attack2 Pattern1Attack2 { get; private set; }
    public Pattern2Attack1 Pattern2Attack1 { get; private set; }
    public Pattern2Attack2 Pattern2Attack2 { get; private set; }
    public Pattern2Attack3 Pattern2Attack3 { get; private set; }
    
    void Awake()
    {
        StateMachine = new MeleeEnemyStateMachine();
        
        CombatIdleState = new EnemyCombatIdleState(this, StateMachine, "CombatIdle", false);
        CombatMovestate = new EnemyCombatMovestate(this, StateMachine, "CombatMove", true);
        CombatStunState = new EnemyCombatStunState(this, StateMachine, "CombatStun", false);
        // 패턴 1 공격 2개 상태 
        Pattern1Attack1 = new Pattern1Attack1(this, StateMachine, "Pattern1Attack1", false);
        Pattern1Attack2 = new Pattern1Attack2(this, StateMachine, "Pattern1Attack2", false);
        // 패턴 2 공격 3개 상태
        Pattern2Attack1 = new Pattern2Attack1(this, StateMachine, "Pattern2Attack1", false);
        Pattern2Attack2 = new Pattern2Attack2(this, StateMachine, "Pattern2Attack2", false);
        Pattern2Attack3 = new Pattern2Attack3(this, StateMachine, "Pattern2Attack3", false);
    }

    void Start()
    {
        StateMachine.Initialize(CombatIdleState);
        patterns.Add(Pattern1Attack1);
        patterns.Add(Pattern2Attack1);
    }

    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    public void isStunned()
    {
        StateMachine.ChangeState(CombatStunState);
    }

    #region 애니메이션 이벤트 호출 함수들

    public void LHit()
    {
        lHitBox.EnableDetection();
    }

    public void RHit()
    {
        rHitBox.EnableDetection();
    }
    
    #endregion

}
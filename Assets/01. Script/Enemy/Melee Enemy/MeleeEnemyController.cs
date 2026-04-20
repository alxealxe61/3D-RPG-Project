using _01._Script.Enemy_Data;
using _01._Script.Enemy;
using _01._Script.Enemy.EnemyState.Melee_EnemyState;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.CombatState.AttackPatten1;
using _01._Script.Enemy.EnemyState.Melee_EnemyState.PeaceState;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyController : MonoBehaviour
{
    public MeleeEnemyStats meleeEnemyStats;
        
    public float moveSpeed => meleeEnemyStats.moveSpeed;

    [SerializeField] public Animator ani;
    public NavMeshAgent Agent;
    
    public HitBox hitBox;

    public Transform target;

    public EnemyPerception Perception;
    private MeleeEnemyStateMachine StateMachine { get; set; }
    
    public EnemyCombatIdleState combatIdleState { get; private set; }
    public EnemyCombatMovestate combatMovestate { get; private set; }
    public EnemyCombatStunState combatStunState { get; private set; }
    public EnemyEnterCombatState enterCombatState { get; private set; }
    public EnemyExitCombatState exitCombatState { get; private set; }
    public EnemyPeaceIdleState peaceIdleState { get; private set; }
    public EnemyPeaceMoveState peaceMoveState { get; private set; }
    public Patten1Attack1 patten1Attack1 { get; private set; }
    public Patten1Attack2 patten1Attack2 { get; private set; }
    
    void Awake()
    {
        StateMachine = new MeleeEnemyStateMachine();
        
        combatIdleState = new EnemyCombatIdleState(this, StateMachine, "CombatIdle", false);
        combatMovestate = new EnemyCombatMovestate(this, StateMachine, "CombatMove", true);
        combatStunState = new EnemyCombatStunState(this, StateMachine, "", false);
        // 패턴 1 공격 2개 상태 
        patten1Attack1 = new Patten1Attack1(this, StateMachine, "Pattern1Attack1", false);
        patten1Attack2 = new Patten1Attack2(this, StateMachine, "Pattern1Attack2", false);
        
        // 아래 필요 없을 거 같긴한다 혹시 모르니 남김 
        enterCombatState = new EnemyEnterCombatState(this, StateMachine, "", false);
        exitCombatState = new EnemyExitCombatState(this, StateMachine, "", false);
        peaceIdleState = new EnemyPeaceIdleState(this, StateMachine, "", false);
        peaceMoveState = new EnemyPeaceMoveState(this, StateMachine, "", false);
    }

    void Start()
    {
        StateMachine.Initialize(combatIdleState);
    }

    void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }

    private void SetTarget(Transform Newtarget)
    {
        target = Newtarget;
        if (target == null)
        {
            StateMachine.ChangeState(combatMovestate);
        }
    }

    private void OnEnable()
    {
        if (Perception != null)
        {
            Perception.OnTargetDetected += HandleTargetDetected;
            Perception.OnTargetLost += HandleTargetLost;
        }
    }
    
    private void HandleTargetDetected(Transform target)
    {
        SetTarget(target);
        StateMachine.ChangeState(combatMovestate);
    }

    private void HandleTargetLost()
    {
        SetTarget(null);
    }

    private void OnDisable()
    { 
        if (Perception != null)
        {
            Perception.OnTargetDetected -= HandleTargetDetected;
            Perception.OnTargetLost -= HandleTargetLost;
        }
    }

    #region 애니메이션 이벤트 호출 함수들

    public void Hit()
    {
        // 히트 박스 켜지는 함수 호출
    }

    #endregion

}
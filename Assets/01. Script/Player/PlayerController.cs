using UnityEngine;
using _01._Script;

public class PlayerController : MonoBehaviour
{
    private float horizontalSensitivity = 1.0f;
    
    public PlayerStats playerStats;
    public LockOnSystem lockOnSystem;
    public float moveSpeed =>  playerStats.MoveSpeed;
    public Vector2 InputVector { get; private set; }
    public Animator ani;
    public GameObject handSword;
    public GameObject etcSword;
    [Range(0, 1)] [SerializeField] public float daming = 0.0f;
    public bool isWeaponInHand = false;
    public HitBox hitBox;
    public SkillHitBox skillHitBox;
    
    public float GuardTimer { get; set; }
    public bool IsGuarding => StateMachine.CurrentState == combatGuardState;
    
    private PlayerStateMachine StateMachine { get; set; }
    public PeaceIdleState peaceIdleState { get; private set; }
    public PeaceMoveState peaceMoveState { get; private set; }
    public CombatIdleState combatIdleState { get; private set; }
    public CombatMoveState combatMoveState { get; private set; }
    public Attack1State attack1State { get; private set; }
    public Attack2State attack2State { get; private set; }
    public Attack3State attack3State { get; private set; }
    public CombatGuardState combatGuardState { get; private set; }
    public  CombatDodgeState combatDodgeState { get; private set; }
    public CombatSkillState combatSkillState { get; private set; }
    public EnterCombatState enterCombatState { get; private set; }
    public ExitCombatState  exitCombatState { get; private set; }
    public CombatStunState combatStunState { get; private set; }

    void Awake()
    {
        StateMachine = new PlayerStateMachine();
        
        peaceIdleState = new PeaceIdleState(this, StateMachine, "PeaceIdle", false);
        peaceMoveState = new PeaceMoveState(this, StateMachine, "PeaceMove", false);
        combatIdleState = new CombatIdleState(this, StateMachine, "CombatIdle", false);
        combatMoveState = new CombatMoveState(this, StateMachine, "CombatMove", false);
        attack1State = new Attack1State(this, StateMachine, "Attack1", false);
        attack2State = new Attack2State(this, StateMachine, "Attack2", false);
        attack3State = new Attack3State(this, StateMachine, "Attack3", false);
        combatGuardState = new CombatGuardState(this, StateMachine, "CombatGuard", true);
        combatDodgeState = new CombatDodgeState(this, StateMachine, "CombatDodge", false);
        combatSkillState = new CombatSkillState(this, StateMachine, "CombatSkill", false);
        enterCombatState = new EnterCombatState(this, StateMachine, "EnterCombat", false);
        exitCombatState = new ExitCombatState(this, StateMachine, "ExitCombat", false);
        combatStunState = new CombatStunState(this, StateMachine, "CombatStun", false);
    }
    
    void Start()
    {
        StateMachine.Initialize(peaceIdleState);
    }
    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        InputVector = new Vector2(x, y).normalized;
        StateMachine.CurrentState.LogicUpdate();
    }

    void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
    }
    
    public void isStunned()
    {
        StateMachine.ChangeState(combatStunState);
    }
    
    public bool AttemptSkillUse()
    {
        if (playerStats.CanUseSkill())
        {
            playerStats.UseSkill();
            
            StateMachine.ChangeState(combatSkillState);
            return true;
        }
        Debug.Log("[Skill] 포인트가 부족합니다! (현재 포인트 필요: 8)");
        return false;
    }
    
    public void UpdateGuardTimer()
    {
        GuardTimer += Time.deltaTime;
    }
    
    #region 애니메이션 이벤트 함수 모음

    public void WeaponSwitch() 
    {
        //Debug.Log("검뽑");
        if (isWeaponInHand)
        {
            // 뽑는 동작 중이라면: 등 끄고 손 켜기
            etcSword.SetActive(false);
            handSword.SetActive(true);
        }
        else
        {
            // 넣는 동작 중이라면: 등 켜고 손 끄기
            etcSword.SetActive(true);
            handSword.SetActive(false);
        }
    }

    public void Hit()
    {
        hitBox.EnableDetection();
    }

    public void SkillHit()
    {
        skillHitBox.EnableDetection();
    }

    #endregion
}
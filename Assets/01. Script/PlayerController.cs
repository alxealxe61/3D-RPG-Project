using UnityEngine;
using _01._Script;

public class PlayerController : MonoBehaviour
{
    private float horizontalSensitivity = 1.0f;
    
    public PlayerStats playerStats;
    
    public int moveSpeed =>  playerStats.moveSpeed;
    public Vector2 InputVector { get; private set; }

    public Collider[] col;
    public Animator ani;
    
    public GameObject handSword;
    public GameObject etcSword;
    
    public bool isWeaponInHand = false;
    
    public PlayerStateMachine StateMachine { get; private set; }
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

    void Awake()
    {
        StateMachine = new PlayerStateMachine();
        
        peaceIdleState = new PeaceIdleState(this, StateMachine, "PeaceIdle");
        peaceMoveState = new PeaceMoveState(this, StateMachine, "PeaceMove");
        combatIdleState = new CombatIdleState(this, StateMachine, "CombatIdle");
        combatMoveState = new CombatMoveState(this, StateMachine, "CombatMove");
        attack1State = new Attack1State(this, StateMachine, "Attack1");
        attack2State = new Attack2State(this, StateMachine, "Attack2");
        attack3State = new Attack3State(this, StateMachine, "Attack3");
        combatGuardState = new CombatGuardState(this, StateMachine, "CombatGuard");
        combatDodgeState = new CombatDodgeState(this, StateMachine, "CombatDodge");
        combatSkillState = new CombatSkillState(this, StateMachine, "CombatSkill");
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
    
    public void WeaponSwitch() 
    {
        Debug.Log("검뽑");
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
}
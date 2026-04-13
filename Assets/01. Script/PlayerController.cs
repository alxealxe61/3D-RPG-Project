using UnityEngine;
using _01._Script;

public class PlayerController : MonoBehaviour
{
    private float horizontalSensitivity = 1.0f;

    [SerializeField] public int moveSpeed = 5;
    public Vector2 InputVector { get; private set; }
    
    public Animator ani;
    
    public PlayerStateMachine StateMachine { get; private set; }
    public PeaceIdleState peaceIdleState { get; private set; }
    public PeaceMoveState peaceMoveState { get; private set; }
    public CombatIdleState combatIdleState { get; private set; }
    public CombatAttackState combatAttackState { get; private set; }
    public CombatMoveState combatMoveState { get; private set; }

    void Awake()
    {
        StateMachine = new PlayerStateMachine();
        
        peaceIdleState = new PeaceIdleState(this, StateMachine, "PeaceIdle");
        peaceMoveState = new PeaceMoveState(this, StateMachine, "PeaceMove");
        combatIdleState = new CombatIdleState(this, StateMachine, "CombatIdle");
        combatMoveState = new CombatMoveState(this, StateMachine, "CombatMove");
        combatAttackState = new CombatAttackState(this, StateMachine, "CombatAttack");
        
    }
    
    void Start()
    {
        StateMachine.Initialize(combatIdleState);
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
}
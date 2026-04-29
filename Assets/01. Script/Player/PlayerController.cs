using UnityEngine;
using _01._Script;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    private float horizontalSensitivity = 1.0f;
    
    public PlayerStats playerStats;
    public LockOnSystem lockOnSystem;
    public Rigidbody rb;
    public float moveSpeed =>  playerStats.MoveSpeed;
    public Vector2 InputVector { get; private set; }
    public Animator ani;
    public GameObject handSword;
    public GameObject etcSword;
    [Range(0, 1)] [SerializeField] public float daming = 0.0f;
    public bool isWeaponInHand = false;
    public HitBox hitBox;
    public SkillHitBox skillHitBox;
    public GameObject hurtBox;
    [Header("회피")]
    public float dodgetime = DODGE_COOLDOWN;
    public bool isDodge;
    public const float DODGE_COOLDOWN = 4.0f;
    public event System.Action<float, float> OnDodgeCooldownChanged;
    public float GuardTimer { get; set; }
    public bool IsGuarding => StateMachine.CurrentState == CombatGuardState;

    #region 상태 머신 모음
    public static PlayerController Instance { get; private set; }

    private PlayerStateMachine StateMachine { get; set; }
    public PeaceIdleState PeaceIdleState { get; private set; }
    public PeaceMoveState PeaceMoveState { get; private set; }
    public CombatIdleState CombatIdleState { get; private set; }
    public CombatMoveState CombatMoveState { get; private set; }
    public Attack1State Attack1State { get; private set; }
    public Attack2State Attack2State { get; private set; }
    public Attack3State Attack3State { get; private set; }
    public CombatGuardState CombatGuardState { get; private set; }
    public  CombatDodgeState CombatDodgeState { get; private set; }
    public CombatSkillState CombatSkillState { get; private set; }
    public EnterCombatState EnterCombatState { get; private set; }
    public ExitCombatState  ExitCombatState { get; private set; }
    public CombatStunState CombatStunState { get; private set; }
    public CombatPullState CombatPullState { get; private set; }
    public CombatDieState CombatDieState { get; private set; }

    #endregion
    

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        rb = GetComponent<Rigidbody>();
        StateMachine = new PlayerStateMachine();
        
        PeaceIdleState = new PeaceIdleState(this, StateMachine, "PeaceIdle", false);
        PeaceMoveState = new PeaceMoveState(this, StateMachine, "PeaceMove", false);
        CombatIdleState = new CombatIdleState(this, StateMachine, "CombatIdle", false);
        CombatMoveState = new CombatMoveState(this, StateMachine, "CombatMove", false);
        Attack1State = new Attack1State(this, StateMachine, "Attack1State", false);
        Attack2State = new Attack2State(this, StateMachine, "Attack2State", false);
        Attack3State = new Attack3State(this, StateMachine, "Attack3State", false);
        CombatGuardState = new CombatGuardState(this, StateMachine, "CombatGuard", true);
        CombatDodgeState = new CombatDodgeState(this, StateMachine, "CombatDodge", false);
        CombatSkillState = new CombatSkillState(this, StateMachine, "CombatSkill", false);
        EnterCombatState = new EnterCombatState(this, StateMachine, "EnterCombat", false);
        ExitCombatState = new ExitCombatState(this, StateMachine, "ExitCombat", false);
        CombatStunState = new CombatStunState(this, StateMachine, "CombatStun", false);
        CombatPullState = new CombatPullState(this, StateMachine, "CombatPull", false);
        CombatDieState = new CombatDieState(this, StateMachine ,"CombatDie",false);
    }
    
    void Start()
    {
        StateMachine.Initialize(PeaceIdleState);
    }
    
    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        InputVector = new Vector2(x, y).normalized;
        StateMachine.CurrentState.LogicUpdate();
        UpdateDodgeTimer();
        isDie();
    }

    void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();
    public void isStunned()
    {
        EffectManager.Instance.StopEffectsUnder(transform);
        StateMachine.ChangeState(CombatStunState);
    }

    public void isPulling() => StateMachine.ChangeState(CombatPullState);

    public void ResetState()
    {
        StateMachine.ChangeState(PeaceIdleState);
        // 애니메이터 파라미터 초기화가 필요하다면 여기서 수행합니다.
        ani.Play("PeaceIdle", 0, 0f);
    }
    
    private void isDie()
    {
        if (playerStats.CurrentHp <= 0 && StateMachine.CurrentState != CombatDieState)
        {
            EffectManager.Instance.StopEffectsUnder(transform);
            StateMachine.ChangeState(CombatDieState);
            StartCoroutine(DieSequenceRoutine());
        }
    }

    private System.Collections.IEnumerator DieSequenceRoutine()
    {
        // 1초 대기 (사망 애니메이션 등을 보여주기 위함)
        yield return new WaitForSeconds(5.0f);
        
        // UIManager를 통해 사망 UI 표시
        if (_01._Script.UI.UIManager.Instance != null)
        {
            _01._Script.UI.UIManager.Instance.ShowDieUI();
        }
    }
    
    public bool AttemptSkillUse()
    {
        if (playerStats.CanUseSkill())
        {
            playerStats.UseSkill();
            
            StateMachine.ChangeState(CombatSkillState);
            return true;
        }
        Debug.Log("[Skill] 포인트가 부족합니다! (현재 포인트 필요: 8)");
        return false;
    }
    
    public void UpdateGuardTimer() => GuardTimer += Time.deltaTime;

    public void UpdateDodgeTimer()
    {
        if (isDodge)
        {
            dodgetime += Time.deltaTime;
            OnDodgeCooldownChanged?.Invoke(dodgetime, DODGE_COOLDOWN);
            
            if (dodgetime >= DODGE_COOLDOWN)
            {
                isDodge = false;
                OnDodgeCooldownChanged?.Invoke(DODGE_COOLDOWN, DODGE_COOLDOWN);
            }
        }
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

    public void PlayEffect(string effectName)
    {
        EffectManager.Instance.PlayEffect(effectName, transform);
        SoundManager.Instance.PlaySFX(effectName, transform.position);
    }

    #endregion
}
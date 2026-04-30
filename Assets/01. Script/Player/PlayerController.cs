using System.Collections;
using _01._Script.CombatSystem.PlayerHitBox;
using _01._Script.Data;
using _01._Script.Effect;
using _01._Script.Player.PlayerState;
using _01._Script.Player.PlayerState.CombatState;
using _01._Script.Player.PlayerState.PeaceState;
using _01._Script.UI;
using UnityEngine;

namespace _01._Script.Player
{
    public class PlayerController : MonoBehaviour
    {
        public PlayerStats playerStats;
        public LockOnSystem lockOnSystem;
        public Rigidbody rb;
        public float MoveSpeed =>  playerStats.MoveSpeed;
        public Vector2 InputVector { get; private set; }
        public Animator ani;
        public GameObject handSword;
        public GameObject etcSword;
        [Range(0, 1)] [SerializeField] public float daming;
        public bool isWeaponInHand;
        public HitBox hitBox;
        public SkillHitBox skillHitBox;
        public GameObject hurtBox;
        
        [Header("Dodge")]
        public float dodgetime = DodgeCooldown;
        public bool isDodge;
        public const float DodgeCooldown = 4.0f;
        public event System.Action<float, float> OnDodgeCooldownChanged;
        public float GuardTimer { get; set; }
        public bool IsGuarding => StateMachine.CurrentState == CombatGuardState;

        private bool _isDead;
        
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
        private CombatSkillState CombatSkillState { get; set; }
        public EnterCombatState EnterCombatState { get; private set; }
        public ExitCombatState  ExitCombatState { get; private set; }
        private CombatStunState CombatStunState { get; set; }
        private CombatPullState CombatPullState { get; set; }
        private CombatDieState CombatDieState { get; set; }

        
        #endregion


        private void Awake()
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
        
            PeaceIdleState = new PeaceIdleState(this, StateMachine, "PeaceIdle");
            PeaceMoveState = new PeaceMoveState(this, StateMachine, "PeaceMove");
            CombatIdleState = new CombatIdleState(this, StateMachine, "CombatIdle");
            CombatMoveState = new CombatMoveState(this, StateMachine, "CombatMove");
            Attack1State = new Attack1State(this, StateMachine, "Attack1State");
            Attack2State = new Attack2State(this, StateMachine, "Attack2State");
            Attack3State = new Attack3State(this, StateMachine, "Attack3State");
            CombatGuardState = new CombatGuardState(this, StateMachine, "CombatGuard", true);
            CombatDodgeState = new CombatDodgeState(this, StateMachine, "CombatDodge");
            CombatSkillState = new CombatSkillState(this, StateMachine, "CombatSkill");
            EnterCombatState = new EnterCombatState(this, StateMachine, "EnterCombat");
            ExitCombatState = new ExitCombatState(this, StateMachine, "ExitCombat");
            CombatStunState = new CombatStunState(this, StateMachine, "CombatStun");
            CombatPullState = new CombatPullState(this, StateMachine, "CombatPull");
            CombatDieState = new CombatDieState(this, StateMachine ,"CombatDie");
        }

        private void Start()
        {
            StateMachine.Initialize(PeaceIdleState);
        }

        private void Update()
        {
            var x = Input.GetAxisRaw("Horizontal");
            var y = Input.GetAxisRaw("Vertical");
            InputVector = new Vector2(x, y).normalized;
            StateMachine.CurrentState.LogicUpdate();
            UpdateDodgeTimer();
            IsDie();
        }

        private void FixedUpdate() => StateMachine.CurrentState.PhysicsUpdate();
        
        public void IsStunned()
        {
            if (StateMachine.CurrentState == CombatDieState) return;
            EffectManager.Instance.StopEffectsUnder(transform);
            StateMachine.ChangeState(CombatStunState);
        }

        public void IsPulling()
        {
            if (StateMachine.CurrentState == CombatDieState) return;
            StateMachine.ChangeState(CombatPullState);
        } 

        public void ResetState()
        {
            _isDead = false;
            StateMachine.ChangeState(PeaceIdleState);
            ani.Play("PeaceIdle", 0, 0f);
        }
    
    
        private void IsDie()
        {
            if (playerStats.CurrentHp > 0 || _isDead) return;
        
            _isDead = true;
            EffectManager.Instance.StopEffectsUnder(transform);
            StateMachine.ChangeState(CombatDieState);
            StartCoroutine(DieSequenceRoutine());
        }

        private IEnumerator DieSequenceRoutine()
        {
            yield return new WaitForSeconds(3.0f);
            
            if (UIManager.Instance != null)
            {
                UIManager.Instance.ShowDieUI();
            }
        }
    
        public void AttemptSkillUse()
        {
            if (playerStats.CanUseSkill())
            {
                playerStats.UseSkill();
            
                StateMachine.ChangeState(CombatSkillState);
            }
        }
    
        public void UpdateGuardTimer() => GuardTimer += Time.deltaTime;

        private void UpdateDodgeTimer()
        {
            if (!isDodge) return;
            dodgetime += Time.deltaTime;
            OnDodgeCooldownChanged?.Invoke(dodgetime, DodgeCooldown);

            if ((dodgetime >= DodgeCooldown) == false) return;
            isDodge = false;
            OnDodgeCooldownChanged?.Invoke(DodgeCooldown, DodgeCooldown);
        } 
        #region 애니메이션 이벤트 함수 모음

        public void WeaponSwitch() 
        {
            if (isWeaponInHand)
            {
                etcSword.SetActive(false);
                handSword.SetActive(true);
            }
            else
            {
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
}
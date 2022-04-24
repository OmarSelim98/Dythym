using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI; //DEBUG
using UnityEngine.Audio; //DEBUG


public class PlayerStateMachine : MonoBehaviour
{
    [SerializeField] SOPlayerStats player_stats; //fixed player stats saved as so.
    [SerializeField] SOAudioStats _audioStats; //fixed player stats saved as so.
    Transform playerTransform;
    CharacterController playerController; // reference to the player controller.
    PlayerInput playerInput; // reference to the input system.
    Animator playerAnimator; // reference to the animator.
    [SerializeField] GameObject dashVolume;
    [SerializeField] Vector3 playerMovement; // player current movement, applied to the Move() function in the character controller

    PlayerAbstractState _currentState;
    PlayerStateFactory _states;

    [SerializeField] GameObject katanaSlash;
    // Animations Hashes
    int isWalkingHash = Animator.StringToHash("IsWalking"); // walking hash
    int isJumpingHash = Animator.StringToHash("IsJumping"); // jumping hash
    int isDashingHash = Animator.StringToHash("IsDashing"); // dashing hash
    int h_attack = Animator.StringToHash("attack");
    int h_hitCount = Animator.StringToHash("hitCount");
    List<int> h_attackAnimationList = new List<int>(new int[]
    {   Animator.StringToHash("LightAttk1"),
        Animator.StringToHash("LightAttk2"),
        Animator.StringToHash("LightAttk3")});
    List<float> animationDelay = new List<float>(new float[] { 0.17f, 0.2f, 0.19f});
    //List<Material> materials
    [SerializeField]
    List<GameObject> animationSlash = new List<GameObject>();
    [SerializeField]
    List<Material> katanaDissolve = new List<Material>();
    [SerializeField] List<SOOneShot> _attacksAudio;


    [SerializeField] bool canApplyGravity = true;
    bool isJumpPressed = false;
    bool isDashPressed = false;
    bool canJump = true;
    bool isMovementPressed = false;
    bool timedFunctionFinished = false;
    Vector2 movementVector = Vector2.right;
    Vector2 cachedMovementVector; // used for dashing direction

    AudioSource playerAudio;

    /** 
    DEBUG
    */
    [SerializeField] Text state_debug;
    [SerializeField] Text substate_debug;

    //Attacking
    int _hitCounter = 0;
    bool _isAttackingPressed = false;

    public PlayerAbstractState CurrentState { get => _currentState; set => _currentState = value; }
    public float PlayerMovementX { get => playerMovement.x; set => playerMovement.x = value; }
    public float PlayerMovementZ { get => playerMovement.z; set => playerMovement.z = value; }
    public float PlayerMovementY { get => playerMovement.y; set => playerMovement.y = value; }

    public Transform PlayerTransform { get => playerTransform; }
    public Quaternion PlayerRotation { get => playerTransform.rotation; set => playerTransform.rotation = value; }
    public float PlayerTransformY { get => playerTransform.position.y; }
    public float MovementVectorX { get => movementVector.x; }
    public float MovementVectorY { get => movementVector.y; }
    public SOPlayerStats PLAYER_STATS { get => player_stats; set => player_stats = value; }
    public bool IsJumpPressed { get => isJumpPressed; set => isJumpPressed = value; }
    public bool IsDashPressed { get => isDashPressed; set => isDashPressed = value; }
    public bool CanJump { get => canJump; set => canJump = value; }
    public Animator PlayerAnimator { get => playerAnimator; set => playerAnimator = value; }
    public int IsWalkingHash { get => isWalkingHash; set => isWalkingHash = value; }
    public int IsJumpingHash { get => isJumpingHash; set => isJumpingHash = value; }
    public int IsDashingHash { get => isDashingHash; }
    public bool IsMovementPressed { get => isMovementPressed; set => isMovementPressed = value; }
    public CharacterController PlayerController { get => playerController; set => playerController = value; }
    public int HitCounter { get => _hitCounter; set => _hitCounter = value; }
    public bool IsAttackingPressed { get => _isAttackingPressed; set => _isAttackingPressed = value; }
    public int H_attack { get => h_attack; }
    public List<int> H_attackAnimationList { get => h_attackAnimationList; }
    public List<float> AnimationDelay { get => animationDelay; }
    public List<GameObject> AnimationSlash { get => animationSlash; }
    public int H_hitCount { get => h_hitCount; }
    public PlayerInput PlayerInput { get => playerInput; set => playerInput = value; }
    public bool TimedFunctionFinished { get => timedFunctionFinished; set => timedFunctionFinished = value; }

    public PlayerAbstractState MainState { get => _currentState; }
    public bool CanApplyGravity { get => canApplyGravity; set => canApplyGravity = value; }
    public Vector2 CachedMovementVector { get => cachedMovementVector; }
    public GameObject DashVolume { get => dashVolume; set => dashVolume = value; }
    public SOAudioStats AudioStats { get => _audioStats; set => _audioStats = value; }
    public GameObject KatanaSlash { get => katanaSlash; }

    public AudioSource PlayerAudio { get => playerAudio; }
    public List<SOOneShot> AttacksAudio { get => _attacksAudio; }
    public List<Material> KatanaDissolve { get => katanaDissolve; set => katanaDissolve = value; }

    void Awake()
    {
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        playerInput = new PlayerInput();
        playerAudio = GetComponent<AudioSource>();
        playerController = GetComponent<CharacterController>();
        player_stats.SetupJumpVars();

        _states = new PlayerStateFactory(this);

        _currentState = _states.Grounded();
        _currentState.EnterState();

        playerInput.Controller.Jump.started += onJump;
        playerInput.Controller.Jump.canceled += onJump;

        playerInput.Controller.Move.started += onMove;
        playerInput.Controller.Move.performed += onMove;
        playerInput.Controller.Move.canceled += onMove;

        playerInput.Controller.Attack.started += onAttack;

        playerInput.Controller.Dash.started += OnDash;
    }

    void onJump(InputAction.CallbackContext ctx)
    {
        if (_audioStats.canPerformAction)
        {
            isJumpPressed = ctx.ReadValueAsButton();
        }
    }

    void OnDash(InputAction.CallbackContext ctx)
    {
        if (_audioStats.canPerformAction)
        {
            foreach (OneShotOnBeat el in _audioStats.roomAudio.dashList)
            {
                if (_audioStats.CurrentPlayableBeat == el.ForBeat)
                {
                    playerAudio.PlayOneShot(el.OneShot.file);
                    break;
                }
            }
            isDashPressed = ctx.ReadValueAsButton();
            Debug.Log("Dash button pressed");
        }
    }
    void onMove(InputAction.CallbackContext ctx)
    {
        movementVector = ctx.ReadValue<Vector2>();
        isMovementPressed = movementVector.x != 0 || movementVector.y != 0;
        if (isMovementPressed)
        {
            cachedMovementVector = movementVector;
        }
    }
    void onAttack(InputAction.CallbackContext ctx)
    {
        if (_audioStats.canPerformAction)
        {
            foreach(OneShotOnBeat el in _audioStats.roomAudio.attackList)
            {
                if(_audioStats.CurrentPlayableBeat == el.ForBeat)
                {
                    playerAudio.PlayOneShot(el.OneShot.file);
                    break;
                }
            }
            _isAttackingPressed = ctx.ReadValueAsButton();
        }
    }
    public bool isPlayerGrounded()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, player_stats.DISTANCE_TO_GROUND))
        {
            return true;
        }
        else
        {

            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _currentState.UpdateStates();
        state_debug.text = _currentState.getName();
        substate_debug.text = _currentState._subState.getName();
        //CacheMovementInput();
        playerController.Move(playerMovement * Time.deltaTime);
    }
    void OnEnable()
    {
        playerInput.Controller.Enable();
    }
    void OnDisable()
    {
        playerInput.Controller.Disable();
    }

    public void StartTimedFunction(float time)
    {
        StartCoroutine(TimedFunction(time));
    }
    private IEnumerator TimedFunction(float time)
    {
        timedFunctionFinished = false;
        yield return new WaitForSeconds(time);
        timedFunctionFinished = true;
    }

    public void SetPositionY(float value)
    {

        playerTransform.position = new Vector3(playerTransform.position.x, value, playerTransform.position.z);
    }
    public void DisableCharacterController()
    {
        playerController.enabled = false;
    }
    public void EnableCharacterController()
    {
        playerController.enabled = true;
    }
    void CacheMovementInput()
    {
        if (movementVector.x != 0 || movementVector.y != 0)
        {
            cachedMovementVector = movementVector;
        }
    }
}

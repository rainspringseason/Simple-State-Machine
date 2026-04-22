using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 4f;

    [Header("Attack Settings")]
    [SerializeField] private LayerMask attackableLayer;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private Vector2 attackOffsetUp = new Vector2(0, 1f);
    [SerializeField] private Vector2 attackOffsetDown = new Vector2(0, -1f);
    [SerializeField] private Vector2 attackOffsetLeft = new Vector2(-1f, 0);
    [SerializeField] private Vector2 attackOffsetRight = new Vector2(1f, 0);

    [Header("Ability Settings")]
    [SerializeField] private ProjectileAbility projectileAbility;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private PlayerVisual playerVisual;

    private StateMachine<PlayerStateMachine> stateMachine;

    public float MoveSpeed => moveSpeed;
    public Rigidbody2D Rb => rb;
    public PlayerInput PlayerInput => playerInput;
    public PlayerVisual PlayerVisual => playerVisual;
    public Direction CurrentDirection { get; set; } = Direction.Down;
    public Vector2 MovementDirection { get; set; }
    public ProjectileAbility ProjectileAbility => projectileAbility;

    public LayerMask AttackableLayer => attackableLayer;
    public float AttackRange => attackRange;
    public Vector2 AttackOffsetUp => attackOffsetUp;
    public Vector2 AttackOffsetDown => attackOffsetDown;
    public Vector2 AttackOffsetLeft => attackOffsetLeft;
    public Vector2 AttackOffsetRight => attackOffsetRight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
        playerVisual = GetComponent<PlayerVisual>();

        InitializeStateMachine();
    }

    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine<PlayerStateMachine>(this);

        var idleState = new PlayerIdleState();
        var walkState = new PlayerWalkState();
        var attackState = new PlayerAttackState();
        var abilityState = new PlayerAbilityState();

        idleState.Initialize(stateMachine, this);
        walkState.Initialize(stateMachine, this);
        attackState.Initialize(stateMachine, this);
        abilityState.Initialize(stateMachine, this);

        stateMachine.AddState(idleState);
        stateMachine.AddState(walkState);
        stateMachine.AddState(attackState);
        stateMachine.AddState(abilityState);

        stateMachine.Initialize<PlayerIdleState>();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixedUpdate();
    }

    public Direction GetDirectionToMouse()
    {
        Vector2 mousePos = playerInput.GetMousePosition();
        Vector2 playerPos = transform.position;
        Vector2 dir = mousePos - playerPos;

        return DirectionHelper.Vector2ToDirection(dir);
    }

    public Vector2 GetAttackOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return attackOffsetUp;
            case Direction.Down: return attackOffsetDown;
            case Direction.Left: return attackOffsetLeft;
            case Direction.Right: return attackOffsetRight;
            default: return attackOffsetDown;
        }
    }

    public void AttackHit()
    {
        Direction attackDirection = GetDirectionToMouse();
        Vector2 attackOffset = GetAttackOffset(attackDirection);
        Vector2 attackPosition = (Vector2)transform.position + attackOffset;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition, attackRange, attackableLayer);

        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.OnHit();
            }
        }
    }
}

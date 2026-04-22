using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileAbility : AbilityBase
{
    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float projectileLifetime = 3f;

    [Header("Projectile Spawn Offset")]
    [SerializeField] private Vector2 spawnOffsetUp = new Vector2(0, 0.5f);
    [SerializeField] private Vector2 spawnOffsetDown = new Vector2(0, -0.5f);
    [SerializeField] private Vector2 spawnOffsetLeft = new Vector2(-0.5f, 0f);
    [SerializeField] private Vector2 spawnOffsetRight = new Vector2(0.5f, 0f);

    protected override IEnumerator ExecuteAbilityRoutine()
    {
        isExecuting = true;

        Direction abilityDirection = playerStateMachine.GetDirectionToMouse();

        yield return StartCoroutine(PlayAbilityAnimation(abilityDirection));
        yield return StartCoroutine(ExecuteAbility(abilityDirection));
        yield return StartCoroutine(WaitForAnimationEnd(abilityDirection));

        isExecuting = false;
    }

    protected override IEnumerator ExecuteAbility(Direction direction)
    {
        if (projectilePrefab == null)
            yield break;

        Vector2 directionVector = DirectionHelper.DirectionToVector2(direction);
        Vector2 spawnPosition = (Vector2)transform.position + GetSpawnOffset(direction);

        GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        Projectile projectileScript = projectile.GetComponent<Projectile>();
        if (projectileScript != null)
        {
            projectileScript.Initialize(
                directionVector,
                projectileSpeed,
                abilityData.damage,
                projectileLifetime,
                abilityData.targetLayer
            );
        }

        yield return null;
    }

    protected override IEnumerator PlayAbilityAnimation(Direction direction)
    {
        if (playerVisual != null)
        {
            playerVisual.PlayFirstAbility(direction);
        }

        Vector2 moveInput = playerInput.Move;
        playerStateMachine.CurrentDirection = DirectionHelper.Vector2ToDirection(moveInput);

        yield return new WaitForSeconds(abilityData.castTime);
    }

    private Vector2 GetSpawnOffset(Direction direction)
    {
        switch (direction)
        {
            case Direction.Up: return spawnOffsetUp;
            case Direction.Down: return spawnOffsetDown;
            case Direction.Left: return spawnOffsetLeft;
            case Direction.Right: return spawnOffsetRight;
            default: return spawnOffsetDown;
        }
    }
}

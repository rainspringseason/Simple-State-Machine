using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Data", menuName = "My Game/Combat/Ability Data")]
public class AbilityDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string abilityName;
    [TextArea] public string description;
    public Sprite icon;

    [Header("Stats")]
    public float damage = 15f;
    public float range = 3f;

    [Header("Timing")]
    public float castTime = 0.3f;
    public float cooldown = 2f;
    public float duration = 0f;

    [Header("Target Settings")]
    public LayerMask targetLayer;
}

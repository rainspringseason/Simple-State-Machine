using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public Vector2 Move { get; private set; }
    public bool NormalAttack { get; private set; }
    public bool FirstAbility { get; private set; }

    [Header("Move Keys")]
    [SerializeField] private KeyCode moveUpKey = KeyCode.W;
    [SerializeField] private KeyCode moveDownKey = KeyCode.S;
    [SerializeField] private KeyCode moveLeftKey = KeyCode.A;
    [SerializeField] private KeyCode moveRightKey = KeyCode.D;

    [Header("Attack Keys")]
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;

    [Header("Ability Keys")]
    [SerializeField] private KeyCode firstAbilityKey = KeyCode.Mouse1;

    #region Unity Methods
    private void Update()
    {
        MoveInput();
        NormalAttackInput();
        FirstAbilityInput();
    }
    #endregion

    #region Input
    private void MoveInput()
    {
        float XInput = 0f;
        float YInput = 0f;

        if (Input.GetKey(moveUpKey)) YInput++;
        if (Input.GetKey(moveDownKey)) YInput--;
        if (Input.GetKey(moveLeftKey)) XInput--;
        if (Input.GetKey(moveRightKey)) XInput++;

        Move = new Vector2(XInput, YInput).normalized;
    }

    private void NormalAttackInput()
    {
        NormalAttack = Input.GetKeyDown(attackKey);
    }

    private void FirstAbilityInput()
    {
        FirstAbility = Input.GetKeyDown(firstAbilityKey);
    }
    #endregion

    #region Utility Methods
    public Vector3 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    #endregion
}

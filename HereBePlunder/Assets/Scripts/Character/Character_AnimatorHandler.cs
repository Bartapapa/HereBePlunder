using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_AnimatorHandler : MonoBehaviour
{
    public enum TriggerActions
    {
        NoAction = 0,
        Jump = 1,
        Dash = 2,
        Attack = 3,
        ChargedAttack = 4,
        Equip = 5,
    }

    [SerializeField] private KRB_CharacterController _controller;
    public Animator Animator;

    private bool _isInAir = false;

    [Header("Combat")]
    [SerializeField] private Weapon _currentWeapon;
    private int _attackCounter = 0;

    private void Update()
    {
        Vector3 currentVelocity = _controller.Motor.Velocity;

        //Check jump/fall.
        if (!_controller.Motor.GroundingStatus.IsStableOnGround)
        {
            _isInAir = true;

            if (currentVelocity.y <= 0)
            {
                Animator.SetInteger("Jumping", 2);
            }
            else
            {
                Animator.SetInteger("Jumping", 1);
            }
            
        }
        else
        {
            if (_isInAir)
            {
                //Landing
                _isInAir = false;
                Animator.SetInteger("Jumping", 3);
            }
            else if (!_isInAir)
            {
                Animator.SetInteger("Jumping", 0);
            }
        }

        Animator.SetFloat("Velocity X", transform.InverseTransformDirection(currentVelocity).x);
        Animator.SetFloat("Velocity Z", transform.InverseTransformDirection(currentVelocity).z);
    }

    public void JumpTrigger()
    {
        Animator.SetInteger("Jumping", 1);
        SetAnimatorTrigger(TriggerActions.Jump);
    }

    public void DashTrigger()
    {
        SetAnimatorTrigger(TriggerActions.Dash);
    }

    public void AttackTrigger()
    {
        if (!_controller.CanAttack || Animator.GetBool("AttackRequested")) return;

        Animator.SetBool("AttackRequested", true);
        _attackCounter++;
        Animator.SetInteger("AttackCount", _attackCounter);

        SetAnimatorTrigger(TriggerActions.Attack);
    }

    public void SetAnimatorTrigger(TriggerActions triggerAction)
    {
        Animator.SetInteger("TriggerAction", (int)triggerAction);
        Animator.SetTrigger("Trigger");
    }

    public void ClearAttackCounter()
    {
        _attackCounter = 0;
        Animator.SetInteger("AttackCount", 0);
    }

    public void ClearAttackRequest()
    {
        Animator.SetBool("AttackRequested", false);
    }

    public void CanAttack()
    {
        _controller.CanAttack = true;
    }

    public void CannotAttack()
    {
        _controller.CanAttack = false;
    }

    public void ToggleRootMotion(bool on)
    {
        Animator.applyRootMotion = on;
        _controller.RootMotionActive = on;
    }
    private void OnAnimatorMove()
    {
        // Accumulate rootMotion deltas between character updates 
        _controller.RootMotionPositionDelta += Animator.deltaPosition;
        _controller.RootMotionRotationDelta = Animator.deltaRotation * _controller.RootMotionRotationDelta;
    }

    public void CharacterAct()
    {
        float actDuration;

        if (_currentWeapon != null)
        {
            if (Animator.GetInteger("AttackCount") == 0)
            {
                Debug.Log("AttackCount in Animator = 0, using default act duration.");
                actDuration = .5f;
            }
            else
            {
                int currentAttack = Animator.GetInteger("AttackCount");
                actDuration = _currentWeapon.Attacks[currentAttack - 1].ActTime;
                Debug.Log("Character acted for " + actDuration + " seconds.");
            }
        }
        else
        {
            Debug.Log("No weapon found, using default act duration.");
            actDuration = .5f;
        }

        _controller.Act(actDuration);
    }
}

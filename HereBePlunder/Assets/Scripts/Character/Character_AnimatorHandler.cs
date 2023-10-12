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
    [SerializeField] private Character _character;
    public Animator Animator;

    private bool _isInAir = false;

    [Header("Equip transforms")]
    [SerializeField] private Transform _rightHandParent;
    [SerializeField] private Transform _leftHandParent;

    [Header("Combat")]
    [SerializeField] private Weapon _currentWeapon;
    [SerializeField] private WeaponObject _rightWeapon;
    [SerializeField] private WeaponObject _leftWeapon;
    private int _attackCounter = 0;

    private void Start()
    {
        if (_currentWeapon != null)
        {
            Equip(_currentWeapon);
        }
    }

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


    public void SetAnimatorTrigger(TriggerActions triggerAction)
    {
        Animator.SetInteger("TriggerAction", (int)triggerAction);
        Animator.SetTrigger("Trigger");
    }

    #region COMBAT

    public void ResetAttack()
    {
        if (_rightWeapon)
        {
            _rightWeapon.DisableAllColliders();
        }

        if (_leftWeapon)
        {
            _leftWeapon.DisableAllColliders();
        }

        _attackCounter = 0;
        Animator.SetInteger("AttackCount", 0);
        Animator.SetBool("AttackRequested", false);
    }

    public void Equip(Weapon weaponToEquip)
    {
        //TODO: if weapon already equipped, unequip it.
        _currentWeapon = weaponToEquip;

        if (weaponToEquip.RightWeapon == null)
        {
            Debug.LogWarning(weaponToEquip + "'s rightweapon is null.");
            return;
        }
        else
        {
            WeaponObject newRightWeapon = Instantiate<WeaponObject>(weaponToEquip.RightWeapon, _rightHandParent);
            newRightWeapon.InitializeColliders(_character);
            _rightWeapon = newRightWeapon;
        }

        if (weaponToEquip.IsDualWielded)
        {
            if (weaponToEquip.LeftWeapon == null)
            {
                Debug.LogWarning(weaponToEquip + "'s leftweapon is null.");
                return;
            }
            else
            {
                WeaponObject newLeftWeapon = Instantiate<WeaponObject>(weaponToEquip.LeftWeapon, _leftHandParent);
                newLeftWeapon.InitializeColliders(_character);
                _leftWeapon = newLeftWeapon;
            }
        }
    }

    public void Unequip()
    {
        _currentWeapon = null;
        GameObject currentHeldRightWeapon = _rightHandParent.GetChild(0).gameObject;
        GameObject currentHeldLeftWeapon = _leftHandParent.GetChild(0).gameObject;
        _rightWeapon = null;
        _leftWeapon = null;
        Destroy(currentHeldRightWeapon);
        Destroy(currentHeldLeftWeapon);
    }
    public void AttackTrigger()
    {
        if (!_controller.CanAttack || Animator.GetBool("AttackRequested")) return;

        Animator.SetBool("AttackRequested", true);
        _attackCounter++;
        Animator.SetInteger("AttackCount", _attackCounter);

        SetAnimatorTrigger(TriggerActions.Attack);
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

        //As a failsafe, return all WeaponColliders to disabled so that they don't stay enabled through a transition into another attack.
        if (_rightWeapon != null)
        {
            _rightWeapon.DisableAllColliders();
        }
        if (_leftWeapon != null)
        {
            _leftWeapon.DisableAllColliders();
        }
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

    public void EnableRightWeaponCollider(int index)
    {
        if (_rightWeapon == null)
        {
            Debug.LogWarning(this.name + " doesn't have a right weapon equipped.");
            return;
        }

        _rightWeapon.ToggleCollider(index, true);
    }

    public void DisableRightWeaponCollider(int index)
    {
        if (_rightWeapon == null)
        {
            Debug.LogWarning(this.name + " doesn't have a right weapon equipped.");
            return;
        }

        _rightWeapon.ToggleCollider(index, false);
    }

    public void EnableLeftWeaponCollider(int index)
    {
        if (_leftWeapon == null)
        {
            Debug.LogWarning(this.name + " doesn't have a left weapon equipped.");
            return;
        }

        _leftWeapon.ToggleCollider(index, true);
    }

    public void DisableLeftWeaponCollider(int index)
    {
        if (_leftWeapon == null)
        {
            Debug.LogWarning(this.name + " doesn't have a left weapon equipped.");
            return;
        }

        _leftWeapon.ToggleCollider(index, false);
    }

    #endregion

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
                Debug.LogWarning("AttackCount in Animator = 0, using default act duration.");
                actDuration = .5f;
            }
            else
            {
                int currentAttack = Animator.GetInteger("AttackCount");
                actDuration = _currentWeapon.Attacks[currentAttack - 1].ActTime;
                //Debug.Log("Character acted for " + actDuration + " seconds.");
            }
        }
        else
        {
            Debug.LogWarning("No weapon found, using default act duration.");
            actDuration = .5f;
        }

        _controller.Act(actDuration);
    }
}

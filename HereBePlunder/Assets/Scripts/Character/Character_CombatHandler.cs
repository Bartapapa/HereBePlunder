using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_CombatHandler : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Character_AnimatorHandler AnimatorHandler;

    [Header("Weapon")]
    [SerializeField] private Weapon _currentWeapon;
    public Weapon CurrentWeapon => _currentWeapon;

    [Header("Combo")]
    [SerializeField] private float _cooldownTime = 2f;
    private float _lastRequestedAttack = 0f;
    private float _maxComboDelay = 1.5f;
    private int _numberOfAttackRequests = 0;

    private void Update()
    {
        if(_numberOfAttackRequests > 0 && (Time.time - _lastRequestedAttack > _maxComboDelay))
        {
            _numberOfAttackRequests = 0;
            AnimatorHandler.Animator.SetInteger("AttackCount", 0);
        }

        if (_numberOfAttackRequests == 1)
        {
            AnimatorHandler.Animator.SetInteger("AttackCount", 1);
        }

        //Animation name is CurrentWeapon.AnimationName + "-Attack-" + i
        if (_numberOfAttackRequests >= 2 && AnimatorHandler.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .4f && AnimatorHandler.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .9f && AnimatorHandler.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unarmed" + "-Attack-" + 1))
        {
            AnimatorHandler.Animator.SetInteger("AttackCount", 2);
        }

        if (_numberOfAttackRequests >= 3 && AnimatorHandler.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .4f && AnimatorHandler.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime < .9f && AnimatorHandler.Animator.GetCurrentAnimatorStateInfo(0).IsName("Unarmed" + "-Attack-" + 2))
        {
            AnimatorHandler.Animator.SetInteger("AttackCount", 3);
        }
    }
}

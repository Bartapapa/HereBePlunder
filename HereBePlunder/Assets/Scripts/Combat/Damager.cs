using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private int _priority = 1;
    [SerializeField] private int _damage = 1;
    [SerializeField] private bool _canHitAllies = false;
    [HideInInspector] public Collider WCollider;
    [HideInInspector] public Character Owner;

    private void Awake()
    {
        WCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Damage code.
        Health target = other.GetComponent<Health>();
        if (target)
        {
            if (target.GetComponent<Character>() == Owner) return;

            if (_canHitAllies)
            {
                //target.Hurt(_damage, this.gameObject, Owner);
                target.RegisterDamager(_priority, _damage, this.gameObject, Owner);
            }
            else
            {
                Character targetCharacter = target.GetComponent<Character>();
                if (targetCharacter)
                {
                    if (Owner.Team == targetCharacter.Team)
                    {
                        return;
                    }
                    else
                    {
                        //target.Hurt(_damage, this.gameObject, Owner);
                        target.RegisterDamager(_priority, _damage, this.gameObject, Owner);
                    }
                }
                else
                {
                    Debug.LogWarning(target.name + " has no Character script.");
                    return;
                }
            }

        }
    }
}

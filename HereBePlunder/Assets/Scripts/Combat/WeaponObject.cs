using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [SerializeField] private List<Damager> _weaponColliders;

    public void InitializeColliders(Character owner)
    {
        foreach (Damager weapDamager in _weaponColliders)
        {
            weapDamager.WCollider.isTrigger = true;
            weapDamager.WCollider.enabled = false;
            weapDamager.Owner = owner;
        }
    }

    public void ToggleCollider(int index, bool on)
    {
        _weaponColliders[index].WCollider.enabled = on;
    }

    public void DisableAllColliders()
    {
        foreach (Damager weapDamager in _weaponColliders)
        {
            weapDamager.WCollider.enabled = false;
        }
    }
}

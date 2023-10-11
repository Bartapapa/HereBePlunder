using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(menuName = "HereBePlunder/Item/Weapon", fileName = "Weapon")]
public class Weapon : ScriptableObject
{
    public enum WeaponType
    {
        Unarmed = 0,
        SwordAndShield = 1,
        Hatchet = 2,
        Torch = 3,
        Bow = 4,
        Wand = 5,
        Staff = 6,
        Daggers = 7,
        Maul = 8,
    }

    [System.Serializable]
    public class WeaponAttack
    {
        public float ActTime = .5f;
    }

    [Header("ID")]
    [SerializeField] private int _id = -1;
    public int ID => _id;

    [Header("Item")]
    [SerializeField] private WeaponType _weaponType = WeaponType.Unarmed;
    public WeaponType Type => _weaponType;

    [SerializeField] private string _name;
    public string Name => _name;

    [Header("Combat stats")]
    [SerializeField] private float _attackSpeed = 1f;
    public float AttackSpeed => _attackSpeed;

    [SerializeField] private List<WeaponAttack> _attacks = new List<WeaponAttack>();
    public List<WeaponAttack> Attacks => _attacks;
}

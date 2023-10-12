using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private Character Character;

    public delegate void HealthEvent(int damageTaken, GameObject source, Character instigator);
    public event HealthEvent DamageTaken;
    public event HealthEvent HasDied;

    [Header("Stats")]
    [SerializeField] private int _health = 10;

    private List<DamagerNode> _damageNodes = new List<DamagerNode>();

    private void Awake()
    {
        Character = GetComponent<Character>();
    }
    private void FixedUpdate()
    {
        if (_damageNodes.Count == 0) return;
        List<DamagerNode> _highestPrioNodes = new List<DamagerNode>();
        int highestPriority = int.MinValue;

        foreach(DamagerNode damageNode in _damageNodes)
        {
            if (highestPriority <= damageNode.Priority)
            {
                highestPriority = damageNode.Priority;
            }
        }

        foreach (DamagerNode damageNode in _damageNodes)
        {
            if (damageNode.Priority >= highestPriority)
            {
                _highestPrioNodes.Add(damageNode);
            }
        }

        if (_highestPrioNodes.Count <= 1)
        {
            Hurt(_highestPrioNodes[0].DamageDealt, _highestPrioNodes[0].Source, _highestPrioNodes[0].Instigator);
        }
        else
        {
            int randomIndex = Random.Range(0, _highestPrioNodes.Count);
            Hurt(_highestPrioNodes[randomIndex].DamageDealt, _highestPrioNodes[randomIndex].Source, _highestPrioNodes[randomIndex].Instigator);
        }

        foreach (DamagerNode damageNode in _damageNodes)
        {
            Destroy(damageNode);
        }
        _damageNodes.Clear();
    }

    private void Hurt(int damage, GameObject source, Character instigator)
    {
        _health -= damage;

        if (_health > 0)
        {
            Debug.Log(Character.CharacterName + " has been hit for " + damage + " damage, by " + instigator.CharacterName + ". They have " + _health + " health left.");
            DamageTaken?.Invoke(damage, source, instigator);
        }
        else
        {
            Debug.Log(Character.CharacterName + " has been hit for " + damage + " damage, by " + instigator.CharacterName + ". They are dead.");
            DamageTaken?.Invoke(damage, source, instigator);
            HasDied?.Invoke(damage, source, instigator);
        }
    
    }

    public void RegisterDamager(int priority, int damage, GameObject source, Character instigator)
    {
        DamagerNode newDamagerNode = ScriptableObject.CreateInstance<DamagerNode>();
        newDamagerNode.RegisterNode(priority, damage, source, instigator);
        _damageNodes.Add(newDamagerNode);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerNode : ScriptableObject
{
    public int Priority = 0;
    public int DamageDealt = 0;
    public GameObject Source;
    public Character Instigator;

    public void RegisterNode(int priority, int damageDealt, GameObject source, Character instigator)
    {
        Priority = priority;
        DamageDealt = damageDealt;
        Source = source;
        Instigator = instigator;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public enum CharacterTeam
    {
        Player,
        Faction1,
        Faction2,
        Faction3
    }

    public CharacterTeam Team = CharacterTeam.Player;

    public string CharacterName;

    public Health Health;
}

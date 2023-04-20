using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharacterStatHealthModifierSO : CharacterStatModifierSO
{
    public override void AffectCharacter(GameObject character, float val)
    {
        PlayerManager player = character.GetComponent<PlayerManager>();
        if (player != null)
            player.AddHealth((int)val);
    }
}
using UnityEngine;

public class CrumbleBlock : GimmickBase
{
    public override void ToPlayerAction(Player character, eHitType hitType)
    {
        Debug.Log("崩落");
    }
}

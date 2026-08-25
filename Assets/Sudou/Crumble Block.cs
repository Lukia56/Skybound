using UnityEngine;

public class CrumbleBlock : GimmickBase
{
    public override void GimmickAction(Player character, eHitType hitType)
    {
        Debug.Log("崩落");
    }
}

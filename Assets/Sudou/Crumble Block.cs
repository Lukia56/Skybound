using UnityEngine;

public class CrumbleBlock : GimmickBase
{
    public override void ToObjectAction(GimmickObject gimmickObject)
    {
        Debug.Log("崩落");
        gimmickObject.gameObject.SetActive(false);
    }
}

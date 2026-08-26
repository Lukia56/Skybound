using UnityEngine;

public class CollisionCensor : MonoBehaviour
{
    private const int _MAP_LAYER_NUM = 3;
    public bool isHit {  get; private set; }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer != _MAP_LAYER_NUM) return;
        isHit = true;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer != _MAP_LAYER_NUM) return;
        isHit = true;
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer != _MAP_LAYER_NUM) return;
        isHit = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != _MAP_LAYER_NUM) return;
        isHit = false;
    }
}

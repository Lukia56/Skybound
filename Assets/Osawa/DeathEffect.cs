using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DestroyOnAnimationEnd();
    }

    private void DestroyOnAnimationEnd()
    {
        if (_animator == null) return;

        AnimatorStateInfo info = _animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime > 1.0f)
        {
            Destroy(gameObject);
        }
    }
}

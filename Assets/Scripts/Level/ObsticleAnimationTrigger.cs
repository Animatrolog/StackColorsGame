using UnityEngine;

public class ObsticleAnimationTrigger : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out TileCollector tileCollector))
        {
            _animator.enabled = true;
        }
    }
}

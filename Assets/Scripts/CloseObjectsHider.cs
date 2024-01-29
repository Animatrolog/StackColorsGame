using UnityEngine;

public class CloseObjectsHider : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _layerMask;
    
    private void FixedUpdate()
    {
        Ray ray = new(_player.position + Vector3.up, transform.position - _player.position);
        RaycastHit[] hits = Physics.SphereCastAll(ray, 1f, 200f, _layerMask);

        foreach(RaycastHit hit in hits)
        {
            var hidable = hit.transform.GetComponentInParent<HidableObject>();
            if (hidable != null)
            {
                hidable.Hide();
            }
        }
    }
}

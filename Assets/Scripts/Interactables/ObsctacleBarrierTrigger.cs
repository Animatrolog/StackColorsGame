using UnityEngine;

public class ObsctacleBarrierTrigger : MonoBehaviour
{
    [SerializeField] private ObstacleDisappear _disappear;

    private void OnTriggerEnter(Collider collider)
    {    
        if (collider.TryGetComponent<TileCollector>(out TileCollector collector))
        {           
            _disappear.enabled = true;
            enabled = false;
        }
    }
}
using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.Events;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private SplineComputer _splineComputer;
    public UnityEvent OnPlayerEnterTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out TileCollector collector))
        {

            OnPlayerEnterTrigger?.Invoke();
        }
    }
}

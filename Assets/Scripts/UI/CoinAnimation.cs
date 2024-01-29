using System.Collections;
using UnityEngine;

public class CoinAnimation : MonoBehaviour
{
    [SerializeField] private float _duration = 1f;

    public void Animate(Vector3 targetPosition)
    {
        StartCoroutine(Animation(targetPosition));
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }

    private IEnumerator Animation(Vector3 targetPosition)
    {
        float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
        
        while (distanceToTarget > 10f)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime / _duration);
            distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            yield return null;
        }

        Destroy(gameObject);
    }
}

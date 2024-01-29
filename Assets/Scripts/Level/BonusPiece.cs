using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusPiece : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private GameObject _particles;
    [SerializeField] private AudioClip _bonusClip;

    private List<CollectableTile> _triggerTiles = new();

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out CollectableTile tile))
        {
            if (_triggerTiles.Contains(tile)) return;
            _triggerTiles.Add(tile);
            StartCoroutine(DelayCoroutine(tile ,0.25f));
        }
    }

    private IEnumerator DelayCoroutine(CollectableTile tile, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (tile == null) yield break;
        var particles = Instantiate(_particles, transform);
        particles.transform.position = tile.transform.position + Vector3.up;
        GameManager.Instance.Player.PointCounter.AddPoints(5);
        Destroy(particles, 1.5f);
        if (GameDataManager.GameData.IsSoundEnabled)
            _audioSource.PlayOneShot(_bonusClip);

    }
}

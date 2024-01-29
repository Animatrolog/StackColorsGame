using UnityEngine;

public class CollectableCoin : MonoBehaviour
{ 
    private bool _isCollected;

    private void OnTriggerEnter(Collider other)
    {
        if (_isCollected) return;
        if (other.TryGetComponent(out CollectableTile tile))
        {

            if(tile.Collector != null && !tile.WasDropped)
            {
                _isCollected = true;
                GameDataManager.AddCoins(1);
                UICoinSpawner.Instance.SpawnUICoin(transform.position);
                gameObject.SetActive(false);
                return;
            }
        }

        if (other.TryGetComponent(out CoinCollector collector))
        {
            GameDataManager.AddCoins(1);
            UICoinSpawner.Instance.SpawnUICoin(transform.position);
            gameObject.SetActive(false);
        }
    }
}

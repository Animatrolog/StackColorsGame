using System.Collections;
using UnityEngine;

public class CoinsAnimation : MonoBehaviour
{
    [SerializeField] private CoinAnimation _coinPrefab;
    [SerializeField] private RectTransform _toObject;
    [SerializeField] private float _horisontalRandomSpread = 100;

    public void Animate(int coinAmount)
    {
        StartCoroutine(Animation(coinAmount));
    }

    private IEnumerator Animation(int coinAmount)
    {
        float step = 1f / coinAmount;

        for(int i = 0; i < coinAmount; i++)
        {
            Vector3 randomOffset = Vector3.zero;
            randomOffset.x = Random.Range(-_horisontalRandomSpread, _horisontalRandomSpread);
            var coin = Instantiate(_coinPrefab,transform);
            coin.transform.position = coin.transform.position + randomOffset;
            coin.Animate(_toObject.position);
            yield return new WaitForSeconds(step);
        }
    }
}

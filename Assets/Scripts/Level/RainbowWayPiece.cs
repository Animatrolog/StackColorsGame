using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RainbowWayPiece : MonoBehaviour
{
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private GameObject _frontPlanel;
    [SerializeField] private List<TMP_Text> _multiplyerTexts;
    [SerializeField] private Transform _cameraPoint;
    [SerializeField] private GameObject _particles;

    private FinishLineGenerator _generator;
    private MaterialPropertyBlock _block;
    public Transform CameraPoint => _cameraPoint;
    public float Multiplyer {  get; private set; }
    public bool IsReached {  get; private set; }


    public void Initialize(FinishLineGenerator generator, float multiplyer, Color color)
    {
        _block = new MaterialPropertyBlock();
        _generator = generator;
        Multiplyer = multiplyer;
        foreach (var text in _multiplyerTexts)
            text.text = "X" + multiplyer.ToString("F1");
        _block.SetColor("_Color",color);
        _mesh.SetPropertyBlock(_block);
        _mesh.sharedMaterials[0].SetColor("_Color", color);
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out CollectableTile tile))
        {
            if(!IsReached)
            { 
                HandleReached();
            }
        }
    }

    protected virtual void HandleReached()
    {
        IsReached = true;
        if (_frontPlanel == null) return;
        _frontPlanel.SetActive(false);
        _generator.AddReachedPiece(this);
        _particles.SetActive(true);
    }

    public void Blink()
    {
        StartCoroutine(BlinkCoroutine(45f, 1.5f));
    }

    private IEnumerator BlinkCoroutine(float frequency, float duration)
    {
        Color originalColor = _mesh.material.color;
        float progress = 0f;
        while (progress <= 1f)
        {
            _mesh.material.color = Color.Lerp(originalColor, Color.white, (Mathf.Sin(progress * frequency) + 1) / 2f);
            progress += Time.deltaTime / duration;
            yield return null;
        }
        _mesh.sharedMaterials[0].SetColor("_Color", originalColor);
    }
}

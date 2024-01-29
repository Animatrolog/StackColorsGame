using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(MeshRenderer))]
public class CollectableTile : MonoBehaviour
{
    [SerializeField] private int _scorePoints;
    [SerializeField] private ColorChanger _colorChanger;

    private Rigidbody _rigidbody;

    public int ScorePoints => _scorePoints;
    public ColorChanger ColorChanger => _colorChanger;

    public TileCollector Collector { get; private set; }
    public bool WasDropped { get; private set; }

    public Action OnTileCollected;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public void SetCollected(TileCollector collector)
    {
        Collector = collector;
        OnTileCollected?.Invoke();
    }

    public void MakeStatic()
    {
        if (Collector == null) return;
        transform.parent = Collector.transform.parent;
        WasDropped = false;
        _rigidbody.isKinematic = true;
        _rigidbody.useGravity = false;
    }

    public void MakePhysical(Vector3 dropForce)
    {
        if(Collector == null) return;
        transform.parent = null;
        WasDropped = true;
        _rigidbody.isKinematic = false;
        _rigidbody.useGravity = true;
        //_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.AddForce(dropForce, ForceMode.Impulse);
        _rigidbody.AddTorque(_rigidbody.transform.right * dropForce.magnitude);
    }
}

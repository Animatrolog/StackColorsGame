using UnityEngine;

[RequireComponent(typeof(Camera))]
public class FOVToAspectRatio : MonoBehaviour
{
    [SerializeField] private float _minFOV = 50f;
    [SerializeField] private float _maxFOV = 80f;

    private Camera _mainCamera;

    private void Awake()
    {
        _mainCamera = GetComponent<Camera>();
    }

    void Start()
    {
        float aspectRatio = (float)Screen.height / Screen.width;
        float newFOV = aspectRatio * _minFOV;
        newFOV = Mathf.Clamp(newFOV, _minFOV, _maxFOV);
        _mainCamera.fieldOfView = newFOV;
    }
}

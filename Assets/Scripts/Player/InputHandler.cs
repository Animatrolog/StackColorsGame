using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private PlayerMovement _player;
    [SerializeField] private float _sensitivity;
    [SerializeField] private Transform _collectorTransform;

    public float TargetDistance {  get; private set; }
    private Transform _cameraTransform;
    private float _distanceFromCamera;

    private void Start()
    {
        _cameraTransform = _camera.transform;
        _distanceFromCamera = Vector3.Distance(_cameraTransform.position, _collectorTransform.position);

        TargetDistance = _collectorTransform.localPosition.x;
        _touchDeltaPos = Input.mousePosition;
    }

    void Update()
    {
        HandleInput();
    }

    public void HandleInput()
    {
        if (Input.GetMouseButton(0))
        {
            _touchDeltaPos = Input.mousePosition - _touchDeltaPos;
            if (Input.GetMouseButtonDown(0))
                ResetTargetPosition();
            if(_touchDeltaPos.magnitude > 0)
            {
                _distanceFromCamera = Vector3.Distance(_cameraTransform.position, _collectorTransform.position);
                TargetDistance += (_touchDeltaPos.x / Screen.height) * _distanceFromCamera * _sensitivity;
            }

            _touchDeltaPos = Input.mousePosition;
        }
    }

    public void ResetTargetPosition()
    {
        TargetDistance = _collectorTransform.localPosition.x;
        _touchDeltaPos = Vector3.zero;
    }

    Vector3 _touchDeltaPos = Vector3.zero;

    private Vector3 WorldPositionDelta()
    {
        Vector3 touchPosition = Input.mousePosition;

        if (_touchDeltaPos.magnitude == 0)
            return Vector3.zero;

        Vector3 direction = Vector3.forward * _distanceFromCamera;
        var posBefore = _camera.ScreenToWorldPoint(touchPosition - _touchDeltaPos + direction);
        var posNow = _camera.ScreenToWorldPoint(touchPosition + direction);

        return posNow - posBefore;
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            TargetDistance += Time.deltaTime * 10f;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            TargetDistance -= Time.deltaTime * 10f;
        }
    }
}

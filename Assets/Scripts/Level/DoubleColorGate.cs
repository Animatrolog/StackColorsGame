using UnityEngine;

public class DoubleColorGate : ColorGate
{
    [SerializeField] private ColorChanger _rightColorChanger;
    [SerializeField] private ColorChanger _leftColorChanger;
    [SerializeField] private BoxCollider _collider;

    private void Awake()
    {
        _colorChanger = _rightColorChanger;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerColorChanger playerColor))
        {
            float leftDistance = Vector3.Distance(playerColor.transform.position, _leftColorChanger.transform.position);
            float rightDistance = Vector3.Distance(playerColor.transform.position, _rightColorChanger.transform.position);
            LevelPiece levelPiece = GetComponentInParent<LevelPiece>();

            if(leftDistance >= rightDistance)
            {
                playerColor.SetColor(_rightColorChanger.ColorIndex);
                if (levelPiece != null) levelPiece.SetLastGateColor(_rightColorChanger.ColorIndex);
            }
            else
            {
                playerColor.SetColor(_leftColorChanger.ColorIndex);
                if (levelPiece != null) levelPiece.SetLastGateColor(_leftColorChanger.ColorIndex);
            }
            _collider.enabled = false;
        }
    }
}

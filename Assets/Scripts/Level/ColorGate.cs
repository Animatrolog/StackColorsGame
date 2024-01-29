using UnityEngine;

public class ColorGate : MonoBehaviour
{
    [SerializeField] protected ColorChanger _colorChanger;

    public ColorChanger ColorChanger => _colorChanger;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out PlayerColorChanger playerColor))
        {
            playerColor.SetColor(_colorChanger.ColorIndex);
        }
    }
}

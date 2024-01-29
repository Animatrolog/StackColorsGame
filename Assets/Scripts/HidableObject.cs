using System.Collections.Generic;
using UnityEngine;

public class HidableObject : MonoBehaviour
{
    [SerializeField] private List<MeshRenderer> _hidableMeshes;

    public void Hide()
    {
        foreach(var mesh in _hidableMeshes)
        {
            Color newColor = mesh.material.color;
            newColor.a = 0.25f;
            mesh.material.color = newColor;
        }
    }
}

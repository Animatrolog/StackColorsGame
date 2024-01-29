using UnityEngine;

[CreateAssetMenu(fileName = "NewColorMaterialsSet", menuName = "ScriptableObjects/ColorMaterialsSet", order = 1)]
public class ColorMaterialsSet : ScriptableObject
{
    public Material[] Materials;
}

using UnityEngine;

public class IconRenderer : MonoBehaviour
{
    [SerializeField] private Camera _renderCamera;
    [SerializeField] private Transform _skinHolder;
    [SerializeField] private int _renderLayerIndex = 7;
    [SerializeField] private Texture2D _texture;

    public Texture RenderIcon(PlayerSkinModel skinPrefab)
    {
        PlayerSkinModel skinInstance = Instantiate(skinPrefab, _skinHolder);
        skinInstance.BodyRenderer.sharedMaterial = LevelManager.Instance.ColorMaterials.Materials[0];
        
        skinInstance.gameObject.layer = _renderLayerIndex;
        foreach (Transform child in skinInstance.transform)
            child.gameObject.layer = _renderLayerIndex;

        RenderTexture renderTexture = new RenderTexture(200, 200, 0);
        _renderCamera.targetTexture = renderTexture;
        _renderCamera.Render();
        skinInstance.gameObject.SetActive(false);
        Destroy(skinInstance);
        return renderTexture;
    }
}

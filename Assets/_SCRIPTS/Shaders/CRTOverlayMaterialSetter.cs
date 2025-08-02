using UnityEngine;
using UnityEngine.UI;

public class CRTOverlayMaterialSetter : MonoBehaviour
{
    [SerializeField] private Material _crtMaterial;

    private void Awake()
    {
        RawImage image = GetComponent<RawImage>();
        image.texture  = Texture2D.whiteTexture;
        image.material = _crtMaterial;
    }
}

using UnityEngine;
using UnityEngine.UI;

public class RawFishReset : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void ResetAlpha()
    {
        _image.color = new Color(1f, 1f, 1f, 0f);
    }
}
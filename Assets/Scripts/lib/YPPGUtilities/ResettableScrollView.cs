using UnityEngine;
using UnityEngine.UI;

public class ResettableScrollView : MonoBehaviour
{
    private Scrollbar _scrollbar;

    private void Awake()
    {
        _scrollbar = GetComponentInChildren<Scrollbar>();
    }

    public void ResetScrollView() => _scrollbar.value = 1f;

}

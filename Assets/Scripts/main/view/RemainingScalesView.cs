using TMPro;
using UnityEngine;

namespace main.view
{
    public class RemainingScalesView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingScalesText;

        public void Render(int remainingScales)
        {
            if (remainingScales < 0) remainingScales = 0;
            _remainingScalesText.text = remainingScales.ToString();
        }
    }
}
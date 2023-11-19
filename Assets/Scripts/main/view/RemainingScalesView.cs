using main.service.Fish_Management;
using TMPro;
using UnityEngine;

namespace main.view
{
    public class RemainingScalesView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _remainingScalesText;

        private void Start()
        {
            FishService.Instance.OnFishScalesHaveChanged.AddListener(Render);
            Render(100);
        }

        private void Render(int remainingScales)
        {
            _remainingScalesText.text = remainingScales.ToString();
        }
    }
}
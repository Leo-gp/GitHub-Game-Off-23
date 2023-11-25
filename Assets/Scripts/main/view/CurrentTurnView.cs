using main.service.Turn_System;
using TMPro;
using UnityEngine;

namespace main.view
{
    public class CurrentTurnView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentTurnText;

        private void Start()
        {
            GameService.Instance.OnTurnNumberIncreased.AddListener(Render);
            _currentTurnText.text = "1";
        }

        private void Render(int currentTurnNumber)
        {
            _currentTurnText.text = currentTurnNumber.ToString();
        }
    }
}
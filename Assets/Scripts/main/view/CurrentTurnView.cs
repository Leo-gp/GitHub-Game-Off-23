using main.service.Turn_System;
using TMPro;
using UnityEngine;
using Zenject;

namespace main.view
{
    public class CurrentTurnView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentTurnText;

        private TurnService turnService;
        
        [Inject]
        public void Construct(TurnService turnService)
        {
            this.turnService = turnService;
        }

        private void Start()
        {
            turnService.OnTurnNumberIncreased.AddListener(Render);
            _currentTurnText.text = "1";
        }

        private void Render(int currentTurnNumber)
        {
            _currentTurnText.text = currentTurnNumber.ToString();
        }
    }
}
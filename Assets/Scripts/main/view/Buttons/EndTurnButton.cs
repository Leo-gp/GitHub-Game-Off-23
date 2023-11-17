using main.service.Turn_System;
using UnityEngine;

namespace main.view
{
    public class EndTurnButton : MonoBehaviour
    {
        public void OnClick()
        {
            GameService.Instance.EndTurn();
            GameService.Instance.StartTurn();
        }
    }
}
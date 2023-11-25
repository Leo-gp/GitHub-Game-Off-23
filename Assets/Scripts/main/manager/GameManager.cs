using main.service.Turn_System;
using UnityEngine;

namespace main.manager
{
    public class GameManager : MonoBehaviour
    {
        private void Awake()
        {
            // We are using Awake for setting up the game!
            // Do NOT use awake to try to access entities, services, etc.
            // Use Start to register events
            new GameService();
        }

        private void Start()
        {
            GameService.Instance.StartTurn();
        }
    }
}
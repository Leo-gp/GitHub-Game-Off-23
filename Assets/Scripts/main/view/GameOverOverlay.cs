using main.service.Turn_System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.view
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] private GameObject _container;

        private void Awake()
        {
            _container.SetActive(false);
        }

        private void Start()
        {
            GameService.Instance.OnGameOver.AddListener(Render);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene("Main");
        }

        public void ReturnToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

        private void Render()
        {
            _container.SetActive(true);
        }
    }
}
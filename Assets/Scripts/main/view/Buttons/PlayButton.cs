using UnityEngine;
using UnityEngine.SceneManagement;

namespace main.view.Buttons
{
    public class PlayButtonView : MonoBehaviour
    {
        public void OnClick()
        {
            SceneManager.LoadScene("Main");
        }
    }
}
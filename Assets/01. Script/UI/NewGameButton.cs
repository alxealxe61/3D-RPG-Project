using _01._Script.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _01._Script.UI
{
    public class NewGameButton : MonoBehaviour
    {
        
        public void OnNewGameClicked()
        {
            DataManager.Instance.CreateNewGame();
            SceneManager.LoadScene("00. Scenes/01.Village");
        }
    }
}
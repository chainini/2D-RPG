using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Options : MonoBehaviour
{
    public void SaveAndQuit()
    {
        SaveManager.instance.SaveGame();
        SceneManager.LoadScene(0,LoadSceneMode.Single);
    }
}

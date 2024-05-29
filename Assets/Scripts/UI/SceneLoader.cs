using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public Pause pause = null;


    public void LoadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadScene(string sceneName)
    {
        if (pause != null)
        {
            pause.TogglePaused();
        }
        SceneManager.LoadScene(sceneName);
    }
}

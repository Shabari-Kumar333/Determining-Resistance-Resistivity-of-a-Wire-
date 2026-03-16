using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [Header("Scene Settings")]
    [Tooltip("Scene name OR build index")]
    public string sceneName;

    public int sceneBuildIndex = -1;

    [Header("Options")]
    public bool useSceneName = true;
    public bool loadAdditive = false;

    // 🔘 Call from Button / UnityEvent
    public void LoadScene()
    {
        if (useSceneName && !string.IsNullOrEmpty(sceneName))
        {
            LoadByName(sceneName);
        }
        else if (sceneBuildIndex >= 0)
        {
            LoadByIndex(sceneBuildIndex);
        }
        else
        {
            Debug.LogError("SceneLoader: No valid scene set!");
        }
    }

    public void LoadByName(string name)
    {
        if (loadAdditive)
            SceneManager.LoadScene(name, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(name, LoadSceneMode.Single);
    }

    public void LoadByIndex(int index)
    {
        if (loadAdditive)
            SceneManager.LoadScene(index, LoadSceneMode.Additive);
        else
            SceneManager.LoadScene(index, LoadSceneMode.Single);
    }

    // 🔙 Optional back / restart helpers
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        int nextIndex = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextIndex);
    }

    public void LoadPreviousScene()
    {
        int prevIndex = SceneManager.GetActiveScene().buildIndex - 1;
        if (prevIndex >= 0)
            SceneManager.LoadScene(prevIndex);
    }

    // 🚪 Quit application
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Application Quit (Editor only log)");
    }
}

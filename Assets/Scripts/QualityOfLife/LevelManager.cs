using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    public void GoToNextLevel()
    {
        int buildIndex = GetActiveSceneIndex();

        if (!GetSceneIsValid(buildIndex + 1))
        { Debug.LogError("Scene by build index " + (buildIndex + 1) + " can not be found."); return; }

        SceneManager.LoadScene(buildIndex + 1);
    }
    public void GoToPreviousLevel()
    {
        int buildIndex = GetActiveSceneIndex();

        if (!GetSceneIsValid(buildIndex - 1))
        { Debug.LogError("Scene by build index " + (buildIndex - 1) + " can not be found."); return; }

        SceneManager.LoadScene(buildIndex - 1);
    }
    public void GoToLevel(int index)
    {
        if (!GetSceneIsValid(index))
        { Debug.LogError("Scene by build index " + index + " can not be found."); return; }

        SceneManager.LoadScene(index);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public bool GetSceneIsValid(int buildIndex) => -1 != SceneUtility.GetBuildIndexByScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    public bool GetSceneIsValid(string scenePath) => -1 != SceneUtility.GetBuildIndexByScenePath(scenePath);
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoSingleton<LevelManager>
{
    public bool GoToNextLevel()
    {
        int buildIndex = GetActiveSceneIndex();

        if (!GetSceneIsValid(buildIndex + 1))
        { 
            Debug.LogError("Scene by build index " + (buildIndex + 1) + " can not be found.");
            return false; 
        }

        SceneManager.LoadScene(buildIndex + 1);
        return true;
    }

    public void GoToNextLevelForEvents()
    {
        int buildIndex = GetActiveSceneIndex();

        if (!GetSceneIsValid(buildIndex + 1))
        {
            Debug.LogError("Scene by build index " + (buildIndex + 1) + " can not be found.");
            return;
        }

        SceneManager.LoadScene(buildIndex + 1);
    }

    public bool GoToPreviousLevel()
    {
        int buildIndex = GetActiveSceneIndex();

        if (!GetSceneIsValid(buildIndex - 1))
        { 
            Debug.LogError("Scene by build index " + (buildIndex - 1) + " can not be found."); 
            return false; 
        }

        SceneManager.LoadScene(buildIndex - 1);
        return true;
    }
    public void GoToPreviousLevelForEvents()
    {
        int buildIndex = GetActiveSceneIndex();

        if (!GetSceneIsValid(buildIndex - 1))
        {
            Debug.LogError("Scene by build index " + (buildIndex - 1) + " can not be found.");
            return;
        }

        SceneManager.LoadScene(buildIndex - 1);
    }
    public bool GoToLevel(int index)
    {
        if (!GetSceneIsValid(index))
        { 
            Debug.LogError("Scene by build index " + index + " can not be found.");
            return false;
        }

        SceneManager.LoadScene(index);
        return true;
    }
    public bool GoToLevel(int index, bool GoToZeroIfFailed)
    {
        if (!GetSceneIsValid(index))
        {
            Debug.LogError("Scene by build index " + index + " can not be found.");

            if (GoToZeroIfFailed) GoToLevel(0, false);

            return false;
        }

        SceneManager.LoadScene(index);
        return true;
    }
    public void GoToLevelZero()
    {
        SceneManager.LoadScene(0);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToLevelAfterSeconds(float seconds, int buildIndex, bool GoToZeroIfFailed)
    {
        if (levelIEnum != null) return;

        levelIEnum = GoAfterSeconds(seconds, buildIndex, GoToZeroIfFailed);
        StartCoroutine(levelIEnum);
    }
    public void CancelLevelChange()
    {
        if (levelIEnum == null) return;

        StopCoroutine(levelIEnum);
        levelIEnum = null;
    }
    IEnumerator levelIEnum;
    IEnumerator GoAfterSeconds(float seconds, int buildIndex, bool GoToZeroIfFailed)
    {
        yield return new WaitForSeconds(seconds);
        GoToLevel(buildIndex, GoToZeroIfFailed);
    }

    public int GetActiveSceneIndex() => SceneManager.GetActiveScene().buildIndex;
    public bool GetSceneIsValid(int buildIndex) => -1 != SceneUtility.GetBuildIndexByScenePath(SceneUtility.GetScenePathByBuildIndex(buildIndex));
    public bool GetSceneIsValid(string scenePath) => -1 != SceneUtility.GetBuildIndexByScenePath(scenePath);
}

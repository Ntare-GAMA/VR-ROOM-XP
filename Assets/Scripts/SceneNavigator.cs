using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Moves between the app's scenes. Fades to black first when the scene has a Fade Canvas,
/// so the player never sees a hard cut. Call LoadScene from a UI Button's On Click event.
/// </summary>
public class SceneNavigator : MonoBehaviour
{
    [Tooltip("How long to wait for the fade to black before loading the next scene")]
    public float fadeDuration = 0.3f;

    private bool isLoading = false;

    public void LoadScene(string sceneName)
    {
        if (!isLoading)
            StartCoroutine(FadeAndLoad(sceneName));
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private IEnumerator FadeAndLoad(string sceneName)
    {
        isLoading = true;

        FadeCanvas fade = FindFirstObjectByType<FadeCanvas>();
        if (fade != null)
        {
            fade.QuickFadeIn();
            yield return new WaitForSeconds(fadeDuration);
        }

        // Each scene has its own XR Origin, so input and interactions start fresh after the load.
        yield return SceneManager.LoadSceneAsync(sceneName);
    }
}

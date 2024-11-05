using UnityEngine;

public class ExitGameScript : MonoBehaviour
{
    // This function will be called when the player clicks the exit button
    public void ExitGame()
    {
        // Check if we are in the Unity Editor
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application if in a build
        Application.Quit();
#endif
    }
}

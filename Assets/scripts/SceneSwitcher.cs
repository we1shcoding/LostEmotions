using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void LoadRegisterScene()
    {
        SceneManager.LoadScene("RegisterScene");
    }

    public void LoadLoginScene()
    {
        SceneManager.LoadScene("LoginScene");
    }

    public void OnExitButtonClick()
    {
        Application.Quit();
        Debug.Log("꺼져");
    }

}

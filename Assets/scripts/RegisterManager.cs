using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class RegisterManager : MonoBehaviour
{
    public TMP_InputField usernameInputField;
    public TMP_InputField emailInputField;
    public TMP_InputField userIdInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField confirmPasswordInputField;
    public TMP_Text resultText;
    public TMP_Text passwordMismatchText;

    private string registerUrl = "http://localhost:3000/register";

    public void OnRegisterButtonClick()
    {
        if (passwordInputField.text != confirmPasswordInputField.text)
        {
            passwordMismatchText.text = "passwordNoNO";
            return;
        }

        StartCoroutine(Register(usernameInputField.text, emailInputField.text, userIdInputField.text, passwordInputField.text));
    }

    private IEnumerator Register(string username, string email, string userId, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", username);
        form.AddField("email", email);
        form.AddField("userid", userId);
        form.AddField("password", password);

        UnityWebRequest www = UnityWebRequest.Post(registerUrl, form);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success && www.downloadHandler.text == "User registered successfully")
        {
            resultText.text = "OKOK";
            yield return new WaitForSeconds(2);
            SceneManager.LoadScene("LoginScene");
        }
        else
        {
            resultText.text = "NONO";
        }
    }
}

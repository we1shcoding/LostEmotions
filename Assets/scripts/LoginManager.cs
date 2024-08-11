using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField userIdInputField;
    public TMP_InputField passwordInputField;
    public TMP_Text resultText;

    private string loginUrl = "http://127.0.0.1:3000/login"; // localhost 대신 127.0.0.1 사용

    public void OnLoginButtonClick()
    {
        if (string.IsNullOrEmpty(userIdInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            resultText.text = "Please enter both user ID and password.";
            return;
        }

        StartCoroutine(Login(userIdInputField.text, passwordInputField.text));
    }

    private IEnumerator Login(string userId, string password)
    {
        // JSON 형식으로 데이터 준비
        string json = JsonUtility.ToJson(new LoginRequest { userid = userId, password = password });

        // UnityWebRequest를 사용하여 POST 요청 생성
        UnityWebRequest request = new UnityWebRequest(loginUrl, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json"); // JSON 형식으로 요청 헤더 설정

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            if (request.downloadHandler.text == "Login successful")
            {
                SceneManager.LoadScene("SampleScene");
            }
            else
            {
                resultText.text = "Login failed: " + request.downloadHandler.text;
            }
        }
        else
        {
            resultText.text = "Login failed: " + request.error;
        }
    }

    [System.Serializable]
    public class LoginRequest
    {
        public string userid;
        public string password;
    }
}

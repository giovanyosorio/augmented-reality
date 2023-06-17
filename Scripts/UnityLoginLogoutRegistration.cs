using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class UnityLoginLogoutRegistration : MonoBehaviour
{

    public string baseUrl = "https://halyfax.com/game/";
    public static string username;

    public   InputField accountUserName;
    public  InputField accountPassword;
    public InputField accountEmail;

    public Text info;
    public Text info2;

    private bool useMount; // are we in range of the mount?

    public static  string currentUsername;
    private string ukey = "accountusername";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("get string"+PlayerPrefs.GetString(ukey));
        Debug.Log("hashkey"+PlayerPrefs.HasKey(ukey));

        currentUsername = "";
        if (PlayerPrefs.HasKey(ukey))
        {
            if (PlayerPrefs.GetString(ukey) != "")
            {
                  SceneManager.LoadScene("Home");
                currentUsername = PlayerPrefs.GetString(ukey);
             //   info.text = "Welcome  " + currentUsername;
              //  SceneManager.LoadScene("Home");

            }
            else 
            {
                PlayerPrefs.DeleteKey(ukey);
              //  info.text = "You are not loged in.";
            }
        }
        else
        {
          //  info.text = "You are not loged in.";
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AccountLogout()
    {
        currentUsername = "";
        PlayerPrefs.DeleteAll();
        // info.text = "You are just loged out.";
        SceneManager.LoadSceneAsync("AccountLogin");
    }

    public void AccountRegister()
    {
        string uName = accountUserName.text;
        string pWord = accountPassword.text;
       // string eMail = accountEmail.text;

        StartCoroutine(RegisterNewAccount(uName, pWord));
    }

    public void AccountLogin()
    {
       

            string uName = accountUserName.text;
            string pWord = accountPassword.text;
            StartCoroutine(LogInAccount(uName, pWord));
        

    }
    public void ForgotPassword()
    {
        string uName = accountUserName.text;
        string pWord = accountPassword.text;
     //   string eMail = accountPassword.text;
        StartCoroutine(ForgotPassword(uName, pWord));


    }
    IEnumerator RegisterNewAccount(string uName, string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("newAccountUsername", uName);
        form.AddField("newAccountPassword", pWord);
      //  form.AddField("newAccountEmail", eMail);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
                Debug.Log("Response = " + responseText);
                info.text = responseText;
            }
        }
    }



    IEnumerator LogInAccount(string uName, string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUsername", uName);
        form.AddField("loginPassword", pWord);
        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else 
            {
              
                string responseText = www.downloadHandler.text;
                info.text = "Response = " + responseText;
               Debug.Log(responseText.GetType());
                Debug.Log(responseText == "true");
                Debug.Log(responseText);
                Debug.Log(responseText.TrimStart().TrimEnd() == "true");
                if (responseText.TrimStart().TrimEnd() == "true")
                { PlayerPrefs.SetString(ukey, uName);
                    //Start();
                   SceneManager.LoadScene("Home");
                }
                else if (responseText.TrimStart().TrimEnd() == "false")
                {
                    info.text = "Usuario y/o contraseña incorrecta";

                }
                else if (responseText.TrimStart().TrimEnd() == "asd")
                {
                    info.text = "Ambos campos son requeridos ";

                }

            }
        }
    }

    IEnumerator ForgotPassword(string uName,string pWord)
    {
        WWWForm form = new WWWForm();
        form.AddField("newUser", uName);
      //  form.AddField("newEmail", uEmail);
        form.AddField("newPasword", pWord);

        using (UnityWebRequest www = UnityWebRequest.Post(baseUrl, form))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.isNetworkError)
            {
                Debug.Log(www.error);
            }
            else
            {
                string responseText = www.downloadHandler.text;
               // info2.text = "Response = " + responseText;
       
                if (responseText.TrimStart().TrimEnd() == "true")
                {
                    info2.text = "Contraseña cambiada " ;

                }

                else if (responseText.TrimStart().TrimEnd() == "false")
                {
                    info2.text = "Usuario no encontrado ";

                }
                else if (responseText.TrimStart().TrimEnd() == "asd")
                {
                    info2.text = "Ambos campos son requeridos ";

                }

            }

        }
    }




    public void CambiarDeEscena(int n)
    {
        SceneManager.LoadScene(n);
    }

}

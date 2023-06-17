using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NetWork : MonoBehaviour
{
    // Start is called before the first frame update
public void CreateUser(string username, string email, string password, Action<Response> response)
    {
        StartCoroutine(CO_CreateUSer(username, email, password, response));
    }

    private IEnumerator CO_CreateUSer(string username, string email, string password, Action<Response> response)
    {
        WWWForm form= new WWWForm();
        form.AddField("username",username);
        form.AddField("email",email);
        form.AddField("password",password);
      //  form.AddField("password2", password2);
        WWW w = new WWW("https://halyfax.com/game/createuser.php", form);

        yield return w;


       response(JsonUtility.FromJson<Response>(w.text));
    }
}


public class Response
{
    public bool done = false;
    public string message = "";
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
public class LoginNew : MonoBehaviour
{
    [SerializeField] private InputField m_usernameInput = null;
    [SerializeField] private Text m_text = null;

    [SerializeField] private InputField m_password = null;
    [SerializeField] private InputField m_reEnterPassword = null;

    [SerializeField] private InputField m_emailInput = null;
    // Start is called before the first frame update
    [SerializeField] private GameObject m_registerUI = null;
    [SerializeField] private GameObject m_loginUI = null;

    //private NetWorkManager m_networkManager = null;

    private NetWork m_networkManager = null;
    
    private void Awake()
    {
        m_networkManager = GameObject.FindObjectOfType<NetWork>();   
    }
    public void SubmitRegister()
    {
        if(m_usernameInput.text == "" || m_emailInput.text == "" || m_password.text == "" || m_reEnterPassword.text == "")
        {
            m_text.text = "Por favor llena todos los campos";
            return;
        }
        if(m_password.text == m_reEnterPassword.text)
        {
            m_text.text = "Procesado...";

            m_networkManager.CreateUser(m_usernameInput.text, m_emailInput.text, m_password.text, delegate ( Response response )
               {
                   m_text.text = response.message;
               });
        }else
        {
            m_text.text = "contraseñas no son iguales";
        }
    }
    public void ShowLogin()
    {
        m_registerUI.SetActive(false);
        m_loginUI.SetActive(true);
    }

    public void ShowRegister()
    {
        m_registerUI.SetActive(true);
        m_loginUI.SetActive(false);
    }
}











































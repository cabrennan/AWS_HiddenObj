using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using System;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using System.IO;


public class Cognito : MonoBehaviour
{

    public Button SignupButton;
    public Button SignInButton;
    public InputField EmailField;
    public InputField PasswordField;
    bool signInSuccessful;

    static Amazon.RegionEndpoint Region = Amazon.RegionEndpoint.USEast1;
    private Hashtable CognitoInfo;

    private void GetAWSCognitoInfo()
    {
        //local path of Cognito ID info
        string path = "C:/Users/Christine/CognitoInfo.txt";
       
        try {
            if (File.Exists(path))
            {
                CognitoInfo = new Hashtable();
                using (StreamReader sr = new StreamReader(path))
                {
                    while (sr.Peek() >= 0)
                    {
                        string str = sr.ReadLine();
                        if(str.Contains(":"))
                        {
                            string[] kv = str.Split(new string[] { ":" }, 2, StringSplitOptions.None);
                            CognitoInfo[kv[0]] = kv[1];
                        } 
                    }
                }
            }
        }
        catch(Exception e) 
        {
            Debug.LogError(e.ToString());         
        }

        // For debugging
        //foreach (string key in CognitoInfo.Keys)
        //{
        //    Debug.Log(String.Format("{0}: {1}", key, CognitoInfo[key]));
        //}

    }




    public void SignIn()
    {
        SignInButton.onClick.AddListener(on_signin_click);
        signInSuccessful = false;
    }

    public void Register()
    {
        SignupButton.onClick.AddListener(on_signup_click);
    }


    void Update()
    {
        if (signInSuccessful)
            SceneManager.LoadScene(1);
    }

    public void on_signup_click()
    {
        Debug.Log("sign up button clicked");
        _ = SignUpMethodAsync();
    }

    public void on_signin_click()
    {
        Debug.Log("sign in button clicked");
        _ = SignInUser();
    }

    //Method that creates a new Cognito user
    private async Task SignUpMethodAsync()
    {
        string password = PasswordField.text;
        string email = EmailField.text;
        string userName = email;

        GetAWSCognitoInfo();

        AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(
             new Amazon.Runtime.AnonymousAWSCredentials(), Region);

        SignUpRequest signUpRequest = new SignUpRequest()
        {
            ClientId = (string)CognitoInfo["AppClientID"],
            Username = userName,
            Password = password
        };

        List<AttributeType> attributes = new List<AttributeType>()
        {
            new AttributeType(){Name = "email", Value = email}
        };

        signUpRequest.UserAttributes = attributes;

        try
        {
            SignUpResponse request = await provider.SignUpAsync(signUpRequest);

            Debug.Log("Sign up worked");
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            return;
        }

    }

    //Method that signs in Cognito user 
    private async Task SignInUser()
    {
        string password = PasswordField.text;
        string email = EmailField.text;
        string userName = email;
       
        GetAWSCognitoInfo();


        try
        {
            AmazonCognitoIdentityProviderClient provider = new AmazonCognitoIdentityProviderClient(
                new Amazon.Runtime.AnonymousAWSCredentials(), Region);

            CognitoUserPool userPool = new CognitoUserPool((string)CognitoInfo["PoolID"], (string)CognitoInfo["AppClientID"], provider);
            CognitoUser user = new CognitoUser(email, (string)CognitoInfo["AppClientID"], userPool, provider);

            InitiateSrpAuthRequest authRequest = new InitiateSrpAuthRequest()
            {
                Password = password
            };


            AuthFlowResponse authResponse = await user.StartWithSrpAuthAsync(authRequest).ConfigureAwait(false);
            GetUserRequest getUserRequest = new GetUserRequest();
            getUserRequest.AccessToken = authResponse.AuthenticationResult.AccessToken;

            Debug.Log("User Access Token: " + getUserRequest.AccessToken);
            signInSuccessful = true;
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e);
            return;
        }
    }
}
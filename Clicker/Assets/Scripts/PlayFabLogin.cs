using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
public class PlayFabLogin : MonoBehaviour
{
    private string userEmail;
    private string userPassword;
    private string userName;
    public Text TextOut;
    public GameObject LoginPanel,ResultPanel;
    public void Start()
    {
        if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        {
            /*
            Please change the titleId below to your own titleId from PlayFab Game Manager.
            If you have already set the value in the Editor Extensions, this can be skipped.
            */
            PlayFabSettings.staticSettings.TitleId = "B6CD9";
        }
        if (PlayerPrefs.HasKey("EMAIL")) 
        {
            userName = PlayerPrefs.GetString("EMAIL");
            userPassword = PlayerPrefs.GetString("PASSWORD");
            var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
            PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
            LoginPanel.SetActive(false);
        }

    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        // PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName})
        LoginPanel.SetActive(false);
        TextOut.text = "Lets Click";
    }
    /*
    void SetUserData()
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>() {
            {MainIteraction.RankName,MainIteraction.RankPoints}
        }
        },
        result => Debug.Log("Successfully updated user data"),
        error => {
            Debug.Log("Got error setting user data Ancestor to Arthur");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    */
    void GetUserData(string myPlayFabeId)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myPlayFabeId,
            Keys = null
        }, result => {
            Debug.Log("Got user data:");
            if (result.Data == null || !result.Data.ContainsKey("Ancestor")) Debug.Log("No Ancestor");
            else Debug.Log("Ancestor: " + result.Data["Ancestor"].Value);
        }, (error) => {
            Debug.Log("Got error retrieving user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }
    private void OnRegisterSucess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        Debug.Log("CONGRATS");
        TextOut.text = "Verifique seus dados";
    }
    private void OnLoginFailure(PlayFabError error)
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSucess, OnRegisterFailure);
        TextOut.text = "Verifique seus dados";
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
    public void GetUserEmail(string emailLogin)
    {
        userEmail = emailLogin;
    }

    public void GetUserPassword(string passwordLogin)
    {
        userPassword = passwordLogin;
    }

    public void GetUsername(string usernameLogin)
    {
        userName = usernameLogin;
    }

    public void LoginClick()
    {
        var request = new LoginWithEmailAddressRequest { Email = userEmail, Password = userPassword };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnLoginFailure);
        ResultPanel.SetActive(true);
    }

    
    #region Leaderboard
    public void GetLeaderBoard()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StatisticName = "Ranking", MaxResultsCount = 100000 };
        //PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeadboard, OnErrorLeadBoard);
    }

    void OnGetLeadboard(GetLeaderboardRequest result)
    {
        foreach (PlayerLeaderboardEntry player in result.Leaderboard)
        {
            Debug.Log(player.DisplayName + ": " + player.StatValue);
        }


    }

    void OnErrorLeadBoard(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
    #endregion
    
}
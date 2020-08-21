using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

public class PlayFabController : MonoBehaviour
{
    private string userEmail;
    private string userPassword;
    private string userName;
    public Text TextOut;
    public GameObject LoginPanel, ResultPanel;
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
            //LoginPanel.SetActive(false);
        }
        else
        {
#if UNITY_ANDROID
            var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
#if UNITY_IOS
            var requestIOS = new LoginWithIOSDeviceIDRequest { DeviceId = ReturnMobileID(), CreateAccount = true };
            PlayFabClientAPI.LoginWithIOSDeviceID(requestIOS, OnLoginMobileSuccess, OnLoginMobileFailure);
#endif
        }

    }
    /* LOGINS EM GERAL */
    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        // PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName})
        LoginPanel.SetActive(false);
        GetStatistics();
        TextOut.text = "Lets Click";
    }
    private void OnLoginMobileSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
        // PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName})
        LoginPanel.SetActive(false);
        GetStatistics();
        TextOut.text = "Lets Click";
    }

    private void OnLoginFailure(PlayFabError error)
    {
        var registerRequest = new RegisterPlayFabUserRequest { Email = userEmail, Password = userPassword, Username = userName };
        PlayFabClientAPI.RegisterPlayFabUser(registerRequest, OnRegisterSucess, OnRegisterFailure);
        TextOut.text = "Verifique seus dados";
    }

    private void OnLoginMobileFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
        TextOut.text = "Verifique seus dados";
    }
    private void OnRegisterSucess(RegisterPlayFabUserResult result)
    {
        PlayerPrefs.SetString("EMAIL", userEmail);
        PlayerPrefs.SetString("PASSWORD", userPassword);
        Debug.Log("CONGRATS");
        TextOut.text = "Lets Click";
        LoginPanel.SetActive(false);
        GetStatistics();
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest { DisplayName = userName }, OnDisplayName, OnLoginFailure);
    }
    
    void OnDisplayName(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log(result.DisplayName + "is your name");
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
    public static String ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }


    /* ESTATISTICAS DO PLAYER */

 
    public static void SetStats()
    {
        PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
        {
            // request.Statistics is a list, so multiple StatisticUpdate objects can be defined if required.
            Statistics = new List<StatisticUpdate> {
        new StatisticUpdate { StatisticName = "Points", Value = MainIteraction.Points },
    }
        },
result => { Debug.Log("User statistics updated"); },
error => { Debug.LogError(error.GenerateErrorReport()); });
    }

    void GetStatistics()
    {
        PlayFabClientAPI.GetPlayerStatistics(
            new GetPlayerStatisticsRequest(),
            OnGetStatistics,
            error => Debug.LogError(error.GenerateErrorReport())
        );
    }
    void OnGetStatistics(GetPlayerStatisticsResult result)
    {
        Debug.Log("Received the following Statistics:");
        foreach (var eachStat in result.Statistics)
        {
            switch (eachStat.StatisticName)
            {
                case "Points":
                    MainIteraction.Points = eachStat.Value;
                    break;
            }
            Debug.Log("Statistic (" + eachStat.StatisticName + "): " + eachStat.Value);
        }
    }

    /* ESTATISTIC
     * AS DO RANKING */
    public void GetLeaderboarder()
    {
        var requestLeaderboard = new GetLeaderboardRequest { StatisticName = "Points", MaxResultsCount = 100 };
        PlayFabClientAPI.GetLeaderboard(requestLeaderboard, OnGetLeadboard, OnErrorLeaderboard);
    }

    public GameObject ListingPrefab;
    public Transform listingContainer;

    void OnGetLeadboard(GetLeaderboardResult result)
    {
        //Debug.Log(result.Leaderboard[0].StatValue);
        foreach(PlayerLeaderboardEntry player in result.Leaderboard)
        {
            Instantiate(ListingPrefab, listingContainer);
            GameObject tempListing = Instantiate(ListingPrefab, listingContainer);
            LeaderBoardListing LL = tempListing.GetComponent<LeaderBoardListing>();
            LL.PlayerNameText.text = player.DisplayName;
            LL.PlayerPointsText.text = player.StatValue.ToString();
            Debug.Log(player.DisplayName + " " + player.StatValue);
        }
    }
    void OnErrorLeaderboard(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }
}
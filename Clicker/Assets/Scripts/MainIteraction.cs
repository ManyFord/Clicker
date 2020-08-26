using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MainIteraction : MonoBehaviour
{
    public GameObject NoClick, Clicked, Menu, CreateItem,RegisterNamePanel;
    public GameObject InputFieldName;
   // public static Text pointsText, nameText, RankPositionText;
    public static string RankName, RankPoints;
    public static int Points;
    // Start is called before the first frame update
    void Start()
    {
        NoClick.SetActive(true);
        Clicked.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //pointsText.text = "Pontos: " + Points;
        //RankPoints = pointsText.text;
        //nameText.text = RankName;

    }

    public void OnClick()
    {
        GetComponent<AudioSource>().Play();
        Clicked.SetActive(true);
        NoClick.SetActive(false);
        Points++;
        StartCoroutine("TimeClick");
        PlayFabController.SetStats();
    }

    public void Animation()
    {
       //Ideias de animação
    }

    IEnumerator ParticleAnimation()
    {
        Animation();
        yield return new WaitForSeconds(.1f);
    }

    IEnumerator TimeClick()
    {
        yield return new WaitForSeconds(.2f);
        Clicked.SetActive(false);
        NoClick.SetActive(true);
    }

    public void Scale()
    {

    }
    public void LetsClick()
    {
        RankName = InputFieldName.GetComponent<Text>().text;
        if (RankName == "")
        {
            RegisterNamePanel.SetActive(true);
        }
        Debug.Log("Sucess" + RankName);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class MainIteraction : MonoBehaviour
{
    public GameObject NoClick,Clicked;
    public Text pointsText;
    public int Points;
    // Start is called before the first frame update
    void Start()
    {
        NoClick.SetActive(true);
        Clicked.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        pointsText.text = "Pontos: " + Points;
    }

    public void OnClick()
    {
        GetComponent<AudioSource>().Play();
        Clicked.SetActive(true);
        NoClick.SetActive(false);
        Points++;
        StartCoroutine("TimeClick");
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
        yield return new WaitForSeconds(.3f);
        Clicked.SetActive(false);
        NoClick.SetActive(true);
    }
}

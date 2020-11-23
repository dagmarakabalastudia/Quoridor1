using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Specialized;
using TMPro;


public class menuStart : MonoBehaviour
{
    public GameObject twoPlayers;
    public GameObject fourPlayers;
    public GameObject main;
    public GameObject HowToPlay;
    public GameObject[] inputs;
    public int iii = 0;
    public names names;
    void Start()
    {
        
    }
    void Awake()
    {
        names = GameObject.FindObjectOfType<names>();
    }
    void Update()
    {
    }
    public void onClickToTwoPlayers()
    {
        twoPlayers.SetActive(true);
        main.SetActive(false);
    }
    public void onClickToFourPlayers()
    {
        fourPlayers.SetActive(true);
        main.SetActive(false);
    }
    public void BackToMainMenu()
    {
        fourPlayers.SetActive(false);
        twoPlayers.SetActive(false);
        HowToPlay.SetActive(false);
        main.SetActive(true);
    }
    public void GoToPlay()
    {
        inputs = GameObject.FindGameObjectsWithTag("name");
        names.namesTable = new string[inputs.Length];
        foreach (GameObject input in inputs)
        {
            names.namesTable[iii] = input.GetComponent<TMP_InputField>().text;
            iii++;
        }
        names.howManyPlayers = inputs.Length;
        Application.LoadLevel("game");
    }
    public void exit()
    {
        Application.Quit();
    }
    public void HowPlay()
    {
        main.SetActive(false);
        HowToPlay.SetActive(true);

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class menuEnd : MonoBehaviour
{
    public names names;
    public TMP_Text win;

    void Start()
    {
        
    }
    void Awake()
    {
        names = GameObject.FindObjectOfType<names>();
        win.text = names.winner;
    }

    void Update()
    {
        
    }
    public void exit()
    {
        Application.Quit();

    }
    public void playAgain()
    {
        Application.LoadLevel("start");

    }
}

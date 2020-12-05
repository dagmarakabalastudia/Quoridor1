using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using UnityEngine;
using TMPro;

public class moving : MonoBehaviour
{
    Transform player;
    public GameObject camer;
    public GameObject select;
    public GameObject select2;
    public GameObject select3;
    public GameObject select4;
    public GameObject select5;
    public GameObject[] players;
    public GameObject[] blocksX;
    public GameObject[] blocksZ;
    public GameObject blockX;
    public GameObject blockZ;
    public TMP_Text title;
    public TMP_Text whatToDoText;
    public TMP_Text blocksCount;
    public GameObject here;
    int[] playersBlock = { 0, 0, 0, 0 };
    int blockForPlayer;
    float y;
    public Material disabledButton;
    public Material button;
    int i = 0;
    int what = 0;
    int m = 0;
    int activePlane;
    public GameObject[] allObjects;
    GameObject[] allBlocks;
    GameObject[] buttonBlock;
    GameObject[] changeBlock = new GameObject[1];
    GameObject[] moveBlock;
    GameObject[,] gameBoard = new GameObject[17, 17];
    public names names;
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;
    public GameObject pause;
    public GameObject info;
    public TMP_Text changes;
    public GameObject changesCube;
    List<int> resultsPlayer0 = new List<int>();
    List<int> resultsPlayer1 = new List<int>();
    List<int> resultsPlayer2 = new List<int>();
    List<int> resultsPlayer3 = new List<int>();
    List<int> allResults = new List<int>();
    GameObject[,] helperGameBoard = new GameObject[19, 19];
    void Start()
    {
        createPlayers();
        buttonBlock = GameObject.FindGameObjectsWithTag("createBlock");
        moveBlock = GameObject.FindGameObjectsWithTag("move");
        y = select.transform.position.y;
        players = GameObject.FindGameObjectsWithTag("Player");
        blockForPlayer = 20/players.Length;
    }
    void Awake()
    {
        names = GameObject.FindObjectOfType<names>();
    }
    void Update()
    {
       findBlocks();
       setPlaneTable();
       setPlayer();  
       ifWin();
       checkWhatToDo();
       remainingBlocks();
       openPause();
       changeMind();
    } 
    void checkWhatToDo()
    {
        if (what == 0)
        {
            whatToDo();
            whatToDoText.text = "Co chcesz zrobić?";
            changes.gameObject.SetActive(false);
            changesCube.SetActive(false);

        }
        else if (what == 1)
        {
            movePlayers();
            whatToDoText.text = "Wybierz miejsce ruchu na planszy";
            if (playersBlock[i] != blockForPlayer)
            {
                changes.gameObject.SetActive(true);
                changesCube.SetActive(true);
            }
            changeBlock = GameObject.FindGameObjectsWithTag("change");
            buttonBlock[0].GetComponent<MeshRenderer>().material = disabledButton;
            moveBlock[0].GetComponent<MeshRenderer>().material = button;

        }
        else
        {
            checkIfRoadExist();
            blockPosition();
            whatToDoText.text = "Wybierz miejsce ściany na planszy";
            changes.gameObject.SetActive(true);
            changesCube.SetActive(true);
            changeBlock = GameObject.FindGameObjectsWithTag("change");
            buttonBlock[0].GetComponent<MeshRenderer>().material = button;
            moveBlock[0].GetComponent<MeshRenderer>().material = disabledButton;
        }
    }
    void setPlaneTable()
    {
        Array.Clear(gameBoard, 0, 289);
        Array.Clear(helperGameBoard, 0, 361);
        for (float xposition = 0; xposition < 17; xposition++)
        {
            for (float zposition = 0; zposition < 17; zposition++)
            {
                foreach (GameObject obiekt in allObjects)
                {
                    if (Math.Round(obiekt.transform.position.x / 5, MidpointRounding.AwayFromZero) == xposition && Math.Round(obiekt.transform.position.z / 5, MidpointRounding.AwayFromZero) == zposition)
                    {
                        gameBoard[(int)xposition, (int)zposition] = obiekt;
                    }
                }
            }
        }
        for (int one = 1; one < 17; one += 2)
        {
            for (int second = 1; second < 17; second += 2)
            {
                if (gameBoard[one, second] != null)
                {
                    if (gameBoard[one, second].tag == "blockZ")
                    {
                        gameBoard[one, second - 1] = gameBoard[one, second];
                        gameBoard[one, second + 1] = gameBoard[one, second];
                    }
                    else if (gameBoard[one, second].tag == "blockX")
                    {
                        gameBoard[one - 1, second] = gameBoard[one, second];
                        gameBoard[one + 1, second] = gameBoard[one, second];
                    }
                }
            }
        }
        for(int first = 0; first < 19; first++)
        {
            for (int second = 0; second < 19; second++)
            {
                if(first == 0 || second == 0 || first == 18 || second == 18)
                {
                    helperGameBoard[first, second] = here;
                }
                else
                {
                    helperGameBoard[first, second] = gameBoard[first - 1, second - 1];
                }
            }
        }
    }
    void createPlayers()
    {
        if(names.howManyPlayers == 2)
        {
            Instantiate(player1, new Vector3(40, 15, 80), Quaternion.identity);
            Instantiate(player2, new Vector3(40, 15, 0), Quaternion.identity);
        }
        else if(names.howManyPlayers == 4)
        {
            Instantiate(player1, new Vector3(40, 15, 80), Quaternion.identity);
            Instantiate(player3, new Vector3(80, 15, 40), Quaternion.identity);
            Instantiate(player2, new Vector3(40, 15, 0), Quaternion.identity);
            Instantiate(player4, new Vector3(0, 15, 40), Quaternion.identity);
        }
    }
    void movePlayer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camer.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "selectPlane")
                {
                    player.position = new Vector3(hit.transform.position.x, 10, hit.transform.position.z);
                    i++;
                    if (i > players.Length - 1)
                    {
                        i = 0; 
                    }
                    what = 0;
                    select.SetActive(false);
                    select2.SetActive(false);
                    select3.SetActive(false);
                    select4.SetActive(false);
                    select5.SetActive(false);
                }
            }
        }
    }
    void setSelectedPlane()
    {
        select.SetActive(true);
        select2.SetActive(true);
        select3.SetActive(true);
        select4.SetActive(true);
      
        if (player.position.x - 10 > -1)
        {

            select.transform.position = new Vector3(player.position.x - 10, y, player.position.z);
            if (gameBoard[(int)player.position.x / 5 - 1, (int)player.position.z / 5] != null)
            {
                select.transform.position = new Vector3(0, -1, 0);
            }
            else
            {
                if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5] != null)
                {
                    select.transform.position = new Vector3(0, -1, 0);
                    if(player.position.x - 30 > 0)
                    {
                        if (player.position.z + 10 < 81 && gameBoard[(int)player.position.x / 5 - 3, (int)player.position.z / 5] != null)
                        {
                            select5.SetActive(true);
                            select5.transform.position = new Vector3(player.position.x - 10, y, player.position.z + 10);
                            if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 2] != null || gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 1] != null)
                            {
                                select5.SetActive(false);
                            }
                        }
                    }
                    if (player.position.x - 20 > -1 && player.transform.position.z - 10 > -1)
                    {
                        if (gameBoard[(int)player.position.x / 5 - 3, (int)player.position.z / 5] != null)
                        {
                            if (gameBoard[(int)player.transform.position.x / 5 - 2, (int)player.transform.position.z / 5 - 1] != null)
                            {
                                select.transform.position = new Vector3(0, -1, 0);
                            }
                            else if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 - 1] != null)
                            {
                                if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 2] != null)
                                {
                                    select.transform.position = new Vector3(0, -1, 0);
                                }
                                else
                                {
                                    select.transform.position = new Vector3(player.position.x - 10, y, player.position.z + 10);
                                }
                                if(player.transform.position.x -20 > 0)
                                {
                                    if(gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 1] != null)
                                    {
                                        select.transform.position = new Vector3(0, -1, 0);
                                    }
                                }
                            }
                            else
                            {
                                if (player.transform.position.z - 10 > -1)
                                {
                                    select.transform.position = new Vector3(player.position.x - 10, y, player.position.z - 10);
                                    if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 - 2] != null || gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 - 1] != null)
                                    {
                                        select.transform.position = new Vector3(0, -1, 0);
                                    }
                                }
                                if (player.transform.position.z + 10 < 81)
                                {
                                    select5.SetActive(true);
                                    select5.transform.position = new Vector3(player.position.x - 10, y, player.position.z + 10);
                                    if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 2] != null || gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 1] != null)
                                    {
                                        select5.SetActive(false);
                                    }
                                }
                            }
                        }
                        else if (gameBoard[(int)player.position.x / 5 - 4, (int)player.position.z / 5] != null)
                        {
                            select.transform.position = new Vector3(0, -1, 0);
                        }
                        else
                        {
                            select.transform.position = new Vector3(player.position.x - 20, y, player.position.z);
                        }
                    }
                }
            }         
        }
        else
        {
            select.transform.position = new Vector3(0, -1, 0);
        }
        if (player.position.x + 10 < 90)
        {
            select2.transform.position = new Vector3(player.position.x + 10, y, player.position.z);
            if (gameBoard[(int)player.position.x / 5 + 1, (int)player.position.z / 5] != null)
            {
                select2.transform.position = new Vector3(0, -1, 0);
            }
            else
            {
                if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5] != null)
                {
                    select2.transform.position = new Vector3(0, -1, 0);
                    if (player.position.x + 20 < 90)
                    {
                        if (gameBoard[(int)player.position.x / 5 + 3, (int)player.position.z / 5] != null)
                        {
                            if(player.position.z - 10 > 0 )
                            {
                                if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 - 1] != null)
                                {
                                    select2.transform.position = new Vector3(0, -1, 0);
                                }
                                else
                                {
                                    if (player.position.z - 10 > -1)
                                    {
                                        select2.transform.position = new Vector3(player.position.x + 10, y, player.position.z - 10);
                                        if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 - 2] != null || gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 - 1] ) 
                                        {
                                            select2.transform.position = new Vector3(0, -1, 0);
                                        }
                                    }
                                    
                                }
                                if (player.position.z + 10 < 81)
                                {
                                    select5.SetActive(true);
                                    select5.transform.position = new Vector3(player.position.x + 10, y, player.position.z + 10);
                                    if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 + 2] != null || gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 + 1] != null)
                                    {
                                        select5.SetActive(false);
                                    }
                                }
                            }
                        }
                        else if (gameBoard[(int)player.position.x / 5 + 4, (int)player.position.z / 5] != null)
                        {
                            select2.transform.position = new Vector3(0, -1, 0);
                        }
                        else
                        {
                            select2.transform.position = new Vector3(player.position.x + 20, y, player.position.z);
                        }
                    }
                }
            }
        }
        else
        {
            select2.transform.position = new Vector3(0, -1, 0);
        }
        if (player.position.z - 10 > -1)
        {
            select3.transform.position = new Vector3(player.position.x, y, player.position.z - 10);
            if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 - 1] != null)
            {
                select3.transform.position = new Vector3(0, -1, 0);
            }
            else
            {
                if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 - 2] != null)
                {
                    select3.transform.position = new Vector3(0, -1, 0);
                    if (player.position.z - 20 > -1)
                    {
                        if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 - 3] != null)
                        {
                            if (player.position.x - 10 > 0)
                            {
                                if (gameBoard[(int)player.position.x / 5 - 1, (int)player.position.z / 5 - 2] != null)
                                {
                                    select3.transform.position = new Vector3(0, -1, 0);
                                }
                                else if (gameBoard[(int)player.position.x / 5 + 1, (int)player.position.z / 5 - 2] != null)
                                {
                                    select3.transform.position = new Vector3(0, -1, 0);
                                }
                                select3.transform.position = new Vector3(player.position.x + 10, y, player.position.z - 10);
                                select5.SetActive(true);
                                select5.transform.position = new Vector3(player.position.x - 10, y, player.position.z - 10);
                                if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 - 2] != null || gameBoard[(int)player.position.x / 5 + 1, (int)player.position.z / 5 - 2] != null)
                                {
                                    select3.transform.position = new Vector3(0, -1, 0);
                                }
                                if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 - 2] != null || gameBoard[(int)player.position.x / 5 - 1, (int)player.position.z / 5 - 2] != null)
                                {
                                    select5.SetActive(false);
                                }

                            }
                            if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 - 2] != null)
                            {
                                select3.transform.position = new Vector3(0, -1, 0);
                            }
                        }
                        else if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 - 4] != null)
                        {
                            select3.transform.position = new Vector3(0, -1, 0);
                        }
                        else
                        {
                            select3.transform.position = new Vector3(player.position.x, y, player.position.z - 20);
                        }
                    }
                }
            }
        }
        else
        {
            select3.transform.position = new Vector3(0, -1, 0);
        }
        if (player.position.z + 10 < 81)
        {
            select4.transform.position = new Vector3(player.position.x, y, player.position.z + 10);
            if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 + 1] != null)
            {
                select4.transform.position = new Vector3(0, -1, 0);
            }
            else
            {
                if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 + 2] != null)
                {
                    select4.transform.position = new Vector3(0, -1, 0);
                    if (player.position.z + 20 < 81)
                    {
                        if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 + 3] != null)
                        {
                            select5.SetActive(true);
                            select5.transform.position = new Vector3(player.position.x - 10, y, player.position.z + 10);
                            if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 + 2] != null || gameBoard[(int)player.position.x / 5 - 1, (int)player.position.z / 5 + 2] != null)
                            {
                                select5.SetActive(false);
                            }
                            if (gameBoard[(int)player.transform.position.x / 5 + 2, (int)player.transform.position.z / 5 - 2] != null || gameBoard[(int)player.transform.position.x / 5 + 1, (int)player.transform.position.z / 5 - 2] != null)
                            {
                                select4.transform.position = new Vector3(0, -1, 0);
                            }
                            else
                            {
                                select4.transform.position = new Vector3(player.position.x - 10, y, player.position.z + 10);
                            }
                            if (player.position.z + 10 < 81)
                            {
                                select4.transform.position = new Vector3(player.position.x + 10, y, player.position.z + 10);
                            }
                            if (gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 + 2] != null || gameBoard[(int)player.position.x / 5 + 2, (int)player.position.z / 5 + 1] != null)
                            {
                                select4.transform.position = new Vector3(0, -1, 0);
                            }
                            if (gameBoard[(int)player.position.x / 5 - 2, (int)player.position.z / 5 - 2] != null)
                            {
                                select4.transform.position = new Vector3(0, -1, 0);
                            }
                            if (gameBoard[(int)player.position.x / 5 + 1, (int)player.position.z / 5 + 2] != null)
                            {
                                select4.transform.position = new Vector3(0, -1, 0);
                            }
                        }
                        else if (gameBoard[(int)player.position.x / 5, (int)player.position.z / 5 + 4] != null)
                        {
                            if (select4.transform.position != new Vector3(player.position.x + 10, y, player.position.z + 10))
                            {
                                select4.transform.position = new Vector3(0, -1, 0);
                            }
                        }
                        else
                        {
                            select4.transform.position = new Vector3(player.position.x, y, player.position.z + 20);

                        }
                    }
                        
                }
            }   
        }
        else
        {
            select4.transform.position = new Vector3(0, -1, 0);
        }
        howManyActivePlane();
 
    }
    void findBlocks()
    {
        blocksX = GameObject.FindGameObjectsWithTag("blockX");
        blocksZ = GameObject.FindGameObjectsWithTag("blockZ");
        allBlocks = blocksX.Concat(blocksZ).ToArray();
        allObjects = allBlocks.Concat(players).ToArray();
    }
    void ifWin()
    {
        if (names.howManyPlayers == 2)
        {
            if (players[0].transform.position.z == 0)
            {
                names.winner = names.namesTable[i-1];
                Application.LoadLevel("endGame");
            }
            if(players[1].transform.position.z == 80)
            {
                names.winner = names.namesTable[i+1];
                Application.LoadLevel("endGame");
            }
        }
        else if (names.howManyPlayers == 4)
        {
            if (players[0].transform.position.z == 0)
            {
                names.winner = names.namesTable[i-1];
                Application.LoadLevel("endGame");
            }
            if(players[2].transform.position.z == 80)
            {
                names.winner = names.namesTable[i-1];
                Application.LoadLevel("endGame");
            }
            if (players[1].transform.position.x == 0)
            {
                names.winner = names.namesTable[i-1];
                Application.LoadLevel("endGame");
            }
            if (players[3].transform.position.x == 80)
            {
                names.winner = names.namesTable[i+3];
                Application.LoadLevel("endGame");
            }
        }
    }
    void movePlayers()
    {
        setSelectedPlane();
        movePlayer();
    }
    void whatToDo()
    {
        if(playersBlock[i] == blockForPlayer)
        {
            buttonBlock[0].GetComponent<MeshRenderer>().material = disabledButton;
            if (changeBlock[0] != null)
            {
                if (changeBlock[0].activeSelf == true)
                {
                    changeBlock[0].GetComponent<MeshRenderer>().material = button;

                }

            }
            what = 1;
        }
        else
        {
            buttonBlock[0].GetComponent<MeshRenderer>().material = button;
            if (changeBlock[0] != null)
            {
                if(changeBlock[0].activeSelf == true)
                {
                    changeBlock[0].GetComponent<MeshRenderer>().material = button;

                }
            }
            moveBlock[0].GetComponent<MeshRenderer>().material = button;

            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = camer.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.gameObject.tag == "move")
                    {
                        what = 1;
                    }
                    if (hit.transform.gameObject.tag == "createBlock")
                    {
                        what = 2;                
                    }
                }
            }
        }
    }
    void setPlayer()
    {
        title.text = names.namesTable[i];
        player = players[i].transform;
    }
    void changeMind()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camer.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "change")
                {
                    select.SetActive(false);
                    select2.SetActive(false);
                    select3.SetActive(false);
                    select4.SetActive(false);
                    select5.SetActive(false);
                    here.transform.position = new Vector3(500, y, 500);
                    switch (what)
                    {
                        case 1:
                            {
                                what = 2;
                                break;
                            }

                        case 2:
                            {
                                what = 1;
                                break;
                            }
                    }
                }
            }
        }

    }
    void blockPosition()
    {
        Ray ray = camer.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            m++;
            if (m == 2)
            {
                m = 0;
            }
        }
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.position.x < 75 && hit.transform.position.x > -5 && hit.transform.position.z < 75 && hit.transform.position.z > -5)
            {

                if (m == 0)
                {
                    if (gameBoard[(int)Math.Round(hit.transform.position.x / 5, MidpointRounding.AwayFromZero), (int)Math.Round(hit.transform.position.z / 5 + 1, MidpointRounding.AwayFromZero)] == null && gameBoard[(int)Math.Round(hit.transform.position.x / 5 + 2, MidpointRounding.AwayFromZero), (int)Math.Round(hit.transform.position.z / 5 + 1, MidpointRounding.AwayFromZero)] == null && gameBoard[(int)Math.Round(hit.transform.position.x / 5 +1 , MidpointRounding.AwayFromZero), (int)Math.Round(hit.transform.position.z / 5 + 1, MidpointRounding.AwayFromZero)] == null && (int)Math.Round(hit.transform.position.x / 5 + 1, MidpointRounding.AwayFromZero)%2 == 1 && (int)Math.Round(hit.transform.position.z / 5 + 1, MidpointRounding.AwayFromZero)%2 ==1)
                    {
                        here.transform.position = new Vector3(hit.transform.position.x + 5, y, hit.transform.position.z + 5);
                        here.transform.rotation = Quaternion.Euler(0, 0, 0);
                        helperGameBoard[(int)here.transform.position.x / 5 + 0, (int)here.transform.position.z / 5 + 1] = here;
                        helperGameBoard[(int)here.transform.position.x / 5 + 1, (int)here.transform.position.z / 5 + 1] = here;
                        helperGameBoard[(int)here.transform.position.x/5 + 2, (int)here.transform.position.z /5 + 1] = here;
                        helperGameBoard[(int)here.transform.position.x /5 + 3, (int)here.transform.position.z /5 + 1] = here;
                        updateGameBoardHelper();
                        checkIfRoadExist();
                        checkResultsAStar();
                        if(allResults.Count < players.Length)
                        {
                            here.transform.position = new Vector3(500, y, 500);
                        }
                        else
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                playersBlock[i] += 1;
                                Instantiate(blockX, new Vector3(hit.transform.position.x + 5, 15, hit.transform.position.z + 5), Quaternion.identity);
                                i++;
                                if (i > players.Length - 1)
                                {
                                    i = 0;
                                }
                                what = 0;
                                here.transform.position = new Vector3(500, y, 500);
                            }
                        }
                    }
                    else
                    {
                        here.transform.position = new Vector3(500, y, 500);
                    }
                }
                else
                {
                    if (gameBoard[(int)Math.Round(hit.transform.position.x / 5 + 1, MidpointRounding.AwayFromZero), (int)Math.Round(hit.transform.position.z / 5 + 2, MidpointRounding.AwayFromZero)] == null && gameBoard[(int)Math.Round(hit.transform.position.x / 5 +1, MidpointRounding.AwayFromZero), (int)Math.Round(hit.transform.position.z / 5, MidpointRounding.AwayFromZero)] == null && gameBoard[(int)Math.Round(hit.transform.position.x / 5 + 1, MidpointRounding.AwayFromZero), (int)Math.Round(hit.transform.position.z / 5 + 1, MidpointRounding.AwayFromZero)] == null &&  (int)Math.Round(hit.transform.position.x / 5 + 1, MidpointRounding.AwayFromZero)%2 ==1 && (int)Math.Round(hit.transform.position.z / 5 + 1, MidpointRounding.AwayFromZero)%2 == 1)
                    {
                        here.transform.position = new Vector3(hit.transform.position.x + 5, y, hit.transform.position.z + 5);
                        here.transform.rotation = Quaternion.Euler(0, 90, 0);
                        helperGameBoard[(int)here.transform.position.x / 5 + 1, (int)here.transform.position.z / 5 ] = here;
                        helperGameBoard[(int)here.transform.position.x / 5 + 1, (int)here.transform.position.z / 5 + 2] = here;
                        updateGameBoardHelper();
                        checkIfRoadExist();
                        checkResultsAStar();
                        if (allResults.Count < players.Length)
                        {
                            here.transform.position = new Vector3(500, y, 500);
                        }
                        else
                        {
                            if (Input.GetMouseButtonDown(0))
                            {
                                playersBlock[i] += 1;
                                Instantiate(blockZ, new Vector3(hit.transform.position.x + 5, 15, hit.transform.position.z + 5), Quaternion.Euler(0, 90, 0));
                                i++;
                                if (i > players.Length - 1)
                                {
                                    i = 0;
                                }
                                what = 0;
                                here.transform.position = new Vector3(500, y, 500);
                            }
                        }
                    }
                    else
                    {
                        here.transform.position = new Vector3(500, y, 500);
                    }
                }
            }
        }
    }
    void remainingBlocks()
    {
        string[] blocksPlayers = new string[players.Length];
        int d = 0;
        int reamaining = 0;
        string resultText = "Ilość pozostałych ścian \n" ; 
        foreach(string name in names.namesTable)
        {
            reamaining = blockForPlayer - playersBlock[d];
            blocksPlayers[d] = name + " : " +  reamaining.ToString();
            resultText = resultText + " \n" + blocksPlayers[d];
            d++;
        }
        blocksCount.text = resultText;
    }
    void openPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pause.SetActive(true);
        }
    }
    void updateGameBoardHelper()
    {
        for (int one = 2; one < 17; one += 2)
        {
            for (int second = 2; second < 17; second += 2)
            {
                if (helperGameBoard[one, second - 1] != null)
                {
                    helperGameBoard[one, second] = helperGameBoard[one, second - 1];
                }
                if (helperGameBoard[one, second + 1] != null)
                {
                    helperGameBoard[one, second] = helperGameBoard[one, second + 1];
                }
                if (helperGameBoard[one - 1, second] != null)
                {
                    helperGameBoard[one, second] = helperGameBoard[one - 1, second];
                }
                if (helperGameBoard[one + 1, second] != null)
                {
                    helperGameBoard[one, second] = helperGameBoard[one + 1, second];

                }
            }
        }
    }
    void checkIfRoadExist()
    {
        resultsPlayer0.Clear();
        resultsPlayer1.Clear();
        resultsPlayer2.Clear();
        resultsPlayer3.Clear();
        allResults.Clear();
        if (names.howManyPlayers == 2)
        {
            aStar((int)players[0].transform.position.x/5+1, (int)players[0].transform.position.z/5+1, 1, 1, resultsPlayer0);
            aStar((int)players[0].transform.position.x/5+1, (int)players[0].transform.position.z/5+1, 17, 1, resultsPlayer0);
            aStar((int)players[1].transform.position.x/5+1, (int)players[1].transform.position.z/5+1, 1, 17, resultsPlayer1);
            aStar((int)players[1].transform.position.x/5+1, (int)players[1].transform.position.z/5+1, 17, 17, resultsPlayer1);
            for (int q = 1; q < 17; q = q+2)
            {
                if (gameBoard[q, 0] != null)
                {
                    aStar((int)players[0].transform.position.x/5+1, (int)players[0].transform.position.z/5+1, q+2, 1, resultsPlayer0);
                }
                if (gameBoard[q, 16] != null)
                {
                    aStar((int)players[1].transform.position.x/5+1, (int)players[1].transform.position.z/5+1, q+2, 17, resultsPlayer1);
                }
            }
        }
        else if(names.howManyPlayers == 4)
        {
            aStar((int)players[0].transform.position.x/5+1, (int)players[0].transform.position.z/5+1, 1, 1, resultsPlayer0);
            aStar((int)players[0].transform.position.x/5+1, (int)players[0].transform.position.z/5+1, 17, 1, resultsPlayer0);
            aStar((int)players[1].transform.position.x/5+1, (int)players[1].transform.position.z/5+1, 1, 1, resultsPlayer1);
            aStar((int)players[1].transform.position.x/5+1, (int)players[1].transform.position.z/5+1, 1, 17, resultsPlayer1);
            aStar((int)players[2].transform.position.x/5+1, (int)players[2].transform.position.z/5+1, 1,17, resultsPlayer2);
            aStar((int)players[2].transform.position.x/5+1, (int)players[2].transform.position.z/5+1, 17,17, resultsPlayer2);
            aStar((int)players[3].transform.position.x/5+1, (int)players[3].transform.position.z/5+1, 17, 1, resultsPlayer3);
            aStar((int)players[3].transform.position.x/5+1, (int)players[3].transform.position.z/5+1, 17, 17, resultsPlayer3);

            for (int q = 1; q < 17; q = q+2)
            {
                if (gameBoard[q, 0] != null)
                {
                    aStar((int)players[0].transform.position.x/5+1, (int)players[0].transform.position.z/5+1, q+2, 1, resultsPlayer0);

                }
                if (gameBoard[0, q] != null)
                {
                    aStar((int)players[1].transform.position.x/5+1, (int)players[1].transform.position.z/5+1, 1, q+2, resultsPlayer1);

                }
                if (gameBoard[q, 16] != null)
                {
                    aStar((int)players[2].transform.position.x/5+1, (int)players[2].transform.position.z/5+1, q+2, 17, resultsPlayer2);

                }
                if (gameBoard[16, q] != null)
                {
                    aStar((int)players[3].transform.position.x/5+1, (int)players[3].transform.position.z/5+1, 17, q+2, resultsPlayer3);
                }
            }
        }
    }
    void aStar(int playerX, int playerZ, int findX, int findZ, List<int> playerList)
    {
        Location target = new Location { X = findX, Z = findZ};
        Location current = null;
        List<Location> openLocations = new List<Location>();
        List<Location> closedLocations = new List<Location>();
        Location start = new Location { X = playerX, Z = playerZ };
        int g = 0;
        openLocations.Add(start);
        int ComputeHScore(int x, int z, int targetX, int targetZ)
        {
            return Math.Abs(targetX - x) + Math.Abs(targetZ - z);
        }
        while (openLocations.Count > 0)
        {
            
            var lowest = openLocations.Min(l => l.F);
            current = openLocations.First(l => l.F == lowest);
            closedLocations.Add(current);
            openLocations.Remove(current);
            if (closedLocations.FirstOrDefault(l => l.X == target.X && l.Z == target.Z) != null)
            {
                playerList.Add(0);
                break;
            }
            var adjacentSquares = GetWalkableAdjacentSquares(current.X, current.Z, helperGameBoard);
            g++;
            List<Location> GetWalkableAdjacentSquares(int x, int z, GameObject[,] gameBoard)
            {
                var proposedLocations = new List<Location>()
                {
                    new Location { X = x, Z = z - 1 },
                    new Location { X = x, Z = z + 1 },
                    new Location { X = x - 1, Z = z },
                    new Location { X = x + 1, Z = z },
                };
               return proposedLocations.Where(
                l => gameBoard[l.X, l.Z] == null || gameBoard[l.X, l.Z].tag == "Player" ).ToList(); 
            }
            foreach (var adjacentSquare in adjacentSquares)
            {
                if (closedLocations.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Z == adjacentSquare.Z) != null)
                    continue;
                if (openLocations.FirstOrDefault(l => l.X == adjacentSquare.X
                        && l.Z == adjacentSquare.Z) == null)
                {
                    adjacentSquare.G = g;
                    adjacentSquare.H = ComputeHScore(adjacentSquare.X,
                        adjacentSquare.Z, target.X, target.Z);
                    adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                    adjacentSquare.parent = current;
                    openLocations.Insert(0, adjacentSquare);
                }
                else
                {
                    if (g + adjacentSquare.H < adjacentSquare.F)
                    {
                        adjacentSquare.G = g;
                        adjacentSquare.F = adjacentSquare.G + adjacentSquare.H;
                        adjacentSquare.parent = current;
                    }
                }
            }
        }
        if(openLocations.Count == 0)
        {
            playerList.Add(1);

        }
    }
    void checkResultsAStar()
    {
        foreach (int element in resultsPlayer0)
        {
            if(element == 0)
            {
                allResults.Add(0);
                break;
            }
        }
        foreach (int element in resultsPlayer1)
        {
            if (element == 0)
            {
                allResults.Add(0);
                break;
            }
        }
        foreach (int element in resultsPlayer2)
        {
            if (element == 0)
            {
                allResults.Add(0);
                break;
            }
        }
        foreach (int element in resultsPlayer3)
        {
            if (element == 0)
            {
                allResults.Add(0);
                break;
            }
        }
    }
    void howManyActivePlane()
    {
        activePlane = 0;
        if((int)select.transform.position.y == -1)
        {
            activePlane += 1;
        }
        if ((int)select2.transform.position.y == -1)
        {
            activePlane += 1;
        }
        if ((int)select3.transform.position.y == -1)
        {
            activePlane += 1;
        }
        if ((int)select4.transform.position.y == -1)
        {
            activePlane += 1;
        }
        if (activePlane == 4 && playersBlock[i] == blockForPlayer)
        {
            info.SetActive(true);
        }
    }
    public void backToStart()
    {
        Application.LoadLevel("start");
    }
    public void backToPlay()
    {
        pause.SetActive(false);
    }
    public void exit()
    {
        Application.Quit();
    }
    public void skipQueue()
    {
        i++;
        info.SetActive(false);
        what = 0;
    }
}
public class Location
{
    public int X;
    public int Z;
    public int F;
    public int G;
    public int H;
    public Location parent;
}

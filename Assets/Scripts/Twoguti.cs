using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Twoguti : MonoBehaviour
{
    public GameObject[] board_cell = new GameObject[5];
    int[] board_num = {1, 1, 2, 2, 0 };
    int settings = 0;
    int current_player = 0;
    bool selected = false;
    public Sprite[] got_value = new Sprite[2];
    public GameObject parentObject;
    GameObject seleceted_object;
    int index = -1;
    public GameObject pauseMenu;
    public GameObject resumeMenu;
    int[,] graph = {{0,1,0,1,1 }, {1,0,1,0,1}, {0,1,0,0,1},{1,0,0,0,1},{1,1,1,1,0}};
    public Button resumeButton, restartButton, exitButton, cancelButton;
    public Button restartBtn, exButton;
    public TextMeshProUGUI text_pop;
    TextMeshPro turning_text;
    public GameObject turning_object;
    public Button pause_object;
    bool isPaused = false;
    bool gameFinished = false;

    public GameObject bground;

    void Start()
    {
        pauseMenu.SetActive(false);
        resumeMenu.SetActive(false);
        resizeScreen();
        settings = PlayerPrefs.GetInt("twoguti", 1);
        resumeButton.onClick.AddListener(onResumeClicked);
        cancelButton.onClick.AddListener(onResumeClicked);
        restartButton.onClick.AddListener(onRestartClicked);
        restartBtn.onClick.AddListener(onRestartClicked);
        exButton.onClick.AddListener(onExitClicked);
        exitButton.onClick.AddListener(onExitClicked);
        pause_object.onClick.AddListener(onPauseGame);
        turning_text = turning_object.GetComponent<TextMeshPro>();
        current_player = Random.Range(1, 3);

        if (settings == 0)
        {
            if(current_player == 2)
            {
                turning_text.text = "RED's Turn";
            }
            else
            {
                turning_text.text = "Blue's Turn";
            }
        }
        else
        {
            if (current_player == 2)
            {
                turning_text.text = "AI's Turn";
                if(settings == 1)
                {
                    Invoke("AI_Turn_Easy", 1f);
                }
                else if(settings == 2)
                {
                    Invoke("AI_Turn_Medium", 1f);
                }
                else
                {
                    Invoke("AI_Turn_Hard", 1f);
                }
            }
            else
            {
                turning_text.text = "Your Turn";
            }
        }
    }
    void onExitClicked()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene("parentpage");
    }

    void onRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void onResumeClicked()
    {
        pauseMenu.SetActive(false);
        isPaused = false;
    }


    void resizeScreen()
    {
        //Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        //RectTransform rectTransform = parentObject.GetComponent<RectTransform>();
        //Vector2 sizeInPixels = rectTransform.rect.size * rectTransform.localScale.x;

        Renderer renderer = parentObject.GetComponent<Renderer>();

        // Get the bounds of the object
        Bounds bounds = renderer.bounds;

        Bounds bounds1 = bground.GetComponent<Renderer>().bounds;

        // Convert the bounds size from world space to screen space
        Vector3 sizeInScreenSpace = Camera.main.WorldToScreenPoint(bounds.size) - Camera.main.WorldToScreenPoint(Vector3.zero);

        Vector3 sizeInScreenSpace1 = Camera.main.WorldToScreenPoint(bounds1.size) - Camera.main.WorldToScreenPoint(Vector3.zero);

        // Get the size in the same unit as Screen.width
        float sizeInScreenUnitswidth = Screen.width / sizeInScreenSpace.x;
        float sizeInScreenUnitsheight = Screen.height / sizeInScreenSpace.y;

        float sizeInScreenUnitswidth1 = Screen.width / sizeInScreenSpace1.x;
        float sizeInScreenUnitsheight1 = Screen.height / sizeInScreenSpace1.y;

        if (sizeInScreenUnitswidth < sizeInScreenUnitsheight)
        {
            sizeInScreenUnitsheight = sizeInScreenUnitswidth;
            //sizeInScreenUnitswidth1 = sizeInScreenUnitsheight1;
        }
        else
        {
            sizeInScreenUnitswidth = sizeInScreenUnitsheight;
            //sizeInScreenUnitsheight1 = sizeInScreenUnitswidth1;
        }

        parentObject.transform.localScale = new Vector3(sizeInScreenUnitswidth, sizeInScreenUnitsheight, 1);
        bground.transform.localScale = new Vector3(sizeInScreenUnitswidth1, sizeInScreenUnitsheight1, 1);
    }


    void Update()
    {
        if (isPaused) return;
        if (gameFinished) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;
                if(settings == 0)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (board_cell[i] == gamer)
                        {
                            if (selected)
                            {
                                if (board_num[i] == current_player)
                                {
                                    seleceted_object.transform.localScale = new Vector3(1f, 1f, 1f);
                                    seleceted_object = gamer;
                                    index = i;
                                    seleceted_object.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                                }
                                else if (board_num[i] == 0)
                                {
                                    if (!isPossibleMove(index, i))
                                    {
                                        return;
                                    }
                                    seleceted_object.transform.localScale = new Vector3(1f, 1f, 1f);
                                    seleceted_object.GetComponent<SpriteRenderer>().sprite = null;
                                    board_num[index] = 0;
                                    board_num[i] = current_player;
                                    selected = false;
                                    SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();
                                    spriteRenderer.sprite = got_value[current_player - 1];
                                    if (isGameEnd())
                                    {
                                        gameFinished = true;
                                        StartCoroutine(showWinner(1));
                                        return;
                                    }
                                    current_player = (current_player == 1) ? 2 : 1;
                                    if(current_player == 2)
                                    {
                                        turning_text.text = "RED's Turn";
                                    }
                                    else
                                    {
                                        turning_text.text = "Blue's Turn";
                                    }
                                }
                            }
                            else
                            {
                                if (board_num[i] == 0 || board_num[i] != current_player)
                                {
                                    return;
                                }

                                seleceted_object = gamer;
                                index = i;
                                seleceted_object.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                                selected = true;

                            }
                        }
                    }
                }

                else
                {
                    if(current_player == 2)
                    {
                        return;
                    }

                    //AI Logic

                    for (int i = 0; i < 5; i++)
                    {
                        if (board_cell[i] == gamer)
                        {
                            if (selected)
                            {
                                if (board_num[i] == current_player)
                                {
                                    seleceted_object.transform.localScale = new Vector3(1f, 1f, 1f);
                                    seleceted_object = gamer;
                                    index = i;
                                    seleceted_object.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                                }

                                else if (board_num[i] == 0)
                                {
                                    if (!isPossibleMove(index, i))
                                    {
                                        return;
                                    }
                                    seleceted_object.transform.localScale = new Vector3(1f, 1f, 1f);
                                    seleceted_object.GetComponent<SpriteRenderer>().sprite = null;
                                    board_num[index] = 0;
                                    board_num[i] = current_player;
                                    selected = false;
                                    SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();
                                    spriteRenderer.sprite = got_value[current_player - 1];
                                    if (isGameEnd())
                                    {
                                        gameFinished = true;
                                        StartCoroutine(showWinner(2));
                                        return;
                                    }
                                    current_player = (current_player == 1) ? 2 : 1;
                                    turning_text.text = "AI's Turn";
                                    if (settings == 1)
                                    {
                                        Invoke("AI_Turn_Easy", 1f);
                                    }
                                    else if(settings == 2)
                                    {
                                        Invoke("AI_Turn_Medium", 1f);
                                    }
                                    else
                                    {
                                        Invoke("AI_Turn_Hard", 1f);
                                    }
                                }
                            }
                            else
                            {
                                if (board_num[i] == 0 || board_num[i] != current_player)
                                {
                                    return;
                                }

                                seleceted_object = gamer;
                                index = i;
                                seleceted_object.transform.localScale = new Vector3(1.5f, 1.5f, 1f);
                                selected = true;

                            }
                        }
                    }
                }

            }
        }
        else if (Input.GetKey(KeyCode.Escape))
        {
            onPauseGame();
        }

    }


    IEnumerator showWinner(int asif)
    {
        yield return new WaitForSeconds(2f);
        if(asif == 1)
        {
            resumeMenu.SetActive(true);
            if (current_player == 1)
            {
                text_pop.text = "Blue is Winner!";
            }
            else
            {
                text_pop.text = "Red is Winner!";
            }
        }
        else if(asif == 2)
        {
            resumeMenu.SetActive(true);
            if (current_player == 1)
            {
                text_pop.text = "You've Won!";
            }
            else
            {
                text_pop.text = "You've Lost!";
            }
        }
        else
        {
            resumeMenu.SetActive(true);
            if (current_player == 2)
            {
                text_pop.text = "You've Lost!";
            }
            else
            {
                text_pop.text = "You've Won!";
            }
        }
    }

    void onPauseGame()
    {
        if (gameFinished) return;
        isPaused = true;
        pauseMenu.SetActive(true);
    }

    bool isPossibleMove(int posInitial, int posFinal)
    {
        return graph[posInitial,posFinal] == 1;
    }


    bool isGameEnd()
    {
        int pos_num = (current_player == 1) ? 2 : 1;
        int pos_zero = -1;
        for(int i=0; i<5; i++)
        {
            if(board_num[i] == 0)
            {
                pos_zero = i;
                break;
            }
        }

        for(int i=0; i<5; i++)
        {
            if(board_num[i] == pos_num)
            {
                if(isPossibleMove(i, pos_zero))
                {
                    return false;
                }
            }
        }
        return true;
    }

    int findZero()
    {
        int pos = -1;
        for (int i = 0; i < 5; i++)
        {
            if(board_num[i] == 0)
            {
                pos = i;
                break;
            }
        }

        return pos;
    }    

    void AI_Turn_Easy()
    {
        int pos_zero = findZero();
        int[] index_store = new int[2];
        int count = 0;
        for(int i=0; i<5; i++)
        {
            if(board_num[i] == current_player && isPossibleMove(i, pos_zero))
            {
                index_store[count] = i;
                count++;
            }
        }

        int rand = Random.Range(0, count);
        board_num[pos_zero] = current_player;
        board_num[index_store[rand]] = 0;
        board_cell[pos_zero].GetComponent<SpriteRenderer>().sprite = got_value[current_player - 1];
        board_cell[index_store[rand]].GetComponent<SpriteRenderer>().sprite = null;
        if (isGameEnd())
        {
            gameFinished = true;
            StartCoroutine(showWinner(3));
        }
        else
        {
            current_player = (current_player == 1) ? 2 : 1;
            turning_text.text = "Your Turn";
        }
    }


    void AI_Turn_Medium()
    {
        int pos_zero = findZero();
        for (int i = 0; i < 5; i++)
        {
            if (board_num[i] == current_player && isPossibleMove(i, pos_zero))
            {
                board_num[pos_zero] = current_player;
                board_num[i] = 0;
                board_cell[pos_zero].GetComponent<SpriteRenderer>().sprite = got_value[current_player - 1];
                board_cell[i].GetComponent<SpriteRenderer>().sprite = null;
                if (isGameEnd())
                {
                    gameFinished = true;
                    StartCoroutine(showWinner(3));
                }
                else
                {
                    current_player = (current_player == 1) ? 2 : 1;
                    turning_text.text = "Your Turn";
                }
                break;
            }
        }
        
    }

    void AI_Turn_Hard()
    {
        int pos1 = -1, pos2 = -1;
        int zero = findZero();
        
        for(int i=0; i<5; i++)
        {
            if(board_num[i] == current_player)
            {
                if (pos1 == -1)
                {
                    pos1 = i;
                }
                else
                {
                    pos2 = i;
                }
            }
        }

        int mov_index = pos1;

        if (graph[pos1,zero] == 1 && graph[pos2, zero] == 1)
        {
            if ((pos1 == 3 && pos2 == 4))
            {
                mov_index = pos1;
            }
            else if ((pos1 == 2 && pos2 == 4))
            {
                mov_index = pos1;
            }
            else if ((pos1 == 1 && pos2 == 3) && (board_num[0] == 1 && board_num[2] == 1))
            {
                mov_index = pos1;
            }
            else if ((pos1 == 0 && pos2 == 2) && (board_num[1] == 1 && board_num[3] == 1))
            {
                mov_index = pos1;
            }
            else
            {
                mov_index = pos2;
            }
        }
        else
        {
            if (graph[pos1, zero] == 1)
            {
                mov_index = pos1;
            }
            else if (graph[pos2, zero] == 1)
            {
                mov_index = pos2;
            }
        }


        board_num[zero] = current_player;
        board_num[mov_index] = 0;
        board_cell[zero].GetComponent<SpriteRenderer>().sprite = got_value[current_player - 1];
        board_cell[mov_index].GetComponent<SpriteRenderer>().sprite = null;

        if (isGameEnd())
        {
            gameFinished = true;
            StartCoroutine(showWinner(3));
        }
        else
        {
            current_player = (current_player == 1) ? 2 : 1;
            turning_text.text = "Your Turn";
        }

    }

}

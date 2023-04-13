using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Simpage : MonoBehaviour
{
    public GameObject[] grid_value = new GameObject[30];
    int[,] grid_num = new int[10, 3];
    GameObject[,] grid_v = new GameObject[10, 3];
    Color[] df_val = {Color.red, Color.blue };
    int current_player = 0;
    int settings = 0;
    public GameObject parentObject;

    public GameObject pauseMenu;
    public GameObject resumeMenu;
    public Button resumeButton, restartButton, exitButton, cancelButton;
    public Button restartBtn, exButton;
    public TextMeshProUGUI text_pop;
    TextMeshPro turning_text;
    public GameObject turning_object;
    public Button pause_object;
    bool isPaused = false;
    bool gameFinish = false;

    public GameObject bground;

    void Start()
    {

        pauseMenu.SetActive(false);
        resumeMenu.SetActive(false);
        resizeScreen();
        resumeButton.onClick.AddListener(onResumeClicked);
        cancelButton.onClick.AddListener(onResumeClicked);
        restartButton.onClick.AddListener(onRestartClicked);
        restartBtn.onClick.AddListener(onRestartClicked);
        exButton.onClick.AddListener(onExitClicked);
        exitButton.onClick.AddListener(onExitClicked);
        pause_object.onClick.AddListener(onPauseGame);
        turning_text = turning_object.GetComponent<TextMeshPro>();

        for (int i = 0; i < 10; i++)
        {
            for(int j=0; j<3; j++)
            {
                grid_num[i,j] = 0;
            }
        }


        for(int i=0; i<30; i++)
        {
            int t1 = i % 3;
            int t2 = i / 3;
            grid_v[t2, t1] = grid_value[i];
        }

        settings = PlayerPrefs.GetInt("simpage", 0);

        current_player = UnityEngine.Random.Range(1, 3);

        if (settings == 0)
        {
            if (current_player == 2)
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
                if (settings == 1)
                {
                    Invoke("AI_Turn_Easy", 1f);
                }
                else if (settings == 2)
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

    void onPauseGame()
    {
        if (gameFinish) return;
        isPaused = true;
        pauseMenu.SetActive(true);
    }


    void Update()
    {
        if (isPaused) return;
        if (gameFinish) return;
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;

                if (settings == 0)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (grid_v[i, j] == gamer)
                            {
                                if (grid_num[i, j] != 0)
                                {
                                    return;
                                }

                                if (CheckifWinner(gamer))
                                {

                                    gameFinish = true;
                                    StartCoroutine(showWinner(2));
                                    return;
                                }

                                if (checkifEnd())
                                {
                                    gameFinish = true;
                                    StartCoroutine(showWinner(1));
                                    return;
                                }

                                current_player = (current_player == 1) ? 2 : 1;
                                if (current_player == 2)
                                {
                                    turning_text.text = "RED's Turn";
                                }
                                else
                                {
                                    turning_text.text = "Blue's Turn";
                                }
                                return;

                            }
                        }
                    }
                }
                else
                {
                    if (current_player == 2) return;

                    for (int i = 0; i < 10; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (grid_v[i, j] == gamer)
                            {
                                if (grid_num[i, j] != 0)
                                {
                                    return;
                                }

                                if (CheckifWinner(gamer))
                                {

                                    gameFinish = true;
                                    StartCoroutine(showWinner(3));
                                    return;
                                }

                                if (checkifEnd())
                                {
                                    gameFinish = true;
                                    StartCoroutine(showWinner(1));
                                    return;
                                }

                                move_AI();
                                return;

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

    void move_AI()
    {
        current_player = (current_player == 1) ? 2 : 1;
        if (current_player == 2)
        {
            turning_text.text = "AI's Turn";
        }
        else
        {
            turning_text.text = "Your Turn";
        }

        if (current_player == 2)
        {
            if (settings == 1)
            {
                Invoke("AI_Turn_Easy", 1f);
            }
            else if (settings == 2)
            {
                Invoke("AI_Turn_Medium", 1f);
            }
            else
            {
                Invoke("AI_Turn_Hard", 1f);
            }
        }
    }

    bool checkifEnd()
    {
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid_num[i, j] == 0)
                {
                    return false;
                }
            }

        }
        return true;
    }


    void AI_Turn_Easy()
    {
        List<Vector2Int> emptyList = new List<Vector2Int>();
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid_num[i, j] == 0)
                {
                    emptyList.Add(new Vector2Int(i, j));
                }
            }
        }

        int rand = UnityEngine.Random.Range(0, emptyList.Count);

        GameObject gm = grid_v[emptyList[rand].x, emptyList[rand].y];
        if (CheckifWinner(gm))
        {
            gameFinish = true;
            StartCoroutine(showWinner(3));
            return;
        }

        if (checkifEnd())
        {
            gameFinish = true;
            StartCoroutine(showWinner(1));
            return;
        }

        move_AI();
    }


    void AI_Turn_Medium()
    {
        int randomIndex = UnityEngine.Random.Range(0, 100);
        int num = randomIndex % 3;
        if (num == 0 || num == 2)
        {
            AI_Turn_Hard();
        }
        else
        {
            AI_Turn_Easy();
        }

    }

    void AI_Turn_Hard()
    {
        int movX = -1, movY = -1, tempX = -1, tempY = -1, blockX = -1, blockY = -1;
        for (int i = 0; i < 10; i++)
        {
            int coun1 = 0, coun2 = 0, coun3 = 0;
            for (int j = 0; j < 3; j++)
            {
                if (grid_num[i, j] == current_player)
                {
                    coun2++;
                }
                else if (grid_num[i, j] == 0)
                {
                    coun1++;
                    tempX = i;
                    tempY = j;
                }
                else
                {
                    coun3++;
                }
            }

            if (coun2 == 2 && coun1 == 1)
            {
                movX = tempX;
                movY = tempY;
                break;
            }
            else if (coun3 == 2 && coun1 == 1)
            {
                blockX = tempX;
                blockY = tempY;
            }

        }

        int finalX, finalY;
        if ((movX != -1 && movY != -1) || (blockX != -1 && blockY != -1))
        {
            if (movX != -1 && movY != -1)
            {
                finalX = movX;
                finalY = movY;
            }
            else
            {
                finalX = blockX;
                finalY = blockY;
            }

            GameObject gm = grid_v[finalX, finalY];
            if (CheckifWinner(gm))
            {
                gameFinish = true;
                StartCoroutine(showWinner(3));
                return;
            }

            if (checkifEnd())
            {
                gameFinish = true;
                StartCoroutine(showWinner(1));
                return;
            }

            move_AI();

        }
        else
        {
            AI_Turn_Easy();
        }



    }



    bool CheckifWinner(GameObject gm)
    {
        bool flag = false;
        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (grid_v[i, j] == gm)
                {
                    grid_num[i, j] = current_player;
                    grid_v[i, j].GetComponent<SpriteRenderer>().color = df_val[current_player - 1];
                }
            }
        }

        for (int i = 0; i < 10; i++)
        {
            int count = 0;
            for (int j = 0; j < 3; j++)
            {
                if (grid_num[i, j] == current_player)
                {
                    count++;
                }
            }

            if (count == 3)
            {
                flag = true;
                break;
            }
        }


        return flag;
    }


    IEnumerator showWinner(int ridoy)
    {
        yield return new WaitForSeconds(2.5f);
        if (ridoy == 1)
        {
            resumeMenu.SetActive(true);
            text_pop.text = "Match Draw!";
        }
        else if (ridoy == 2)
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
        else if (ridoy == 3)
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
    }

}

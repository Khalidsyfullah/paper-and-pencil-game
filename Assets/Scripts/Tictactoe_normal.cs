using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Tictactoe_normal : MonoBehaviour
{
    GameObject[,] grid_cell = new GameObject[3, 3];
    int[,] grid_board = new int[3, 3];
    public GameObject mainParent;
    int current_player = 1;
    public Sprite[] move_object = new Sprite[2];
    int settings = 0;
    public GameObject pauseMenu;
    public GameObject resumeMenu;
    public Button resumeButton, restartButton, exitButton, cancelButton;
    public Button restartBtn, exButton;
    public TextMeshProUGUI text_pop;
    TextMeshPro turning_text;
    public GameObject turning_object;
    public Button pause_object;
    bool isPaused = false;
    //0 == 2 player
    //1 == Normal Multiplayer
    //2 == medium
    //3 == hard
    public GameObject parentObject;
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

        string name = "Artboard 1_9";
        for(int i=0; i<9; i++)
        {
            string temp = name + " ("+i+")";
            int t1 = i / 3;
            int t2 = i % 3;
            Transform childTransform = mainParent.transform.Find(temp);
            grid_cell[t1, t2] = childTransform.gameObject;
        }

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                grid_board[i, j] = 0;
            }
        }
        settings = PlayerPrefs.GetInt("Tictactoe", 0);
        current_player = Random.Range(1, 3);

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
                SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();

                if (settings != 0)
                {
                    if (current_player == 2) return;

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (grid_cell[i, j] == gamer)
                            {
                                if(grid_board[i,j] == 0)
                                {
                                    grid_board[i, j] = current_player;
                                    spriteRenderer.sprite = move_object[current_player - 1];
                                    int val = CheckWinnerisReady(grid_board);
                                    if (val == 0)
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

                                        if(current_player == 2)
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
                                        return;
                                    }
                                    else if(val == -1)
                                    {
                                        gameFinish = true;
                                        StartCoroutine(showWinner(1));
                                        return;
                                    }
                                    else
                                    {
                                        gameFinish = true;
                                        StartCoroutine(showWinner(3));
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (grid_cell[i, j] == gamer)
                            {
                                if (grid_board[i, j] == 0)
                                {
                                    grid_board[i, j] = current_player;
                                    spriteRenderer.sprite = move_object[current_player-1];
                                    int val = CheckWinnerisReady(grid_board);
                                    if (val == 0)
                                    {
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
                                    else if (val == -1)
                                    {
                                        gameFinish = true;
                                        StartCoroutine(showWinner(1));
                                        return;
                                    }
                                    else
                                    {
                                        gameFinish = true;
                                        StartCoroutine(showWinner(2));
                                        return;
                                    }
                                }
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

    
    IEnumerator showWinner(int ridoy)
    {
        yield return new WaitForSeconds(1f);
        if(ridoy == 1)
        {
            resumeMenu.SetActive(true);
            text_pop.text = "Match Draw!";
        }
        else if(ridoy == 2)
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
        else if(ridoy == 3)
        {
            resumeMenu.SetActive(true);
            if(current_player == 1)
            {
                text_pop.text = "You've Won!";
            }
            else
            {
                text_pop.text = "You've Lost!";
            }
        }
    }


    void onPauseGame()
    {
        if (gameFinish) return;
        isPaused = true;
        pauseMenu.SetActive(true);
    }




    void AI_Turn_Easy()
    {
        int coun = 0;
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(grid_board[i,j] == 0)
                {
                    coun++;
                }
            }
        }

        int indexc = Random.Range(0, coun);
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(grid_board[i,j] == 0)
                {
                    if(indexc == 0)
                    {
                        grid_board[i, j] = current_player;
                        SpriteRenderer spriteRenderer = grid_cell[i, j].GetComponent<SpriteRenderer>();
                        spriteRenderer.sprite = move_object[current_player - 1];
                        int val = CheckWinnerisReady(grid_board);
                        if (val == 0)
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
                        }
                        else if (val == -1)
                        {
                            gameFinish = true;
                            StartCoroutine(showWinner(1));
                        }
                        else
                        {
                            gameFinish = true;
                            StartCoroutine(showWinner(3));
                        }
                        return;
                    }
                    indexc--;
                }
            }
        }
    }


    void AI_Turn_Hard()
    {
        int cont0 = 0, cont1 = 0, cont2 = 0;
        bool finish = false;
        int finalx = 0, finaly = 0;
        int temp = 0;
        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(grid_board[i,j] == 0)
                {
                    finalx = i;
                    finaly = j;
                    break;
                }
            }
        }

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(grid_board[i,j] == 0)
                {
                    temp = j;
                    cont0++;
                }
                else if(grid_board[i,j] == 1)
                {
                    cont1++;
                }
                else if(grid_board[i,j] == 2)
                {
                    cont2++;
                }
            }
            if((cont1 == 2 || cont2 == 2) && cont0 == 1)
            {
                finalx = i;
                finaly = temp;
                finish = true;
                break;
            }
            cont0 = 0;
            cont1 = 0;
            cont2 = 0;
        }

        if (!finish)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (grid_board[j, i] == 0)
                    {
                        temp = j;
                        cont0++;
                    }
                    else if (grid_board[j, i] == 1)
                    {
                        cont1++;
                    }
                    else if (grid_board[j, i] == 2)
                    {
                        cont2++;
                    }
                }
                if ((cont1 == 2 || cont2 == 2) && cont0 == 1)
                {
                    finalx = temp;
                    finaly = i;
                    finish = true;
                    break;
                }

                cont0 = 0;
                cont1 = 0;
                cont2 = 0;
            }
        }

        if (!finish)
        {
            for (int i = 0; i < 3; i++)
            {

                if (grid_board[i, i] == 0)
                {
                    temp = i;
                    cont0++;
                }
                else if (grid_board[i, i] == 1)
                {
                    cont1++;
                }
                else if (grid_board[i, i] == 2)
                {
                    cont2++;
                }
                if ((cont1 == 2 || cont2 == 2) && cont0 == 1)
                {
                    finalx = temp;
                    finaly = i;
                    finish = true;
                    break;
                }

            }
            cont0 = 0;
            cont1 = 0;
            cont2 = 0;
        }

        if (!finish)
        {
            for (int i = 0; i < 3; i++)
            {

                if (grid_board[i, 2 - i] == 0)
                {
                    temp = i;
                    cont0++;
                }
                else if (grid_board[i, 2 - i] == 1)
                {
                    cont1++;
                }
                else if (grid_board[i, 2 - i] == 2)
                {
                    cont2++;
                }
                if ((cont1 == 2 || cont2 == 2) && cont0 == 1)
                {
                    finalx = temp;
                    finaly = 2 - i;
                    finish = true;
                    break;
                }

            }
        }

        if (!finish)
        {
            AI_Turn_Easy();
        }
        else
        {
            grid_board[finalx, finaly] = current_player;
            SpriteRenderer spriteRenderer = grid_cell[finalx, finaly].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = move_object[current_player - 1];
            int val = CheckWinnerisReady(grid_board);
            if (val == 0)
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
            }
            else if (val == -1)
            {
                gameFinish = true;
                StartCoroutine(showWinner(1));
            }
            else
            {
                gameFinish = true;
                StartCoroutine(showWinner(3));
            }
        }

    }

    int FindNextBestMove(int[,] board)
    {
        // Check rows for two in a row
        for (int i = 0; i < 3; i++)
        {
            int playerCount = 0;
            int emptyCount = 0;
            int emptyIndex = -1;

            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 1)
                {
                    playerCount++;
                }
                else if (board[i, j] == 0)
                {
                    emptyCount++;
                    emptyIndex = j;
                }
            }

            if (playerCount == 2 && emptyCount == 1)
            {
                return i * 3 + emptyIndex;
            }
        }

        // Check columns for two in a row
        for (int j = 0; j < 3; j++)
        {
            int playerCount = 0;
            int emptyCount = 0;
            int emptyIndex = -1;

            for (int i = 0; i < 3; i++)
            {
                if (board[i, j] == 1)
                {
                    playerCount++;
                }
                else if (board[i, j] == 0)
                {
                    emptyCount++;
                    emptyIndex = i;
                }
            }

            if (playerCount == 2 && emptyCount == 1)
            {
                return emptyIndex * 3 + j;
            }
        }

        // Check diagonal from top left to bottom right
        int diagPlayerCount = 0;
        int diagEmptyCount = 0;
        int diagEmptyIndex = -1;

        for (int i = 0; i < 3; i++)
        {
            if (board[i, i] == 1)
            {
                diagPlayerCount++;
            }
            else if (board[i, i] == 0)
            {
                diagEmptyCount++;
                diagEmptyIndex = i;
            }
        }

        if (diagPlayerCount == 2 && diagEmptyCount == 1)
        {
            return diagEmptyIndex * 3 + diagEmptyIndex;
        }

        // Check diagonal from top right to bottom left
        diagPlayerCount = 0;
        diagEmptyCount = 0;
        diagEmptyIndex = -1;

        for (int i = 0; i < 3; i++)
        {
            if (board[i, 2 - i] == 1)
            {
                diagPlayerCount++;
            }
            else if (board[i, 2 - i] == 0)
            {
                diagEmptyCount++;
                diagEmptyIndex = i;
            }
        }

        if (diagPlayerCount == 2 && diagEmptyCount == 1)
        {
            return diagEmptyIndex * 3 + (2 - diagEmptyIndex);
        }

        // If no winning move, pick random empty cell
        List<int> emptyCells = new List<int>();

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (board[i, j] == 0)
                {
                    emptyCells.Add(i * 3 + j);
                }
            }
        }

        if (emptyCells.Count > 0)
        {
            return emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];
        }

        // No empty cells left, return -1
        return -1;
    }


        void AI_Turn_Medium()
    {

        int num = FindNextBestMove(grid_board);

        int bestMoveX = num/3;
        int bestMoveY = num%3;
        

        grid_board[bestMoveX, bestMoveY] = current_player;
        SpriteRenderer spriteRenderer = grid_cell[bestMoveX, bestMoveY].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[current_player - 1];
        int val = CheckWinnerisReady(grid_board);
        if (val == 0)
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
        }
        else if (val == -1)
        {
            gameFinish = true;
            StartCoroutine(showWinner(1));
        }
        else
        {
            gameFinish = true;
            StartCoroutine(showWinner(3));
        }
    }

    

    private int CheckWinnerisReady(int[,] board)
    {
        // Check rows
        for (int i = 0; i < 3; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2])
            {
                if (board[i, 0] == 0) return 0;
                drawLine(grid_cell[i,0], grid_cell[i, 2], current_player);
                if (board[i, 0] == 1)
                {
                    return 1; // Player 1 wins
                }
                else if (board[i, 0] == 2)
                {
                    return 2; // Player 2 wins
                }
            }
        }

        // Check columns
        for (int j = 0; j < 3; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j])
            {
                if (board[0, j] == 0) return 0;
                drawLine(grid_cell[0, j], grid_cell[2, j], current_player);

                if (board[0, j] == 1)
                {
                    return 1; // Player 1 wins
                }
                else if (board[0, j] == 2)
                {
                    return 2; // Player 2 wins
                }
            }
        }

        // Check diagonals
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[1, 1] != 0)
        {
            drawLine(grid_cell[0, 0], grid_cell[2, 2], current_player);

            if (board[0, 0] == 1)
            {
                return 1; // Player 1 wins
            }
            else if (board[0, 0] == 2)
            {
                return 2; // Player 2 wins
            }
        }

        if (board[0, 2] == board[1, 1] && board[1, 1] == board[2, 0] && board[1, 1] != 0)
        {
            drawLine(grid_cell[2, 0], grid_cell[0, 2], current_player);

            if (board[0, 2] == 1)
            {
                return 1; // Player 1 wins
            }
            else if (board[0, 2] == 2)
            {
                return 2; // Player 2 wins
            }
        }

        for(int i=0; i<3; i++)
        {
            for(int j=0; j<3; j++)
            {
                if (board[i, j] == 0) return 0;
            }
        }

        // Game is not over yet
        return -1;
    }


    void drawLine(GameObject g1, GameObject g2, int current_player)
    {
        LineRenderer lineRenderer;
        lineRenderer = g1.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, g1.transform.position);
        lineRenderer.SetPosition(1, g2.transform.position);
        lineRenderer.sortingOrder = 4;
        Color blue = Color.blue;
        Color red = Color.red;
        if(current_player == 1)
        {
            lineRenderer.startColor = blue;
            lineRenderer.endColor = blue;
        }
        else
        {
            lineRenderer.startColor = red;
            lineRenderer.endColor = red;
        }
    }

    

}

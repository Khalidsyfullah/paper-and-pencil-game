using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Sosgame : MonoBehaviour
{
    public GameObject gamePopScreen;
    public GameObject mainParent;
    GameObject[,] grid_cell = new GameObject[7, 6];
    int[,] grid_board = new int[7, 6];

    int current_player = 1;
    public Sprite[] move_object = new Sprite[2];
    int settings = 0;
    public Button button1, button2, button3;
    bool runningFlag = false;
    int selectedX = -1, selectedY = -1;
    public GameObject parentObject;

    public GameObject pauseMenu, score1, score2;
    public GameObject resumeMenu;
    public Button resumeButton, restartButton, exitButton, cancelButton;
    public Button restartBtn, exButton;
    public TextMeshProUGUI text_pop;
    TextMeshPro turning_text, score1val, score2val;
    public GameObject turning_object;
    public Button pause_object;
    bool isPaused = false;
    bool gameFinish = false;
    int scorenum1 = 0, scorenum2 = 0;

    public GameObject bground;

    void Start()
    {
        resizeScreen();
        gamePopScreen.SetActive(false);
        pauseMenu.SetActive(false);
        resumeMenu.SetActive(false);
        resumeButton.onClick.AddListener(onResumeClicked);
        cancelButton.onClick.AddListener(onResumeClicked);
        restartButton.onClick.AddListener(onRestartClicked);
        restartBtn.onClick.AddListener(onRestartClicked);
        exButton.onClick.AddListener(onExitClicked);
        exitButton.onClick.AddListener(onExitClicked);
        pause_object.onClick.AddListener(onPauseGame);
        turning_text = turning_object.GetComponent<TextMeshPro>();
        score1val = score1.GetComponent<TextMeshPro>();
        score2val = score2.GetComponent<TextMeshPro>();


        string name = "Artboard 1@1.5xsos2_5";
        for (int i = 0; i < 42; i++)
        {
            string temp = name + " (" + i + ")";
            int t1 = i / 6;
            int t2 = i % 6;
            Transform childTransform = mainParent.transform.Find(temp);
            grid_cell[t1, t2] = childTransform.gameObject;
        }

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                grid_board[i, j] = 0;
            }
        }
        settings = PlayerPrefs.GetInt("sosgame", 0);

        current_player = UnityEngine.Random.Range(1, 3);

        if (settings == 0)
        {
            score1val.text = "Blue's Score: 0";
            score2val.text = "RED's Score: 0";
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
            score1val.text = "Your Score: 0";
            score2val.text = "AI's Score: 0";
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

        button1.onClick.AddListener(button1Click1);
        button2.onClick.AddListener(button1Click2);
        button3.onClick.AddListener(button1Click3);
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
        if (gamePopScreen.activeSelf)
        {
            runningFlag = false;
            gamePopScreen.SetActive(false);
        }
        else
        {
            isPaused = true;
            pauseMenu.SetActive(true);
        }
        
    }


    void button1Click1()
    {
        gamePopScreen.SetActive(false);
        grid_board[selectedX, selectedY] = 1;
        SpriteRenderer spriteRenderer = grid_cell[selectedX, selectedY].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[0];
        runningFlag = false;
        if(settings == 0)
        {
            action_Player(CheckWinnerisReady(grid_board));
        }
        else
        {
            action_AI(CheckWinnerisReady(grid_board));
        }
    }

    void button1Click2()
    {
        gamePopScreen.SetActive(false);
        grid_board[selectedX, selectedY] = 2;
        SpriteRenderer spriteRenderer = grid_cell[selectedX, selectedY].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[1];
        runningFlag = false;
        if (settings == 0)
        {
            action_Player(CheckWinnerisReady(grid_board));
        }
        else
        {
            action_AI(CheckWinnerisReady(grid_board));
        }
    }

    void button1Click3()
    {
        gamePopScreen.SetActive(false);
        selectedX = -1;
        selectedY = -1;
        runningFlag = false;
    }

    void Update()
    {
        if (runningFlag) return;
        if (isPaused) return;
        if (gameFinish) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;
                if (settings != 0)
                {
                    if (current_player == 2) return;

                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (grid_cell[i, j] == gamer)
                            {
                                if (grid_board[i, j] == 0)
                                {
                                    runningFlag = true;
                                    selectedX = i;
                                    selectedY = j;
                                    gamePopScreen.SetActive(true);
                                }
                            }
                        }
                    }
                }
                else
                {

                    for (int i = 0; i < 7; i++)
                    {
                        for (int j = 0; j < 6; j++)
                        {
                            if (grid_cell[i, j] == gamer)
                            {
                                if (grid_board[i, j] == 0)
                                {
                                    runningFlag = true;
                                    selectedX = i;
                                    selectedY = j;
                                    gamePopScreen.SetActive(true);
                                }
                            }
                        }
                    }
                }


            }
        }

        else if (Input.GetKey(KeyCode.Escape))
        {
            if (gamePopScreen.activeSelf)
            {
                gamePopScreen.SetActive(false);
                runningFlag = false;
            }
            else
            {
                onPauseGame();
            }
        }
    }


    void AI_Turn_Easy()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                if (grid_board[i, j] == 0)
                {
                    emptyCells.Add(new Vector2Int(i, j));
                }
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyCells.Count);
        int ix = emptyCells[randomIndex].x;
        int jx = emptyCells[randomIndex].y;

        int num = UnityEngine.Random.Range(1, 3);
        grid_board[ix, jx] = num;
        SpriteRenderer spriteRenderer = grid_cell[ix, jx].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[num - 1];
        action_AI(CheckWinnerisReady(grid_board));
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
        int rows = grid_board.GetLength(0);
        int cols = grid_board.GetLength(1);
        int[] values = { 0, 0, 0 };
        int movX = -1, movY = -1;
        bool isfound = false;
        int value = -1;
        // Check rows for a win
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j <= cols - 3; j++)
            {
                if (grid_board[i, j] == 1)
                {
                    values[0]++;
                }
                
                if (grid_board[i, j + 1] == 2)
                {
                    values[1]++;
                }
                

                if (grid_board[i, j + 2] == 1)
                {
                    values[2]++;
                }

                if (values[0] == 1 && values[1] == 1 && grid_board[i, j + 2] == 0)
                {
                    movX = i;
                    movY = j+2;
                    value = 1;
                    isfound = true;
                    break;
                }

                if (values[1] == 1 && values[2] == 1 && grid_board[i, j] == 0)
                {
                    movX = i;
                    movY = j;
                    value = 1;
                    isfound = true;
                    break;
                }

                if (values[0] == 1 && values[2] == 1 && grid_board[i, j+1] == 0)
                {
                    movX = i;
                    movY = j+1;
                    value = 2;
                    isfound = true;
                    break;
                }

                values[0] = 0;
                values[1] = 0;
                values[2] = 0;
            }

            if (isfound)
            {
                break;
            }

            values[0] = 0;
            values[1] = 0;
            values[2] = 0;
        }


        // Check columns for a win
        if (!isfound)
        {
            for (int i = 0; i <= rows - 3; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    if (grid_board[i, j] == 1)
                    {
                        values[0]++;
                    }

                    if (grid_board[i+1, j] == 2)
                    {
                        values[1]++;
                    }


                    if (grid_board[i+2, j] == 1)
                    {
                        values[2]++;
                    }

                    if (values[0] == 1 && values[1] == 1 && grid_board[i+2, j] == 0)
                    {
                        movX = i+2;
                        movY = j;
                        value = 1;
                        isfound = true;
                        break;
                    }

                    if (values[1] == 1 && values[2] == 1 && grid_board[i, j] == 0)
                    {
                        movX = i;
                        movY = j;
                        value = 1;
                        isfound = true;
                        break;
                    }

                    if (values[0] == 1 && values[2] == 1 && grid_board[i+1, j] == 0)
                    {
                        movX = i+1;
                        movY = j;
                        value = 2;
                        isfound = true;
                        break;
                    }

                    values[0] = 0;
                    values[1] = 0;
                    values[2] = 0;
                }

                if (isfound)
                {
                    break;
                }

                values[0] = 0;
                values[1] = 0;
                values[2] = 0;
            }
        }


        // Check diagonal (top left to bottom right) for a win
        if (!isfound)
        {

            for (int i = 0; i <= rows - 3; i++)
            {
                for (int j = 0; j <= cols - 3; j++)
                {
                    if (grid_board[i, j] == 1)
                    {
                        values[0]++;
                    }

                    if (grid_board[i+1, j + 1] == 2)
                    {
                        values[1]++;
                    }


                    if (grid_board[i+2, j + 2] == 1)
                    {
                        values[2]++;
                    }

                    if (values[0] == 1 && values[1] == 1 && grid_board[i+2, j + 2] == 0)
                    {
                        movX = i+2;
                        movY = j + 2;
                        value = 1;
                        isfound = true;
                        break;
                    }

                    if (values[1] == 1 && values[2] == 1 && grid_board[i, j] == 0)
                    {
                        movX = i;
                        movY = j;
                        value = 1;
                        isfound = true;
                        break;
                    }

                    if (values[0] == 1 && values[2] == 1 && grid_board[i+1, j + 1] == 0)
                    {
                        movX = i+1;
                        movY = j + 1;
                        value = 2;
                        isfound = true;
                        break;
                    }

                    values[0] = 0;
                    values[1] = 0;
                    values[2] = 0;
                }

                if (isfound)
                {
                    break;
                }

                values[0] = 0;
                values[1] = 0;
                values[2] = 0;
            }
        }




        // Check diagonal (bottom left to top right) for a win
        if (!isfound)
        {

            for (int i = 2; i < rows; i++)
            {
                for (int j = 0; j <= cols - 3; j++)
                {
                    if (grid_board[i, j] == 1)
                    {
                        values[0]++;
                    }

                    if (grid_board[i-1, j + 1] == 2)
                    {
                        values[1]++;
                    }


                    if (grid_board[i-2, j + 2] == 1)
                    {
                        values[2]++;
                    }

                    if (values[0] == 1 && values[1] == 1 && grid_board[i-2, j + 2] == 0)
                    {
                        movX = i-2;
                        movY = j + 2;
                        value = 1;
                        isfound = true;
                        break;
                    }

                    if (values[1] == 1 && values[2] == 1 && grid_board[i, j] == 0)
                    {
                        movX = i;
                        movY = j;
                        value = 1;
                        isfound = true;
                        break;
                    }

                    if (values[0] == 1 && values[2] == 1 && grid_board[i-1, j + 1] == 0)
                    {
                        movX = i-1;
                        movY = j + 1;
                        value = 2;
                        isfound = true;
                        break;
                    }

                    values[0] = 0;
                    values[1] = 0;
                    values[2] = 0;
                }

                if (isfound)
                {
                    break;
                }

                values[0] = 0;
                values[1] = 0;
                values[2] = 0;
            }
        }
        int finalposX, finalposY, final_val;
        if (movX != -1 && movY != -1 && value!= -1)
        {
            finalposX = movX;
            finalposY = movY;
            final_val = value;
        }
        
        else
        {
            List<Vector2Int> emptyCells = new List<Vector2Int>();

            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    if (grid_board[i, j] == 0)
                    {
                        emptyCells.Add(new Vector2Int(i, j));
                    }
                }
            }

            int randomIndex = UnityEngine.Random.Range(0, emptyCells.Count);
            finalposX = emptyCells[randomIndex].x;
            finalposY = emptyCells[randomIndex].y;
            final_val = randomIndex % 2;
            final_val++;
        }

        grid_board[finalposX, finalposY] = final_val;
        SpriteRenderer spriteRenderer = grid_cell[finalposX, finalposY].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[final_val - 1];
        action_AI(CheckWinnerisReady(grid_board));

    }


    int checkifAvailable(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0)
                {
                    return 0;
                }
            }
        }

        return -1;
    }

    void action_Player(int val)
    {
        if (val != -1)
        {
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
            }
            else if (val == 1)
            {
                scorenum1++;
                score1val.text = "Blue's Score: " + scorenum1;
                if (checkifAvailable(grid_board) == 0) return;
                else
                {
                    gameFinish = true;
                    StartCoroutine(showWinner(1));
                }
            }
            else
            {
                scorenum2++;
                score2val.text = "RED's Score: " + scorenum2;
                if (checkifAvailable(grid_board) == 0) return;
                else
                {
                    gameFinish = true;
                    StartCoroutine(showWinner(1));
                }
            }

            return;

        }
        else
        {
            gameFinish = true;
            StartCoroutine(showWinner(1));
            return;
        }
    }

    void action_AI(int val)
    {
        if (val != -1)
        {
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
                return;

            }
            else if (val == 1)
            {
                scorenum1++;
                score1val.text = "Your Score: " + scorenum1;
                if (checkifAvailable(grid_board) == 0) return;
                else
                {
                    gameFinish = true;
                    StartCoroutine(showWinner(2));
                }
            }
            else
            {
                scorenum2++;
                score2val.text = "AI's Score: " + scorenum2;
                if (checkifAvailable(grid_board) == 0)
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
                else
                {
                    gameFinish = true;
                    StartCoroutine(showWinner(2));
                }
            }

            return;

        }
        else
        {
            gameFinish = true;
            StartCoroutine(showWinner(2));
            return;
        }
    }


    IEnumerator showWinner(int ridoy)
    {
        yield return new WaitForSeconds(1f);
        if (ridoy == 1)
        {
            resumeMenu.SetActive(true);
            string val = "Blue's Score: " + scorenum1 + " & Red's Score: " + scorenum2;
            if (scorenum1 == scorenum2)
            {
                val += "\nMatch Draw!";
            }
            else if (scorenum1 > scorenum2)
            {
                val += "\nBlue Wins!!!";
            }
            else
            {
                val += "\nRed Wins!!!";
            }
            text_pop.text = val;
        }
        else
        {
            resumeMenu.SetActive(true);
            string val = "Your Score: " + scorenum1 + " & AI's Score: " + scorenum2;
            if (scorenum1 == scorenum2)
            {
                val += "\nMatch Draw!";
            }
            else if (scorenum1 > scorenum2)
            {
                val += "\nYou Win!!!";
            }
            else
            {
                val += "\nAI Wins!!!";
            }
            text_pop.text = val;
        }

    }


    int CheckWinnerisReady(int[,] grid)
    {
        int rows = grid.GetLength(0);
        int cols = grid.GetLength(1);

        // Check rows for a win
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j <= cols - 3; j++)
            {
                if (grid[i, j] == 1 &&
                    grid[i, j + 1] == 2 &&
                    grid[i, j + 2] == 1)
                {
                    grid[i, j] = -current_player;
                    grid[i, j + 1] = -current_player;
                    grid[i, j + 2] = -current_player;
                    drawLine(grid_cell[i, j], grid_cell[i, j + 2]);
                    return current_player;
                }
            }
        }

        // Check columns for a win
        for (int i = 0; i <= rows - 3; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 1 &&
                    grid[i + 1, j] == 2 &&
                    grid[i + 2, j] == 1)
                {
                    grid[i, j] = -current_player;
                    grid[i + 1, j] = -current_player;
                    grid[i + 2, j] = -current_player;
                    drawLine(grid_cell[i, j], grid_cell[i + 2, j]);
                    return current_player;
                }
            }
        }

        // Check diagonal (top left to bottom right) for a win
        for (int i = 0; i <= rows - 3; i++)
        {
            for (int j = 0; j <= cols - 3; j++)
            {
                if (grid[i, j] != 0 &&
                    grid[i, j] == 1 &&
                    grid[i + 1, j + 1] == 2 &&
                    grid[i + 2, j + 2] == 1)
                {
                    grid[i, j] = -current_player;
                    grid[i + 1, j + 1] = -current_player;
                    grid[i + 2, j + 2] = -current_player;
                    drawLine(grid_cell[i, j], grid_cell[i + 2, j + 2]);
                    return current_player;
                }
            }
        }

        // Check diagonal (bottom left to top right) for a win
        for (int i = 2; i < rows; i++)
        {
            for (int j = 0; j <= cols - 3; j++)
            {
                if (grid[i, j] != 0 &&
                    grid[i, j] == 1 &&
                    grid[i - 1, j + 1] == 2 &&
                    grid[i - 2, j + 2] == 1)
                {
                    grid[i, j] = -current_player;
                    grid[i - 1, j + 1] = -current_player;
                    grid[i - 2, j + 2] = -current_player;
                    drawLine(grid_cell[i, j], grid_cell[i - 2, j + 2]);
                    return current_player;
                }
            }
        }

        // Check if there are any empty cells left
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i, j] == 0)
                {
                    return 0;
                }
            }
        }

        // If no winner and no empty cells, then it's a draw
        return -1;
    }


    void drawLine(GameObject g1, GameObject g2)
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
        if (current_player == 1)
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

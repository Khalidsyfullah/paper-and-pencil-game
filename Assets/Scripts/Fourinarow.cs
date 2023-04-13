using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Fourinarow : MonoBehaviour
{
    public GameObject mainParent;
    GameObject[,] grid_cell = new GameObject[10, 7];
    int[,] grid_board = new int[10, 7];
    Vector2Int[] emptyCells = new Vector2Int[7];
    int current_player = 1;
    public Sprite[] move_object = new Sprite[2];
    int settings = 0;
    public GameObject parentObject;
    public Sprite possible_move;

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


        int nuj = 79;
        string name = "Artboard 14inarow1_14";
        for (int i= 0; i<10; i++)
        {
            for(int j=6; j>=0; j--)
            {
                string temp = name + " (" + nuj + ")";
                Transform childTransform = mainParent.transform.Find(temp);
                
                if(childTransform == null)
                {
                    nuj--;
                    temp = name + " (" + nuj + ")";
                    childTransform = mainParent.transform.Find(temp);
                }
                GameObject gof = childTransform.gameObject;
                grid_cell[i, j] = gof;
                nuj--;
            }
        }

        for (int i = 0; i < 10; i++)
        {
            for (int j = 0; j < 7; j++)
            {
                grid_board[i, j] = 0;
            }
        }


        for(int i=0; i<7; i++)
        {
            emptyCells[i] = new Vector2Int(9, i);
            SpriteRenderer spriteRenderer = grid_cell[9, i].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = possible_move;
        }


        settings = PlayerPrefs.GetInt("fourinarow", 0);

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
                    Invoke("AI_Turn_Easy", 1.5f);
                }
                else if (settings == 2)
                {
                    Invoke("AI_Turn_Medium", 1.5f);
                }
                else
                {
                    Invoke("AI_Turn_Hard", 1.5f);
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

    void onPauseGame()
    {
        if (gameFinish) return;
        isPaused = true;
        pauseMenu.SetActive(true);
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

                    for (int i = 0; i < 7; i++)
                    {
                        if (emptyCells[i].x < 0 || emptyCells[i].y < 0) continue;

                        if (grid_cell[emptyCells[i].x, emptyCells[i].y] == gamer)
                        {
                            grid_board[emptyCells[i].x, emptyCells[i].y] = current_player;
                            spriteRenderer.sprite = move_object[current_player - 1];

                            emptyCells[i].x--;
                            if (emptyCells[i].x >= 0)
                            {
                                grid_cell[emptyCells[i].x, emptyCells[i].y].GetComponent<SpriteRenderer>().sprite = possible_move;
                            }

                            int value = CheckWinner(grid_board);
                            if (value == 0)
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
                                        Invoke("AI_Turn_Easy", 1.5f);
                                    }
                                    else if (settings == 2)
                                    {
                                        Invoke("AI_Turn_Medium", 1.5f);
                                    }
                                    else
                                    {
                                        Invoke("AI_Turn_Hard", 1.5f);
                                    }
                                }
                                return;
                            }
                            else if (value == -1)
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
                else
                {

                    for(int i=0; i<7; i++)
                    {
                        if (emptyCells[i].x < 0 || emptyCells[i].y < 0) continue;

                        if(grid_cell[emptyCells[i].x, emptyCells[i].y] == gamer)
                        {
                            grid_board[emptyCells[i].x, emptyCells[i].y] = current_player;
                            spriteRenderer.sprite = move_object[current_player - 1];
                            int val = CheckWinner(grid_board);
                            emptyCells[i].x--;
                            if (emptyCells[i].x >= 0)
                            {
                                grid_cell[emptyCells[i].x, emptyCells[i].y].GetComponent<SpriteRenderer>().sprite = possible_move;
                            }
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

                    return;
                }


            }
        }

        else if (Input.GetKey(KeyCode.Escape))
        {
            onPauseGame();
        }
    }

    void AI_Turn_Easy()
    {
        List<Vector2Int> emptyPo = new List<Vector2Int>();

        for (int i = 0; i < 7; i++)
        {
            if(emptyCells[i].x >=0 && emptyCells[i].y >= 0)
            {
                emptyPo.Add(emptyCells[i]);
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, emptyPo.Count);
        int ix = emptyPo[randomIndex].x;
        int jx = emptyPo[randomIndex].y;

        grid_board[ix, jx] = current_player;
        SpriteRenderer spriteRenderer = grid_cell[ix, jx].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[current_player - 1];

        for(int i=0; i<7; i++)
        {
            if(emptyCells[i] == emptyPo[randomIndex])
            {
                emptyCells[i].x--;
                if (emptyCells[i].x >= 0)
                {
                    grid_cell[emptyCells[i].x, emptyCells[i].y].GetComponent<SpriteRenderer>().sprite = possible_move;
                }
                break;
            }
        }
        

        int val = CheckWinner(grid_board);
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

        int tempX = -1, tempY = -1;
        int movX = -1, movY = -1;

        for(int i=0; i<7; i++)
        {
            if (emptyCells[i].x < 0 || emptyCells[i].y < 0) continue;

            grid_board[emptyCells[i].x, emptyCells[i].y] = current_player;
            int val = CheckWinnerTemp(grid_board);
            grid_board[emptyCells[i].x, emptyCells[i].y] = 0;
            if (val == current_player)
            {
                movX = emptyCells[i].x;
                movY = emptyCells[i].y;
                break;
            }

            int num = (current_player == 2) ? 1 : 2;
            grid_board[emptyCells[i].x, emptyCells[i].y] = num;
            val = CheckWinnerTemp(grid_board);
            grid_board[emptyCells[i].x, emptyCells[i].y] = 0;
            if (val == num)
            {
                tempX = emptyCells[i].x;
                tempY = emptyCells[i].y;
            }
        }

        if((movX!= -1 && movY!= -1) || (tempX!= -1 && tempY!= -1))
        {
            int finalX, finalY;
            if (movX != -1 && movY != -1)
            {
                finalX = movX;
                finalY = movY;
            }
            else
            {
                finalX = tempX;
                finalY = tempY;
            }

            grid_board[finalX, finalY] = current_player;
            SpriteRenderer spriteRenderer = grid_cell[finalX, finalY].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = move_object[current_player - 1];

            for (int i = 0; i < 7; i++)
            {
                if (emptyCells[i] == new Vector2Int(finalX, finalY))
                {
                    emptyCells[i].x--;
                    if (emptyCells[i].x >= 0)
                    {
                        grid_cell[emptyCells[i].x, emptyCells[i].y].GetComponent<SpriteRenderer>().sprite = possible_move;
                    }
                    break;
                }
            }


            int val = CheckWinner(grid_board);
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
        else
        {
            AI_Turn_Easy();
        }
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


    public int CheckWinnerTemp(int[,] grid)
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);

        // Check rows
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j <= numCols - 4; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i, j + 1] &&
                    symbol == grid[i, j + 2] &&
                    symbol == grid[i, j + 3])
                {
                    return symbol; // winner found
                }
            }
        }

        // Check columns
        for (int i = 0; i <= numRows - 4; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i + 1, j] &&
                    symbol == grid[i + 2, j] &&
                    symbol == grid[i + 3, j])
                {
                    return symbol; // winner found
                }
            }
        }

        // Check diagonal (top-left to bottom-right)
        for (int i = 0; i <= numRows - 4; i++)
        {
            for (int j = 0; j <= numCols - 4; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i + 1, j + 1] &&
                    symbol == grid[i + 2, j + 2] &&
                    symbol == grid[i + 3, j + 3])
                {
                    return symbol; // winner found
                }
            }
        }

        // Check diagonal (top-right to bottom-left)
        for (int i = 0; i <= numRows - 4; i++)
        {
            for (int j = 3; j < numCols; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i + 1, j - 1] &&
                    symbol == grid[i + 2, j - 2] &&
                    symbol == grid[i + 3, j - 3])
                {
                    return symbol; // winner found
                }
            }
        }

        // No winner found, check for draw
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (grid[i, j] == 0) return 0; // there is at least one empty cell, game is not finished
            }
        }

        // All cells are occupied, game is a draw
        return -1;
    }


    public int CheckWinner(int[,] grid)
    {
        int numRows = grid.GetLength(0);
        int numCols = grid.GetLength(1);

        // Check rows
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j <= numCols - 4; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i, j + 1] &&
                    symbol == grid[i, j + 2] &&
                    symbol == grid[i, j + 3])
                {
                    drawLine(grid_cell[i,j], grid_cell[i, j+3]);
                    return symbol; // winner found
                }
            }
        }

        // Check columns
        for (int i = 0; i <= numRows - 4; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i + 1, j] &&
                    symbol == grid[i + 2, j] &&
                    symbol == grid[i + 3, j])
                {
                    drawLine(grid_cell[i, j], grid_cell[i + 3, j]);
                    return symbol; // winner found
                }
            }
        }

        // Check diagonal (top-left to bottom-right)
        for (int i = 0; i <= numRows - 4; i++)
        {
            for (int j = 0; j <= numCols - 4; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i + 1, j + 1] &&
                    symbol == grid[i + 2, j + 2] &&
                    symbol == grid[i + 3, j + 3])
                {
                    drawLine(grid_cell[i, j], grid_cell[i + 3, j + 3]);
                    return symbol; // winner found
                }
            }
        }

        // Check diagonal (top-right to bottom-left)
        for (int i = 0; i <= numRows - 4; i++)
        {
            for (int j = 3; j < numCols; j++)
            {
                int symbol = grid[i, j];
                if (symbol == 0) continue; // empty cell, skip
                if (symbol == grid[i + 1, j - 1] &&
                    symbol == grid[i + 2, j - 2] &&
                    symbol == grid[i + 3, j - 3])
                {
                    drawLine(grid_cell[i, j], grid_cell[i + 3, j - 3]);
                    return symbol; // winner found
                }
            }
        }

        // No winner found, check for draw
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                if (grid[i, j] == 0) return 0; // there is at least one empty cell, game is not finished
            }
        }

        // All cells are occupied, game is a draw
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

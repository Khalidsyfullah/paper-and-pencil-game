using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class Tictactoe_44 : MonoBehaviour
{

    int sound = 1, vibration = 1, soundSettings = 1;
    public AudioSource audioSource;
    public AudioClip audioClip1, audioClip2, audioClip3, audioClip4, audioClip5;


    public GameObject mainParent;
    GameObject[,] grid_cell = new GameObject[4, 4];
    int[,] grid_board = new int[4, 4];

    int current_player = 1;
    public Sprite[] move_object = new Sprite[2];
    int settings = 0;
    public GameObject parentObject;
    public GameObject pauseMenu;
    public GameObject resumeMenu;
    public Button resumeButton, restartButton, exitButton, cancelButton, soundOn, soundOff, vibrationOn, vibrationOff;
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


        soundOn.onClick.AddListener(soundOnclicked);
        soundOff.onClick.AddListener(soundOffclicked);
        vibrationOn.onClick.AddListener(vibrationOnclicked);
        vibrationOff.onClick.AddListener(vibratioffOnclicked);
        soundSettings = PlayerPrefs.GetInt("soundStatus", 1);
        vibration = PlayerPrefs.GetInt("vibrationStatus", 1);
        sound = PlayerPrefs.GetInt("soundSettings", 1);
        playSound(0);

        string name = "Artboard 1_25";
        for (int i = 0; i < 16; i++)
        {
            string temp = name + " (" + i + ")";
            int t1 = i / 4;
            int t2 = i % 4;
            Transform childTransform = mainParent.transform.Find(temp);
            grid_cell[t1, t2] = childTransform.gameObject;
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                grid_board[i, j] = 0;
            }
        }
        settings = PlayerPrefs.GetInt("Tictactoe44", 3);

        current_player = UnityEngine.Random.Range(1, 3);

        if (settings == 0)
        {
            if (current_player == 2)
            {
                turning_text.color = Color.red;
                turning_text.text = "RED's Turn";
            }
            else
            {
                turning_text.color = Color.blue;
                turning_text.text = "Blue's Turn";
            }
        }
        else
        {
            if (current_player == 2)
            {
                turning_text.color = Color.red;
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
                turning_text.color = Color.blue;
                turning_text.text = "Your Turn";
            }
        }
    }



    void gameEndSound()
    {
        if (sound == 1)
        {
            audioSource.PlayOneShot(audioClip5);
        }
        else if (vibration == 1)
        {
            VibrationManager.Vibrate();
        }
    }



    void playSound(int num)
    {
        if (sound != 1) return;
        if (num == 0)
        {
            audioSource.PlayOneShot(audioClip1);
        }
        else if (num == 1)
        {
            audioSource.PlayOneShot(audioClip2);
        }
        else
        {
            audioSource.PlayOneShot(audioClip3);
        }
    }

    void playVibration()
    {
        if (vibration != 1) return;
        VibrationManager.Vibrate(vibration);
    }


    void soundOnclicked()
    {
        sound = 1;
        playButtonClickSound();
        soundOff.GetComponent<Image>().color = Color.white;
        soundOn.GetComponent<Image>().color = Color.green;
    }

    void soundOffclicked()
    {
        sound = 2;
        soundOn.GetComponent<Image>().color = Color.white;
        soundOff.GetComponent<Image>().color = Color.green;
    }

    void vibrationOnclicked()
    {
        playButtonClickSound();
        vibration = 1;
        vibrationOff.GetComponent<Image>().color = Color.white;
        vibrationOn.GetComponent<Image>().color = Color.green;
    }

    void vibratioffOnclicked()
    {
        playButtonClickSound();
        vibration = 2;
        vibrationOn.GetComponent<Image>().color = Color.white;
        vibrationOff.GetComponent<Image>().color = Color.green;
    }


    void playButtonClickSound()
    {
        if (sound != 1) return;
        audioSource.PlayOneShot(audioClip4);
    }


    void soundManagerOperation()
    {
        if (current_player == 1)
        {
            if (soundSettings == 1)
            {
                playSound(1);
            }
            else if (soundSettings == 2)
            {
                playVibration();
            }
            else if (soundSettings == 3)
            {
                playVibration();
            }
            else if (soundSettings == 4)
            {
                playSound(1);
            }
            else
            {
                playVibration();
                playSound(1);
            }
        }

        else
        {
            if (soundSettings == 1)
            {
                playVibration();
            }
            else if (soundSettings == 2)
            {
                playSound(1);
            }
            else if (soundSettings == 3)
            {
                playVibration();
            }
            else if (soundSettings == 4)
            {
                playSound(2);
            }
            else
            {
                playVibration();
                playSound(2);
            }
        }
    }

    void updatePopupPrefs()
    {
        PlayerPrefs.SetInt("soundSettings", sound);
        PlayerPrefs.SetInt("vibrationStatus", vibration);
    }


    void updatePopupPanel()
    {
        if (sound == 1)
        {
            soundOff.GetComponent<Image>().color = Color.white;
            soundOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            soundOn.GetComponent<Image>().color = Color.white;
            soundOff.GetComponent<Image>().color = Color.green;
        }

        if (vibration == 1)
        {
            vibrationOff.GetComponent<Image>().color = Color.white;
            vibrationOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            vibrationOn.GetComponent<Image>().color = Color.white;
            vibrationOff.GetComponent<Image>().color = Color.green;
        }
    }




    void onExitClicked()
    {
        playButtonClickSound();
        SceneManager.LoadScene("parentpage");
    }

    void onRestartClicked()
    {
        playButtonClickSound();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void onResumeClicked()
    {
        playButtonClickSound();
        updatePopupPrefs();
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

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (grid_cell[i, j] == gamer)
                            {
                                if (grid_board[i, j] == 0)
                                {
                                    grid_board[i, j] = current_player;
                                    soundManagerOperation();
                                    spriteRenderer.sprite = move_object[current_player - 1];
                                    int val = CheckWinnerisReady(grid_board);
                                    if (val == 0)
                                    {
                                        current_player = (current_player == 1) ? 2 : 1;
                                        if (current_player == 2)
                                        {
                                            turning_text.color = Color.red;
                                            turning_text.text = "AI's Turn";
                                        }
                                        else
                                        {
                                            turning_text.color = Color.blue;
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
                                    else if (val == -1)
                                    {
                                        gameEndSound();
                                        gameFinish = true;
                                        StartCoroutine(showWinner(1));
                                        return;
                                    }
                                    else
                                    {
                                        gameEndSound();
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

                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 4; j++)
                        {
                            if (grid_cell[i, j] == gamer)
                            {
                                if (grid_board[i, j] == 0)
                                {
                                    grid_board[i, j] = current_player;
                                    spriteRenderer.sprite = move_object[current_player - 1];
                                    soundManagerOperation();
                                    int val = CheckWinnerisReady(grid_board);
                                    if (val == 0)
                                    {
                                        current_player = (current_player == 1) ? 2 : 1;
                                        if (current_player == 2)
                                        {
                                            turning_text.color = Color.red;
                                            turning_text.text = "RED's Turn";
                                        }
                                        else
                                        {
                                            turning_text.color = Color.blue;
                                            turning_text.text = "Blue's Turn";
                                        }
                                        return;
                                    }
                                    else if (val == -1)
                                    {
                                        gameEndSound();
                                        gameFinish = true;
                                        StartCoroutine(showWinner(1));
                                        return;
                                    }
                                    else
                                    {
                                        gameEndSound();
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

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseGame();
        }
    }

    void AI_Turn_Easy()
    {
        int coun = 0;
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid_board[i, j] == 0)
                {
                    coun++;
                }
            }
        }

        int indexc = UnityEngine.Random.Range(0, coun);
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid_board[i, j] == 0)
                {
                    if (indexc == 0)
                    {
                        grid_board[i, j] = current_player;
                        SpriteRenderer spriteRenderer = grid_cell[i, j].GetComponent<SpriteRenderer>();
                        spriteRenderer.sprite = move_object[current_player - 1];
                        soundManagerOperation();
                        int val = CheckWinnerisReady(grid_board);
                        if (val == 0)
                        {
                            current_player = (current_player == 1) ? 2 : 1;
                            if (current_player == 2)
                            {
                                turning_text.color = Color.red;
                                turning_text.text = "AI's Turn";
                            }
                            else
                            {
                                turning_text.color = Color.blue;
                                turning_text.text = "Your Turn";
                            }
                        }
                        else if (val == -1)
                        {
                            gameEndSound();
                            gameFinish = true;
                            StartCoroutine(showWinner(1));
                        }
                        else
                        {
                            gameEndSound();
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
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid_board[i, j] == 0)
                {
                    finalx = i;
                    finaly = j;
                    break;
                }
            }
        }

        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (grid_board[i, j] == 0)
                {
                    temp = j;
                    cont0++;
                }
                else if (grid_board[i, j] == 1)
                {
                    cont1++;
                }
                else if (grid_board[i, j] == 2)
                {
                    cont2++;
                }
            }
            if ((cont1 == 3 || cont2 == 3) && cont0 == 1)
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
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
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
                if ((cont1 == 3 || cont2 == 3) && cont0 == 1)
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
            for (int i = 0; i < 4; i++)
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
            }
                if ((cont1 == 3 || cont2 == 3) && cont0 == 1)
                {
                    finalx = temp;
                    finaly = temp;
                    finish = true;
                    
                }
            
            cont0 = 0;
            cont1 = 0;
            cont2 = 0;
        }

        if (!finish)
        {
            for (int i = 0; i < 4; i++)
            {

                if (grid_board[i, 3 - i] == 0)
                {
                    temp = i;
                    cont0++;
                }
                else if (grid_board[i, 3 - i] == 1)
                {
                    cont1++;
                }
                else if (grid_board[i, 3 - i] == 2)
                {
                    cont2++;
                }
            }
                if ((cont1 == 3 || cont2 == 3) && cont0 == 1)
                {
                    finalx = temp;
                    finaly = 3 - temp;
                    finish = true;
                    
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
            soundManagerOperation();
            int val = CheckWinnerisReady(grid_board);
            if (val == 0)
            {
                current_player = (current_player == 1) ? 2 : 1;
                if (current_player == 2)
                {
                    turning_text.color = Color.red;
                    turning_text.text = "AI's Turn";
                }
                else
                {
                    turning_text.color = Color.blue;
                    turning_text.text = "Your Turn";
                }
            }
            else if (val == -1)
            {
                gameEndSound();
                gameFinish = true;
                StartCoroutine(showWinner(1));
            }
            else
            {
                gameEndSound();
                gameFinish = true;
                StartCoroutine(showWinner(3));
            }
        }

    }


    public Vector2Int GetBestMove(int[,] board, int p)
    {
        int m = board.GetLength(0);
        int n = board.GetLength(1);

        // Check for winning move
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] == 0)
                {
                    board[i, j] = 2;
                    if (CheckWin(board, p, i, j))
                    {
                        board[i, j] = 0;
                        return new Vector2Int(i, j);
                    }
                    board[i, j] = 0;
                }
            }
        }

        // Check for blocking move
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] == 0)
                {
                    board[i, j] = 1;
                    if (CheckWin(board, p, i, j))
                    {
                        board[i, j] = 2;
                        return new Vector2Int(i, j);
                    }
                    board[i, j] = 0;
                }
            }
        }

        // Check for creating a line of length p-1, if possible
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] == 0)
                {
                    // Check row
                    int rowSum = 0;
                    for (int k = 0; k < p - 1 && j + k < n; k++)
                    {
                        rowSum += board[i, j + k];
                    }
                    if (rowSum == p - 2 && (j == 0 || board[i, j - 1] == 0) && (j + p - 1 == n || board[i, j + p - 1] == 0))
                    {
                        board[i, j] = 2;
                        return new Vector2Int(i, j);
                    }

                    // Check column
                    int colSum = 0;
                    for (int k = 0; k < p - 1 && i + k < m; k++)
                    {
                        colSum += board[i + k, j];
                    }
                    if (colSum == p - 2 && (i == 0 || board[i - 1, j] == 0) && (i + p - 1 == m || board[i + p - 1, j] == 0))
                    {
                        board[i, j] = 2;
                        return new Vector2Int(i, j);
                    }

                    // Check diagonal (top-left to bottom-right)
                    int diagonalSum = 0;
                    for (int k = 0; k < p - 1 && i + k < m && j + k < n; k++)
                    {
                        diagonalSum += board[i + k, j + k];
                    }
                    if (diagonalSum == p - 2 && (i == 0 || j == 0 || board[i - 1, j - 1] == 0) && (i + p - 1 == m || j + p - 1 == n || board[i + p - 1, j + p - 1] == 0))
                    {
                        board[i, j] = 2;
                        return new Vector2Int(i, j);
                    }
                    // Check diagonal (bottom-left to top-right)
                    diagonalSum = 0;
                    for (int k = 0; k < p - 1 && i - k >= 0 && j + k < n; k++)
                    {
                        diagonalSum += board[i - k, j + k];
                    }
                    if (diagonalSum == p - 2 && (i == m - 1 || j == 0 || board[i + 1, j - 1] == 0) && (i - p + 1 < 0 || j + p - 1 == n || board[i - p + 1, j + p - 1] == 0))
                    {
                        board[i, j] = 2;
                        return new Vector2Int(i, j);
                    }

                }
            }
        }

        // No winning move or blocking move, choose random empty cell
        List<Vector2Int> emptyCells = new List<Vector2Int>();
        for (int i = 0; i < m; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (board[i, j] == 0)
                {
                    emptyCells.Add(new Vector2Int(i, j));
                }
            }
        }
        if (emptyCells.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, emptyCells.Count);
            return emptyCells[randomIndex];
        }

        // Board is full
        return new Vector2Int(-1, -1);

    }


    private bool CheckWin(int[,] board, int p, int i, int j)
    {
        int m = board.GetLength(0);
        int n = board.GetLength(1);

        // Check row
        int rowSum = 0;
        for (int k = 0; k < p && j + k < n; k++)
        {
            rowSum += board[i, j + k];
        }
        if (rowSum == p)
        {
            return true;
        }

        // Check column
        int colSum = 0;
        for (int k = 0; k < p && i + k < m; k++)
        {
            colSum += board[i + k, j];
            if (colSum > p) // Early exit
            {
                return false;
            }
        }
        if (colSum == p)
        {
            return true;
        }

        // Check diagonal (top-left to bottom-right)
        int diagonalSum = 0;
        for (int k = 0; k < p && i + k < m && j + k < n; k++)
        {
            diagonalSum += board[i + k, j + k];
            if (diagonalSum > p) // Early exit
            {
                return false;
            }
        }
        if (diagonalSum == p)
        {
            return true;
        }

        // Check diagonal (bottom-left to top-right)
        diagonalSum = 0;
        for (int k = 0; k < p && i - k >= 0 && j + k < n; k++)
        {
            diagonalSum += board[i - k, j + k];
            if (diagonalSum > p) // Early exit
            {
                return false;
            }
        }
        if (diagonalSum == p)
        {
            return true;
        }

        // No win
        return false;
    }




    void AI_Turn_Medium()
    {
        Vector2Int num = GetBestMove(grid_board, 4);

        int bestMoveX = num.x;
        int bestMoveY = num.y;


        grid_board[bestMoveX, bestMoveY] = current_player;
        SpriteRenderer spriteRenderer = grid_cell[bestMoveX, bestMoveY].GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = move_object[current_player - 1];
        soundManagerOperation();
        int val = CheckWinnerisReady(grid_board);
        if (val == 0)
        {
            current_player = (current_player == 1) ? 2 : 1;
            if (current_player == 2)
            {
                turning_text.color = Color.red;
                turning_text.text = "AI's Turn";
            }
            else
            {
                turning_text.color = Color.blue;
                turning_text.text = "Your Turn";
            }
        }
        else if (val == -1)
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(1));
        }
        else
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(3));
        }
    }

    IEnumerator showWinner(int ridoy)
    {
        yield return new WaitForSeconds(2.5f);

        bool f = GoogleMobileAdsScript.ShowRewardedAd();
        if (!f)
        {
            GoogleMobileAdsScript.ShowAd();
        }

        if (ridoy == 1)
        {
            resumeMenu.SetActive(true);
            text_pop.text = "Match \nDraw!";
        }
        else if (ridoy == 2)
        {
            resumeMenu.SetActive(true);
            if (current_player == 1)
            {
                text_pop.text = "Blue is \nWinner!";
            }
            else
            {
                text_pop.text = "Red is \nWinner!";
            }
        }
        else if (ridoy == 3)
        {
            resumeMenu.SetActive(true);
            if (current_player == 1)
            {
                text_pop.text = "You've \nWon!";
            }
            else
            {
                text_pop.text = "You've \nLost!";
            }
        }
    }

    int CheckWinnerisReady(int[,] board)
    {
        // Check rows
        for (int i = 0; i < 4; i++)
        {
            if (board[i, 0] == board[i, 1] && board[i, 1] == board[i, 2] && board[i, 2] == board[i, 3] && board[i, 0] != 0)
            {
                drawLine(grid_cell[i, 0], grid_cell[i, 3]);

                if (board[i, 0] == 1)
                {
                    return 1; // Player wins
                }
                else if (board[i, 0] == 2)
                {
                    return 2; // AI wins
                }
            }
        }

        // Check columns
        for (int j = 0; j < 4; j++)
        {
            if (board[0, j] == board[1, j] && board[1, j] == board[2, j] && board[2, j] == board[3, j] && board[0, j] != 0)
            {
                drawLine(grid_cell[0, j], grid_cell[3, j]);

                if (board[0, j] == 1)
                {
                    return 1; // Player wins
                }
                else if (board[0, j] == 2){
                    return 2; // AI wins
                }
            }
        }

        // Check diagonals
        if (board[0, 0] == board[1, 1] && board[1, 1] == board[2, 2] && board[2, 2] == board[3, 3] && board[0, 0] != 0)
        {
            drawLine(grid_cell[0, 0], grid_cell[3, 3]);

            if (board[0, 0] == 1)
            {
                return 1; // Player wins
            }
            else if (board[0, 0] == 2)
            {
                return 2; // AI wins
            }
        }

        if (board[0, 3] == board[1, 2] && board[1, 2] == board[2, 1] && board[2, 1] == board[3, 0] && board[0, 3] != 0)
        {
            drawLine(grid_cell[0, 3], grid_cell[3, 0]);

            if (board[0, 3] == 1)
            {
                return 1; // Player wins
            }
            else if (board[0, 3] == 2)
            {
                return 2; // AI wins
            }
        }

        // Check if there are any empty spaces left
        for (int i = 0; i < 4; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (board[i, j] == 0)
                {
                    return 0; // Game not over yet
                }
            }
        }

        // Game is a tie
        return -1;
    }

    void onPauseGame()
    {
        if (isPaused) return;
        playButtonClickSound();
        if (gameFinish) return;
        isPaused = true;
        updatePopupPanel();
        pauseMenu.SetActive(true);
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

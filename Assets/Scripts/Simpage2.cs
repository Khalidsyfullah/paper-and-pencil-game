using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Simpage2 : MonoBehaviour
{
    int sound = 1, vibration = 1, soundSettings = 1;
    public AudioSource audioSource;
    public AudioClip audioClip1, audioClip2, audioClip3, audioClip4, audioClip5;


    GameObject[,] grid_value = new GameObject[20, 3];
    int[,] grid_num = new int[20,3];
    Color[] df_val = { Color.blue, Color.red };
    int current_player = 0;
    int settings = 0;
    int[,] triangles = new int[20, 3] {{0, 13, 5},{0, 1, 2},{0, 3, 8},{0, 7, 12},{13, 1, 10},{13, 3, 11},{13, 6, 7},{1, 3, 4},{1, 7, 14},{3, 7, 9},{5, 2, 10},{5, 8, 11},{5, 6, 12},
    {2, 4, 8},{2, 12, 14},{8, 12, 9}, {10, 4, 11},{10, 6, 14},{11, 6, 9},{4, 14, 9}};
    public GameObject prt;
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


        for (int i = 0; i < 20; i++)
        {
            for(int j=0; j<3; j++)
            {
                grid_num[i,j] = 0;
            }
            
        }

        string name1 = "Artboard 1@1.5xsim_0";

        for (int i = 0; i < 20; i++)
        {
            for(int j=0; j<3; j++)
            {
                string temp = name1 + " (" + triangles[i,j] + ")";
                Transform childTransform = prt.transform.Find(temp);
                grid_value[i,j] = childTransform.gameObject;
            }
        }

        settings = PlayerPrefs.GetInt("simpage2", 0);

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

    void onPauseGame()
    {
        if (isPaused) return;
        playButtonClickSound();
        if (gameFinish) return;
        isPaused = true;
        updatePopupPanel();
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

                if(settings == 0)
                {
                    for (int i = 0; i < 20; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            if (grid_value[i, j] == gamer)
                            {
                                if (grid_num[i, j] != 0)
                                {
                                    return;
                                }

                                if (CheckifWinner(gamer))
                                {
                                    gameEndSound();
                                    gameFinish = true;
                                    StartCoroutine(showWinner(2));
                                    return;
                                }

                                if (checkifEnd())
                                {
                                    gameEndSound();
                                    gameFinish = true;
                                    StartCoroutine(showWinner(1));
                                    return;
                                }

                                soundManagerOperation();
                                current_player = (current_player == 1) ? 2 : 1;
                                if (current_player == 2)
                                {
                                    turning_text.color = Color.red;
                                    turning_text.text = "RED's Turn";
                                }
                                else
                                {
                                    turning_text.color= Color.blue;
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

                    for(int i=0; i<20; i++)
                    {
                        for(int j=0; j<3; j++)
                        {
                            if(grid_value[i,j] == gamer)
                            {
                                if(grid_num[i, j] != 0)
                                {
                                    return;
                                }

                                if (CheckifWinner(gamer))
                                {
                                    gameEndSound();
                                    gameFinish = true;
                                    StartCoroutine(showWinner(3));
                                    return;
                                }

                                if (checkifEnd())
                                {
                                    gameEndSound();
                                    gameFinish = true;
                                    StartCoroutine(showWinner(1));
                                    return;
                                }
                                soundManagerOperation();
                                move_AI();
                                return;

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

    void move_AI()
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
        for (int i = 0; i < 20; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if(grid_num[i,j] == 0)
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
        for(int i=0; i<20; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(grid_num[i,j] == 0)
                {
                    emptyList.Add(new Vector2Int(i, j));
                }
            }
        }

        int rand = UnityEngine.Random.Range(0, emptyList.Count);
        soundManagerOperation();
        GameObject gm = grid_value[emptyList[rand].x, emptyList[rand].y];
        if (CheckifWinner(gm))
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(3));
            return;
        }

        if (checkifEnd())
        {
            gameEndSound();
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
        int movX = -1, movY = -1, tempX = -1, tempY=-1, blockX=-1, blockY=-1;
        for (int i = 0; i < 20; i++)
        {
            int coun1 = 0, coun2 = 0, coun3 = 0;
            for (int j = 0; j < 3; j++)
            {
                if(grid_num[i,j] == current_player)
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

            if(coun2 == 2 && coun1 == 1)
            {
                movX = tempX;
                movY = tempY;
                break;
            }
            else if(coun3 == 2 && coun1 == 1)
            {
                blockX = tempX;
                blockY = tempY;
            }

        }

        int finalX, finalY;
        if((movX!= -1 && movY!= -1) || (blockX!= -1 && blockY!= -1))
        {
            if(movX != -1 && movY != -1)
            {
                finalX = movX;
                finalY = movY;
            }
            else
            {
                finalX = blockX;
                finalY = blockY;
            }
            soundManagerOperation();
            GameObject gm = grid_value[finalX, finalY];
            if (CheckifWinner(gm))
            {
                gameEndSound();
                gameFinish = true;
                StartCoroutine(showWinner(3));
                return;
            }

            if (checkifEnd())
            {
                gameEndSound();
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
        for(int i=0; i<20; i++)
        {
            for(int j=0; j<3; j++)
            {
                if(grid_value[i,j] == gm)
                {
                    grid_num[i, j] = current_player;
                    grid_value[i, j].GetComponent<SpriteRenderer>().color = df_val[current_player - 1];
                }
            }
        }

        for(int i=0; i<20; i++)
        {
            int count = 0;
            for(int j=0; j<3; j++)
            {
                if(grid_num[i,j] == current_player)
                {
                    count++;
                }
            }

            if(count == 3)
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

}

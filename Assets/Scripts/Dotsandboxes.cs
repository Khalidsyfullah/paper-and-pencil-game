using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Dotsandboxes : MonoBehaviour
{
    int sound = 1, vibration = 1, soundSettings = 1;
    public AudioSource audioSource;
    public AudioClip audioClip1, audioClip2, audioClip3, audioClip4, audioClip5;
    public Button soundOn, soundOff, vibrationOn, vibrationOff;


    public GameObject parent_object;
    GameObject[] horizontal_object = new GameObject[66];
    int[] horizontal_value = new int[66];
    GameObject[] vertical_object = new GameObject[70];
    int[] vertical_value = new int[70];
    GameObject[] boxes = new GameObject[60];
    GameObject[,] all_game = new GameObject[60, 5];
    int[,] all_gamer_value = new int[60, 5];
    public Sprite[] Sprite_red = new Sprite[2];
    public Sprite[] Sprite_blue = new Sprite[2];
    public Sprite[] box_sprite = new Sprite[2];
    int current_player = 1;
    public GameObject parentObject;

    public GameObject bground;
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
    int count = 0;

    int settings = 0;

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
        score1val = score1.GetComponent<TextMeshPro>();
        score2val = score2.GetComponent<TextMeshPro>();


        soundOn.onClick.AddListener(soundOnclicked);
        soundOff.onClick.AddListener(soundOffclicked);
        vibrationOn.onClick.AddListener(vibrationOnclicked);
        vibrationOff.onClick.AddListener(vibratioffOnclicked);
        soundSettings = PlayerPrefs.GetInt("soundStatus", 1);
        vibration = PlayerPrefs.GetInt("vibrationStatus", 1);
        sound = PlayerPrefs.GetInt("soundSettings", 1);
        playSound(0);


        string name1 = "Artboard 1mdpi_6";
        string name2 = "Artboard 1mdpi_8";
        string name3 = "Artboard 1mdpi_9";
        for(int i=0; i<66; i++)
        {
            string temp = name1 + " ("+i+")";
            Transform childTransform = parent_object.transform.Find(temp);
            horizontal_object[i] = childTransform.gameObject;
        }

        for (int i = 0; i < 70; i++)
        {
            string temp = name2 + " (" + i + ")";
            Transform childTransform = parent_object.transform.Find(temp);
            vertical_object[i] = childTransform.gameObject;
        }

        for (int i = 0; i < 60; i++)
        {
            string temp = name3 + " (" + i + ")";
            Transform childTransform = parent_object.transform.Find(temp);
            boxes[i] = childTransform.gameObject;
        }

        for(int i=0; i<60; i++)
        {
            all_game[i, 4] = boxes[i];
            int t1 = i % 6;
            int t2 = i / 6;
            all_game[i, 0] = horizontal_object[i];
            all_game[i, 1] = horizontal_object[i+6];
            all_game[i, 2] = vertical_object[t2 * 7 + t1];
            all_game[i, 3] = vertical_object[t2 * 7 + t1+ 1];
        }

        for(int i=0; i < 70; i++)
        {
            if (i < 66)
            {
                horizontal_value[i] = -1;
            }
            vertical_value[i] = -1;
        }

        for (int i = 0; i < 60; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                all_gamer_value[i, j] = 0;
            }
        }

        settings = PlayerPrefs.GetInt("Dotsandboxes", 0);

        current_player = UnityEngine.Random.Range(1, 3);

        if (settings == 0)
        {
            score1val.text = "Blue's Score: 0";
            score2val.text = "RED's Score: 0";
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
            score1val.text = "Your Score: 0";
            score2val.text = "AI's Score: 0";
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
        SceneManager.LoadSceneAsync("parentpage");
    }

    void onRestartClicked()
    {
        playButtonClickSound();
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
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
            count++;
            if (count > 20)
            {
                if (GoogleMobileAdsScript.ShowAd())
                {
                    count = -20;
                }
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
            if (hit.collider != null)
            {
                GameObject gamer = hit.collider.gameObject;
                if(settings == 0)
                {
                    SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();
                    int index = System.Array.IndexOf(horizontal_object, gamer);

                    if (index != -1)
                    {
                        if (horizontal_value[index] != -1)
                        {
                            return;
                        }

                        horizontal_value[index] = current_player;
                        if (current_player == 1)
                        {
                            spriteRenderer.sprite = Sprite_blue[1];
                        }
                        else
                        {
                            spriteRenderer.sprite = Sprite_red[1];
                        }

                        for (int i = 0; i < 60; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (all_game[i, j] == gamer)
                                {
                                    all_gamer_value[i, j] = -1;
                                }
                            }
                        }
                        soundManagerOperation();
                        if (!checkifWin() && !checkifEnd())
                        {

                            current_player = (current_player == 2) ? 1 : 2;
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
                        else if (checkifEnd())
                        {
                            gameEndSound();
                            gameFinish = true;
                            StartCoroutine(showWinner(1));
                            return;
                        }
                        return;

                    }
                    else
                    {
                        index = System.Array.IndexOf(vertical_object, gamer);
                        if (index != -1)
                        {
                            if (vertical_value[index] != -1)
                            {
                                return;
                            }
                            vertical_value[index] = current_player;

                            if (current_player == 1)
                            {
                                spriteRenderer.sprite = Sprite_blue[0];
                            }
                            else
                            {
                                spriteRenderer.sprite = Sprite_red[0];
                            }

                            for (int i = 0; i < 60; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    if (all_game[i, j] == gamer)
                                    {
                                        all_gamer_value[i, j] = -1;
                                    }
                                }
                            }
                            soundManagerOperation();
                            if (!checkifWin() && !checkifEnd())
                            {
                                current_player = (current_player == 2) ? 1 : 2;
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

                            else if (checkifEnd())
                            {
                                gameEndSound();
                                gameFinish = true;
                                StartCoroutine(showWinner(1));
                                return;
                            }

                        }
                        return;
                    }
                }

                else
                {
                    if (current_player == 2) return;

                    SpriteRenderer spriteRenderer = gamer.GetComponent<SpriteRenderer>();
                    int index = System.Array.IndexOf(horizontal_object, gamer);

                    if (index != -1)
                    {
                        if (horizontal_value[index] != -1)
                        {
                            return;
                        }

                        horizontal_value[index] = current_player;
                        if (current_player == 1)
                        {
                            spriteRenderer.sprite = Sprite_blue[1];
                        }
                        else
                        {
                            spriteRenderer.sprite = Sprite_red[1];
                        }

                        for (int i = 0; i < 60; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                if (all_game[i, j] == gamer)
                                {
                                    all_gamer_value[i, j] = -1;
                                }
                            }
                        }
                        soundManagerOperation();
                        if (!checkifWinAI() && !checkifEnd())
                        {

                            current_player = (current_player == 2) ? 1 : 2;
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
                            return;
                        }
                        else if (checkifEnd())
                        {
                            gameEndSound();
                            gameFinish = true;
                            StartCoroutine(showWinner(2));
                            return;
                        }
                        return;

                    }
                    else
                    {
                        index = System.Array.IndexOf(vertical_object, gamer);
                        if (index != -1)
                        {
                            if (vertical_value[index] != -1)
                            {
                                return;
                            }
                            vertical_value[index] = current_player;

                            if (current_player == 1)
                            {
                                spriteRenderer.sprite = Sprite_blue[0];
                            }
                            else
                            {
                                spriteRenderer.sprite = Sprite_red[0];
                            }

                            for (int i = 0; i < 60; i++)
                            {
                                for (int j = 0; j < 4; j++)
                                {
                                    if (all_game[i, j] == gamer)
                                    {
                                        all_gamer_value[i, j] = -1;
                                    }
                                }
                            }
                            soundManagerOperation();
                            if (!checkifWin() && !checkifEnd())
                            {
                                current_player = (current_player == 2) ? 1 : 2;
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
                                return;
                            }

                            else if (checkifEnd())
                            {   
                                gameEndSound();
                                gameFinish = true;
                                StartCoroutine(showWinner(2));
                                return;
                            }

                        }
                        return;
                    }
                }
            }
        }

        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            onPauseGame();
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
            string val = "Blue's Score: " + scorenum1 + "\nRed's Score: " + scorenum2;
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
            string val = "Your Score: " + scorenum1 + "\nAI's Score: " + scorenum2;
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

    bool checkifEnd()
    {
        for(int i=0; i<60; i++)
        {
            if(all_gamer_value[i, 4] == 0)
            {
                return false;
            }
        }
        return true;
    }

    void moveAI(GameObject gamer)
    {
        for (int i = 0; i < 60; i++)
        {
            for (int j = 0; j < 4; j++)
            {
                if (all_game[i, j] == gamer)
                {
                    all_gamer_value[i, j] = -1;
                }
            }
        }

        soundManagerOperation();
        if (!checkifWin() && !checkifEnd())
        {
            current_player = (current_player == 2) ? 1 : 2;
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

        else if (checkifEnd())
        {
            gameEndSound();
            gameFinish = true;
            StartCoroutine(showWinner(2));
            return;
        }

        else
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


    bool checkifWin()
    {
        bool flag = false;
        for(int i=0; i<60; i++)
        {

            if (all_gamer_value[i, 0] == -1 && all_gamer_value[i, 1] == -1 && all_gamer_value[i, 2] == -1 && all_gamer_value[i, 3] == -1 
                && all_gamer_value[i, 4]== 0)
            {
                flag = true;
                if(current_player == 1)
                {
                    scorenum1++;
                    score1val.text = "Blue's Score: "+scorenum1;
                }
                else
                {
                    scorenum2++;
                    score2val.text = "Red's Score: " + scorenum2;
                }
                all_gamer_value[i, 4] = -1;
                SpriteRenderer spriteRenderer = all_game[i, 4].GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = box_sprite[current_player-1];
            }
        }
        return flag;
    }


    void AI_Turn_Easy()
    {
        List<Vector2Int> emptyCells1 = new List<Vector2Int>();
        List<Vector2Int> emptyCells2 = new List<Vector2Int>();
        for (int i=0; i<60; i++)
        {
            if(all_gamer_value[i,0] == 0)
            {
                emptyCells1.Add(new Vector2Int(i,0));
            }
            if (all_gamer_value[i, 1] == 0)
            {
                emptyCells1.Add(new Vector2Int(i,1));
            }
            if (all_gamer_value[i, 2] == 0)
            {
                emptyCells2.Add(new Vector2Int(i,2));
            }
            if (all_gamer_value[i, 3] == 0)
            {
                emptyCells2.Add(new Vector2Int(i, 3));
            }
        }

        int randomIndex = UnityEngine.Random.Range(0, 2);
        GameObject gmo;
        if(randomIndex == 0)
        {
            int randval = UnityEngine.Random.Range(0, emptyCells1.Count);
            gmo = all_game[emptyCells1[randval].x, emptyCells1[randval].y];
            for(int i=0; i<horizontal_object.Length; i++)
            {
                if(horizontal_object[i] == gmo)
                {
                    horizontal_value[i] = current_player;
                    break;
                }
            }

            if (current_player == 1)
            {
                gmo.GetComponent<SpriteRenderer>().sprite = Sprite_blue[1];
            }
            else
            {
                gmo.GetComponent<SpriteRenderer>().sprite = Sprite_red[1];
            }

        }
        else
        {
            int randval = UnityEngine.Random.Range(0, emptyCells2.Count);
            gmo = all_game[emptyCells2[randval].x, emptyCells2[randval].y];
            for (int i = 0; i < vertical_object.Length; i++)
            {
                if (vertical_object[i] == gmo)
                {
                    vertical_value[i] = current_player;
                }
            }

            if (current_player == 1)
            {
                gmo.GetComponent<SpriteRenderer>().sprite = Sprite_blue[0];
            }
            else
            {
                gmo.GetComponent<SpriteRenderer>().sprite = Sprite_red[0];
            }
        }

        moveAI(gmo);
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
        int indexX = -1, indexY = -1;
        int tempX = -1, tempY = -1;
        for(int i=0; i<60; i++)
        {
            int count = 0;
            for(int j=0; j<4; j++)
            {
                if(all_gamer_value[i,j] != 0)
                {
                    count++;
                }
                if(all_gamer_value[i,j] == 0)
                {
                    tempX = i;
                    tempY = j;
                }
            }
            if(count == 3 && all_gamer_value[i,4] == 0)
            {
                indexX = tempX;
                indexY = tempY;
                break;
            }
        }

        if(indexX!= -1 && indexY!= -1)
        {
            GameObject gmo = all_game[indexX, indexY];
            if(indexY < 2)
            {
                int index = System.Array.IndexOf(horizontal_object, gmo);
                if(index!= -1)
                {
                    horizontal_value[index] = current_player;
                }

                if (current_player == 1)
                {
                    gmo.GetComponent<SpriteRenderer>().sprite = Sprite_blue[1];
                }
                else
                {
                    gmo.GetComponent<SpriteRenderer>().sprite = Sprite_red[1];
                }
            }
            else
            {
                int index = System.Array.IndexOf(vertical_object, gmo);
                if (index != -1)
                {
                    vertical_value[index] = current_player;
                }

                if (current_player == 1)
                {
                    gmo.GetComponent<SpriteRenderer>().sprite = Sprite_blue[0];
                }
                else
                {
                    gmo.GetComponent<SpriteRenderer>().sprite = Sprite_red[0];
                }
            }
            moveAI(gmo);
        }

        else
        {
            AI_Turn_Easy();
        }


    }


    bool checkifWinAI()
    {
        bool flag = false;
        for (int i = 0; i < 60; i++)
        {

            if (all_gamer_value[i, 0] == -1 && all_gamer_value[i, 1] == -1 && all_gamer_value[i, 2] == -1 && all_gamer_value[i, 3] == -1
                && all_gamer_value[i, 4] == 0)
            {
                flag = true;
                if (current_player == 1)
                {
                    scorenum1++;
                    score1val.text = "Your Score: " + scorenum1;
                }
                else
                {
                    scorenum2++;
                    score2val.text = "AI's Score: " + scorenum2;
                }
                all_gamer_value[i, 4] = -1;
                SpriteRenderer spriteRenderer = all_game[i, 4].GetComponent<SpriteRenderer>();
                spriteRenderer.sprite = box_sprite[current_player - 1];
            }
        }
        return flag;
    }

}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hompage : MonoBehaviour
{
    public GameObject popupScreen;
    public Button yesButton, noButton;
    public Button tictacToe, dotsandBoxes, simGame, sosGame, fourinaRow, twoGuti;
    public GameObject scrollbar;
    float scroll_Pos = 0;
    float[] pos;
    public AudioClip audioClipMusic, audioClipSound;
    public AudioSource audioSource;
    public GameObject profilePanel, statisticsPanel, settingsPanel;
    public Button profilebutton, settingsbutton, statisticsbutton, profileQuit, settingsQuit, statisticsQuit, shareButton, moreAppsButton, gameQuitButton;

    public Button soundOn, soundOff, vibrationOn, vibrationOff;
    public ToggleGroup toggleGroup;
    int soundStatus = 1, vibrationStatus = 1, soundSettings = 1;
    string link = "https://play.google.com/store/apps/dev?id=7392900936708981207";

    bool isFocus = false;
    bool isProcessing = false;
    public Toggle[] toggles = new Toggle[5];

    void Start()
    {
        popupScreen.SetActive(false);
        profilePanel.SetActive(false);
        statisticsPanel.SetActive(false);
        settingsPanel.SetActive(false);
        soundStatus = PlayerPrefs.GetInt("soundStatus", 1);
        vibrationStatus = PlayerPrefs.GetInt("vibrationStatus", 1);
        soundSettings = PlayerPrefs.GetInt("soundSettings", 1);


        yesButton.onClick.AddListener(onYesPressed);
        noButton.onClick.AddListener(onNoPressed);
        tictacToe.onClick.AddListener(ont1);
        dotsandBoxes.onClick.AddListener(ont2);
        simGame.onClick.AddListener(ont3);
        sosGame.onClick.AddListener(ont4);
        fourinaRow.onClick.AddListener(ont5);
        twoGuti.onClick.AddListener(ont6);
        profilebutton.onClick.AddListener(onProfilleClicked);
        settingsbutton.onClick.AddListener(onSettingsClicked);
        statisticsbutton.onClick.AddListener(onStatisticsClicked);
        profileQuit.onClick.AddListener(onProfileQuit);
        settingsQuit.onClick.AddListener(onSettingsQuit);
        statisticsQuit.onClick.AddListener(onStatisticsQuit);
        gameQuitButton.onClick.AddListener(onGameQuit);
        moreAppsButton.onClick.AddListener(onMoreAppsClicked);
        shareButton.onClick.AddListener(onShareAppClicked);
        soundOn.onClick.AddListener(soundOnclicked);
        soundOff.onClick.AddListener(soundOffclicked);
        vibrationOn.onClick.AddListener(vibrationOnclicked);
        vibrationOff.onClick.AddListener(vibratioffOnclicked);

        playMusicSound();

    }


    void soundOnclicked()
    {
        soundSettings = 1;
        playButtonClickSound();
        soundOff.GetComponent<Image>().color = Color.white;
        soundOn.GetComponent<Image>().color = Color.green;
    }

    void soundOffclicked()
    {
        soundSettings = 2;
        soundOn.GetComponent<Image>().color = Color.white;
        soundOff.GetComponent<Image>().color = Color.green;
    }

    void vibrationOnclicked()
    {
        playButtonClickSound();
        vibrationStatus = 1;
        vibrationOff.GetComponent<Image>().color = Color.white;
        vibrationOn.GetComponent<Image>().color = Color.green;
    }

    void vibratioffOnclicked()
    {
        playButtonClickSound();
        vibrationStatus = 2;
        vibrationOn.GetComponent<Image>().color = Color.white;
        vibrationOff.GetComponent<Image>().color = Color.green;
    }


    void onMoreAppsClicked()
    {
        //Application.OpenURL(link);
        playButtonClickSound();
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Follow our Play Store Developer Page for More Apps by us!");
#endif
    }


    void onShareAppClicked()
    {
#if UNITY_ANDROID
        if (!isProcessing)
        {
            StartCoroutine(ShareTextInAnroid());
        }
#else
        //Debug.Log("No sharing set up for this platform.");
#endif
    }




    void playButtonClickSound()
    {
        if(soundSettings == 1)
        {
            audioSource.PlayOneShot(audioClipSound);
        }
    }



    void playMusicSound()
    {
        if(soundSettings == 1)
        {
            audioSource.PlayOneShot(audioClipMusic);
        }
    }




    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }

#if UNITY_ANDROID
    public IEnumerator ShareTextInAnroid()
    {
        var shareSubject = "I am in Love with this game";
        var shareMessage = "I think you should try this game. " +
                           "Download it from: " +
                           "https://play.google.com/store/apps/details?id=com.akapps.pnpgames";
        isProcessing = true;
        if (!Application.isEditor)
        {

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");


            currentActivity.Call("startActivity", chooser);


        }
        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }


#endif


    void onGameQuit()
    {
        playButtonClickSound();
        popupScreen.SetActive(true);
    }


#if UNITY_ANDROID && !UNITY_EDITOR
    private void showAndroidToastMessage(string message)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        if (unityActivity != null)
        {
            AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
            unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity, message, 0);
                toastObject.Call("show");
            }));
        }
    }
#endif


    void vibrationM()
    {
        if(vibrationStatus == 1)
        {
            VibrationManager.Vibrate();
        }
    }


    void onSettingsQuit()
    {
        playButtonClickSound();
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle.name == "Toggle (0)")
            {
                soundStatus = 1;
            }
            else if (activeToggle.name == "Toggle (1)")
            {
                soundStatus = 2;
            }
            else if (activeToggle.name == "Toggle (2)")
            {
                soundStatus = 3;
            }
            else if (activeToggle.name == "Toggle (3)")
            {
                soundStatus = 4;
            }
            else
            {
                soundStatus = 5;
            }
        }

        PlayerPrefs.SetInt("soundStatus", soundStatus);
        PlayerPrefs.SetInt("soundSettings", soundSettings);
        PlayerPrefs.SetInt("vibrationStatus", vibrationStatus);

        settingsPanel.SetActive(false);
    }

    void onStatisticsQuit()
    {
        playButtonClickSound();
        statisticsPanel.SetActive(false);
        
    }

    void onProfileQuit() {
        playButtonClickSound();
        profilePanel.SetActive(false);
    }

    void onProfilleClicked()
    {
        playButtonClickSound();
        profilePanel.SetActive(true);
    }

    void onSettingsClicked()
    {
        playButtonClickSound();
        if (soundSettings == 1)
        {
            soundOff.GetComponent<Image>().color = Color.white;
            soundOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            soundOn.GetComponent<Image>().color = Color.white;
            soundOff.GetComponent<Image>().color = Color.green;
        }

        if(vibrationStatus == 1)
        {
            vibrationOff.GetComponent<Image>().color = Color.white;
            vibrationOn.GetComponent<Image>().color = Color.green;
        }
        else
        {
            vibrationOn.GetComponent<Image>().color = Color.white;
            vibrationOff.GetComponent<Image>().color = Color.green;
        }

        toggles[soundStatus - 1].isOn = true;

        settingsPanel.SetActive(true);
    }

    void onStatisticsClicked()
    {
        playButtonClickSound();
        //statisticsPanel.SetActive(true);
#if UNITY_ANDROID && !UNITY_EDITOR
        showAndroidToastMessage("Coming Soon! Please Wait for the Upcoming Updates!");
#endif
    }


    void onYesPressed()
    {
        Application.Quit();
    }

    void onNoPressed()
    {
        playButtonClickSound();
        popupScreen.SetActive(false);
    }

    void ont1()
    {
        vibrationM();
        PlayerPrefs.SetInt("valueGame", 0);
        SceneManager.LoadSceneAsync("parentpage");
    }

    void ont2()
    {
        vibrationM();
        PlayerPrefs.SetInt("valueGame", 1);
        SceneManager.LoadSceneAsync("parentpage");
    }

    void ont3()
    {
        vibrationM();
        PlayerPrefs.SetInt("valueGame", 2);
        SceneManager.LoadSceneAsync("parentpage");
    }

    void ont4()
    {
        vibrationM();
        PlayerPrefs.SetInt("valueGame", 3);
        SceneManager.LoadSceneAsync("parentpage");
    }

    void ont5()
    {
        vibrationM();
        PlayerPrefs.SetInt("valueGame", 4);
        SceneManager.LoadSceneAsync("parentpage");
    }

    void ont6()
    {
        vibrationM();
        PlayerPrefs.SetInt("valueGame", 5);
        SceneManager.LoadSceneAsync("parentpage");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playButtonClickSound();
            if (settingsPanel.activeSelf)
            {
                onSettingsQuit();
            }
            else if (profilePanel.activeSelf)
            {
                onProfileQuit();
            }
            else if (statisticsPanel.activeSelf)
            {
                onStatisticsQuit();
            }
            else
            {
                popupScreen.SetActive(true);
            }
        }

        /*pos = new float[transform.childCount];
        float dis = 1f / (pos.Length - 1f);
        for (int i = 0; i < pos.Length; i++)
        {
            pos[i] = dis * i;
        }
        if (Input.GetMouseButton(0))
        {
            scroll_Pos = scrollbar.GetComponent<Scrollbar>().value;
        }
        else
        {
            for (int i = 0; i < pos.Length; i++)
            {
                if (scroll_Pos < pos[i] + (dis / 2) && scroll_Pos > pos[i] - (dis / 2))
                {
                    scrollbar.GetComponent<Scrollbar>().value = Mathf.Lerp(scrollbar.GetComponent<Scrollbar>().value, pos[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < pos.Length; i++)
        {
            if (scroll_Pos < pos[i] + (dis / 2) && scroll_Pos > pos[i] - (dis / 2))
            {
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1.5f, 1.5f), 0.1f);
            }
            for (int j = 0; j < pos.Length; j++)
            {
                if (j != i)
                {
                    transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(1f, 1f), 0.1f);
                }
            }
        }*/

    }
}

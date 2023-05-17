using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Parentpage : MonoBehaviour
{
    public ToggleGroup toggleGroup;
    public Slider slider;
    public Button onTutorialClicked, onSinglePlayer, onMultiplayer;
    int game_number = 0;
    public Image imageComponent;
    string[] spriteName = {"imh5", "imh1", "imh3", "imh4", "imh6", "imh2" };
    public Button onlineMultiplayer, backbtn;
    int sound = 2, vibration = 2;
    public AudioClip audioClipSound;
    public AudioSource audioSource;
    public Toggle[] toggles = new Toggle[3];

    void Start()
    {
        game_number = PlayerPrefs.GetInt("valueGame", 1);
        onSinglePlayer.onClick.AddListener(onSinglePlayerClicked);
        onMultiplayer.onClick.AddListener(onTwoPlayerClicked);
        onTutorialClicked.onClick.AddListener(onTutorialClick);
        onlineMultiplayer.onClick.AddListener(onMultiplayerClicked);
        backbtn.onClick.AddListener(onBackButtonClicked);
        Sprite sprite = Resources.Load<Sprite>(spriteName[game_number]);
        imageComponent.sprite = sprite;
        vibration = PlayerPrefs.GetInt("vibrationStatus", 1);
        sound = PlayerPrefs.GetInt("soundSettings", 1);

        slider.onValueChanged.AddListener(sliderSound);


        toggles[0].onValueChanged.AddListener((bool on) => {
            if (on)
            {
                playSound();
            }
        });

        toggles[1].onValueChanged.AddListener((bool on) => {
            if (on)
            {
                playSound();
            }
        });

        toggles[2].onValueChanged.AddListener((bool on) => {
            if (on)
            {
                playSound();
            }
        });

    }

    void sliderSound(float v)
    {
        playSound();
    }


    void playSound()
    {
        if(sound == 1)
        {
            audioSource.PlayOneShot(audioClipSound);
        }
    }

    void playVibration()
    {
        if (vibration == 1)
        {
            VibrationManager.Vibrate();
        }
    }



    void onTwoPlayerClicked()
    {
        int number = 1;
        int val = 0;
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if(activeToggle.name == "Grid1")
            {
                number = 1;
            }
            else if(activeToggle.name == "Grid2")
            {
                number = 2;
            }
            else
            {
                number = 3;
            }
        }

        string scene_name;
        string prefab_name;
        if (game_number == 0)
        {
            if (number == 1)
            {
                scene_name = "SampleScene";
                prefab_name = "Tictactoe";
            }
            else if (number == 2)
            {
                scene_name = "tictactoe_44";
                prefab_name = "Tictactoe44";
            }
            else
            {
                scene_name = "tictactoeworldwar";
                prefab_name = "Tictactoeww";
            }
        }
        else if (game_number == 1)
        {
            if (number == 1)
            {
                scene_name = "dotsandboxes_small";
                prefab_name = "DotsandBoxessmall";
            }
            else if (number == 2)
            {
                scene_name = "dotsandboxesnewmap";
                prefab_name = "Dotsandboxesnew";
            }
            else
            {
                scene_name = "dotsandboxes";
                prefab_name = "Dotsandboxes";
            }
        }
        else if (game_number == 2)
        {
            if (number == 1)
            {
                scene_name = "simgame";
                prefab_name = "simpage";
            }
            else if(number == 2)
            {
                scene_name = "New_SIM_Map";
                prefab_name = "simpageNew";
            }
            else
            {
                scene_name = "sim_map2";
                prefab_name = "simpage2";
            }
        }
        else if (game_number == 3)
        {
            scene_name = "sosgame";
            prefab_name = "sosgame";
        }
        else if (game_number == 4)
        {
            if (number == 1)
            {
                scene_name = "fourinarow_small";
                prefab_name = "fourinarowsmall";
            }

            else if(number == 2)
            {
                scene_name = "New_4_in_a_Row";
                prefab_name = "fourinarowmedium";

            }

            else
            {
                scene_name = "fourinarow";
                prefab_name = "fourinarow";
            }
        }
        else
        {
            scene_name = "twoguti";
            prefab_name = "twoguti";
        }

        loadScene(scene_name, prefab_name, val);
    }

    void onBackButtonClicked()
    {
        playSound();
        SceneManager.LoadSceneAsync("Landing_Page");
    }

    void loadScene(string sname, string pname, int p1)
    {
        playVibration();
        PlayerPrefs.SetInt(pname, p1);
        SceneManager.LoadSceneAsync(sname);
    }


    void onMultiplayerClicked()
    {
        playVibration();
        showAndroidToastMessage("Coming Soon! Please Wait for the Upcoming Updates....");
    }


    private void showAndroidToastMessage(string message)
    {
#if UNITY_ANDROID
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
#endif
    }


    void onSinglePlayerClicked()
    {
        int number = 1;
        int val;
        Toggle activeToggle = toggleGroup.ActiveToggles().FirstOrDefault();
        if (activeToggle != null)
        {
            if (activeToggle.name == "Grid1")
            {
                number = 1;
            }
            else if (activeToggle.name == "Grid2")
            {
                number = 2;
            }
            else
            {
                number = 3;
            }
        }

        float fillValue = slider.value;
        if(fillValue < 0.4f)
        {
            val = 1;
        }
        else if(fillValue < 1.4f)
        {
            val = 2;
        }
        else
        {
            val = 3;
        }

        string scene_name;
        string prefab_name;
        if (game_number == 0)
        {
            if (number == 1)
            {
                scene_name = "SampleScene";
                prefab_name = "Tictactoe";
            }
            else if (number == 2)
            {
                scene_name = "tictactoe_44";
                prefab_name = "Tictactoe44";
            }
            else
            {
                scene_name = "tictactoeworldwar";
                prefab_name = "Tictactoeww";
            }
        }
        else if (game_number == 1)
        {
            if (number == 1)
            {
                scene_name = "dotsandboxes_small";
                prefab_name = "DotsandBoxessmall";
            }
            else if (number == 2)
            {
                scene_name = "dotsandboxesnewmap";
                prefab_name = "dotsandboxesnewmap";
            }
            else
            {
                scene_name = "dotsandboxes";
                prefab_name = "Dotsandboxes";
            }
        }
        else if (game_number == 2)
        {
            if (number == 1)
            {
                scene_name = "simgame";
                prefab_name = "simpage";
            }
            else if (number == 2)
            {
                scene_name = "New_SIM_Map";
                prefab_name = "simpageNew";
            }
            else
            {
                scene_name = "sim_map2";
                prefab_name = "simpage2";
            }
        }
        else if (game_number == 3)
        {
            scene_name = "sosgame";
            prefab_name = "sosgame";
        }
        else if (game_number == 4)
        {
            if (number == 1)
            {
                scene_name = "fourinarow_small";
                prefab_name = "fourinarowsmall";
            }

            else
            {
                scene_name = "fourinarow";
                prefab_name = "fourinarow";
            }
        }
        else
        {
            scene_name = "twoguti";
            prefab_name = "twoguti";
        }

        loadScene(scene_name, prefab_name, val);
    }


    void onTutorialClick()
    {
        playSound();
        SceneManager.LoadSceneAsync("tutorialScene");
    }

    
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            playSound();
            SceneManager.LoadSceneAsync("Landing_Page");
        }

    }
}

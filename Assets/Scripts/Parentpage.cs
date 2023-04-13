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
    

    void Start()
    {
        
        game_number = PlayerPrefs.GetInt("valueGame", 1);
        onSinglePlayer.onClick.AddListener(onSinglePlayerClicked);
        onMultiplayer.onClick.AddListener(onTwoPlayerClicked);
        onTutorialClicked.onClick.AddListener(onTutorialClick);

        

        Sprite sprite = Resources.Load<Sprite>(spriteName[game_number]);
        imageComponent.sprite = sprite;
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
                scene_name = "dotsandboxes";
                prefab_name = "Dotsandboxes";
            }
            else
            {
                scene_name = "dotsandboxes_small";
                prefab_name = "DotsandBoxessmall";
            }
        }
        else if (game_number == 2)
        {
            if (number == 1)
            {
                scene_name = "simgame";
                prefab_name = "simpage";
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

    void loadScene(string sname, string pname, int p1)
    {
        PlayerPrefs.SetInt(pname, p1);
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(sname);
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
        else if(fillValue < 0.8f)
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
                scene_name = "dotsandboxes";
                prefab_name = "Dotsandboxes";
            }
            else
            {
                scene_name = "dotsandboxes_small";
                prefab_name = "DotsandBoxessmall";
            }
        }
        else if (game_number == 2)
        {
            if (number == 1)
            {
                scene_name = "simgame";
                prefab_name = "simpage";
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

    }

    
    void Update()
    {

        if (Input.GetKey(KeyCode.Escape))
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene("Landing_Page");
        }


    }
}

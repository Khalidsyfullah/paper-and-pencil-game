using System.Collections;
using System.Collections.Generic;
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


    void Start()
    {
        popupScreen.SetActive(false);
        yesButton.onClick.AddListener(onYesPressed);
        noButton.onClick.AddListener(onNoPressed);
        tictacToe.onClick.AddListener(ont1);
        dotsandBoxes.onClick.AddListener(ont2);
        simGame.onClick.AddListener(ont3);
        sosGame.onClick.AddListener(ont4);
        fourinaRow.onClick.AddListener(ont5);
        twoGuti.onClick.AddListener(ont6);
    }

    void onYesPressed()
    {
        Application.Quit();
    }

    void onNoPressed()
    {
        popupScreen.SetActive(false);
    }

    void ont1()
    {
        PlayerPrefs.SetInt("valueGame", 0);
        SceneManager.LoadScene("parentpage");
    }

    void ont2()
    {
        PlayerPrefs.SetInt("valueGame", 1);
        SceneManager.LoadScene("parentpage");
    }

    void ont3()
    {
        PlayerPrefs.SetInt("valueGame", 2);
        SceneManager.LoadScene("parentpage");
    }

    void ont4()
    {
        PlayerPrefs.SetInt("valueGame", 3);
        SceneManager.LoadScene("parentpage");
    }

    void ont5()
    {
        PlayerPrefs.SetInt("valueGame", 4);
        SceneManager.LoadScene("parentpage");
    }

    void ont6()
    {
        PlayerPrefs.SetInt("valueGame", 5);
        SceneManager.LoadScene("parentpage");
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            popupScreen.SetActive(true);
        }

        pos = new float[transform.childCount];
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
                    transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.9f, 0.9f), 0.1f);
                }
            }
        }

    }
}

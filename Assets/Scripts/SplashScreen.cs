using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    public GameObject panel;
    public Sprite sprite1;
    Image image;
    void Start()
    {
        image = panel.GetComponent<Image>();
        StartCoroutine(loadSecond1());
    }


    IEnumerator loadSecond1()
    {
        yield return new WaitForSeconds(1.5f);
        image.sprite = sprite1;
        StartCoroutine(loadSecond2());
    }

    IEnumerator loadSecond2()
    {
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadSceneAsync("Landing_Page");
    }
}

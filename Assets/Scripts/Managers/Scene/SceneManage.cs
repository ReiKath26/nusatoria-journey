using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneManage : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public static SceneManage instance;

    void Awake()
    {
        instance = this;
    }
    
    public void LoadScene(int sceneindex)
    {
        AudioManager.instance.Play("Click");
        StartCoroutine(LoadAsynchronously(sceneindex));
    }

    public void loadPopUp(GameObject popUp)
    {
        AudioManager.instance.Play("Click");
        popUp.SetActive(true);
    }

    public void closePopUp(GameObject popUp)
    {
        AudioManager.instance.Play("Click");
        popUp.SetActive(false);
    }

    public void togglePopUp(GameObject popUp)
    {
        AudioManager.instance.Play("Click");

        if(popUp.activeSelf == true)
        {
            popUp.SetActive(false);
        }

        else
        {
            popUp.SetActive(true);
        }
    }

    IEnumerator LoadAsynchronously (int sceneindex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneindex);

        loadingScreen.SetActive(true);


        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);

            slider.value = progress;

            yield return null;
        }
    }

}
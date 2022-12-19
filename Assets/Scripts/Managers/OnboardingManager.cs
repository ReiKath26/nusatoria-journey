using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OnboardingManager : MonoBehaviour
{
    private int genderChoose = 0;
    private string chooseName;

    [SerializeField] private GameObject onboardPanelOne;
    [SerializeField] private GameObject onboardPanelTwo;
    [SerializeField] private GameObject onboardPanelThree;

    [SerializeField] private GameObject female_picture_highlight;
    [SerializeField] private GameObject male_picture_highlight;
    [SerializeField] private GameObject userNameInput;

    [SerializeField] private GameObject female_picture;
    [SerializeField] private GameObject male_picture;
    [SerializeField] private TextMeshProUGUI player_name_text;

    [SerializeField] private SceneManage onboardingSceneManager;

    public void choosePicture(int choosen)
    {
        if (choosen == 0)
        {
            male_picture_highlight.SetActive(true);
            female_picture_highlight.SetActive(false);
        }

        else
        {
            male_picture_highlight.SetActive(false);
            female_picture_highlight.SetActive(true);
        }

        genderChoose = choosen;
    }

    public void saveName()
    {
        chooseName = userNameInput.GetComponent<InputField>().text;

        if (chooseName == "")
        {
            return;
        }
        onboardingSceneManager.loadPopUp(onboardPanelThree);
        onboardingSceneManager.closePopUp(onboardPanelTwo);
        displayData();
    }

    public void displayData()
    {
        if (genderChoose == 0)
        {
            male_picture.SetActive(true);
            female_picture.SetActive(false);
        }

        else
        {
            male_picture.SetActive(false);
            female_picture.SetActive(true);
        }

        player_name_text.text = chooseName;
    }

    public void startGame()
    {
        SaveManager.instance.saveData.list.Add(new SaveSlots() {slot = 0, playerName = chooseName, playerGender = genderChoose, time = 0, chapterNumber = 0, understandingLevel = 0, missionNumber = 0, storyNumber = 0});
        SaveManager.instance.saveFile();
        onboardingSceneManager.LoadScene(4);
    
    }

    public void resetData()
    {
        genderChoose = 0;
        chooseName = "";
        onboardingSceneManager.loadPopUp(onboardPanelOne);
        onboardingSceneManager.closePopUp(onboardPanelThree);
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;

public class SmallMenuPanel : MonoBehaviour
{
    public Button ExitButton;
    public Button TitleButton;
    private void Awake()
    {
        ExitButton.onClick.AddListener(Exit);
        TitleButton.onClick.AddListener(ToTitleScreen);
    }
    public void ToTitleScreen()
    {
        Debug.Log("you have clickad on da title");
        SceneManager.LoadScene("Title");
    }

    public void Exit()
    {
        Application.Quit();
    }
}

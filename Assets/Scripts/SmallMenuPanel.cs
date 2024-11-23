using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene("Title");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
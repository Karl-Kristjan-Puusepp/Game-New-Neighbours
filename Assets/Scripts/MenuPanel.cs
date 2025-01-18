using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button ExitButton;
    public Button PlayButton;
    public Button NewGameButton;
    public Button YesButton;
    public Button NoButton;
    public Toggle RequirementsOn;
    public GameObject message;
    public GameObject RestartPanel;

    public AnimationCurve Curve;
    public float Duration = 1;
    public Vector3 StartScale;
    public Vector3 TargetScale;
    private bool isPanelOpen;



    private float timeAggregate;

    private void Awake()
    {
        ExitButton.onClick.AddListener(Exit);
        PlayButton.onClick.AddListener(Play);
        NewGameButton.onClick.AddListener(OpenPanel);
        YesButton.onClick.AddListener(NewGame);
        NoButton.onClick.AddListener(ClosePanel);
        
        timeAggregate = 0;
        transform.localScale = StartScale;
    }

    private void Start()
    {
        if (RequirementsOn != null)
        {
            RequirementsOn.onValueChanged.AddListener(OnToggleValueChanged);
            RequirementsOn.isOn = Game.randomRequirements;

        }
        if (RestartPanel != null)
        {
            RestartPanel.SetActive(false);
        }
        if (message != null)
        {
            message.SetActive(false);
        }
    }

    private void ClosePanel()
    {
        isPanelOpen = false;
        if (RestartPanel != null)
        {
            RestartPanel.SetActive(false);
        }

    }
    private void OpenPanel()
    {
        if (RestartPanel != null)
        {
            isPanelOpen = !isPanelOpen;
            RestartPanel.SetActive(isPanelOpen);
        }
    }


    void OnToggleValueChanged(bool isOn)
    {
        Game.randomRequirements = isOn;

        Debug.Log("Boolean randomRequirements changed to " + Game.randomRequirements);
        if (isOn) showMessage();
    }

    public void showMessage()
    {
        message.SetActive(true);
        message.GetComponent<CanvasGroup>().alpha = 1.0f;
        StartCoroutine(DoFade());
    }

    IEnumerator DoFade()
    {

        CanvasGroup canvasgroup = message.GetComponent<CanvasGroup>();
        while (canvasgroup.alpha > 0)
        {
            canvasgroup.alpha -= Time.deltaTime / 4;
            yield return null;
        }
        canvasgroup.interactable = false;
        yield return null;
    }


    public void Play()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void NewGame()
    {

        Game.randomCounter = 0;
        Game.currentCustomerID = 0;
        Game.happySpriteNames = new List<string>();
        Game.villageSpriteNames = new List<string>();
        SceneData.happyCustomers = 0;
        SceneData.customersTotal = 0;
        Game.gameRestarted = true;

        SceneManager.LoadScene("MapScene");
    
    }



    void Update()
    {
        if (Duration > 0)
        {
            timeAggregate += Time.deltaTime / Duration;

        }
        else
        {
            timeAggregate = 1.0f;
        }

        float value = Curve.Evaluate(timeAggregate);
        transform.localScale = Vector3.LerpUnclamped(StartScale, TargetScale, value);

        if (timeAggregate >= 1.0f)
        {
            enabled = false;
        }
    }


}

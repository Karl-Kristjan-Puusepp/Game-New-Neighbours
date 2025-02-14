using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    public Button ExitButton;
    public Button PlayButton;

    public AnimationCurve Curve;
    public float Duration = 1;
    public Vector3 StartScale;
    public Vector3 TargetScale;


    private float timeAggregate;

    private void Awake()
    {
        ExitButton.onClick.AddListener(Exit);
        PlayButton.onClick.AddListener(Play);
        timeAggregate = 0;
        transform.localScale = StartScale;
    }

    public void Play()
    {
        SceneManager.LoadScene("MapScene");
    }

    public void Exit()
    {
        Application.Quit();
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

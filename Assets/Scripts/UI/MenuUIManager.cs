using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUIManager : MonoBehaviour
{

    private void Awake()
    {
        Transform root = transform;
        root.Find("Bg/StartButton").GetComponent<Button>().onClick.AddListener(OnClickStartButton);
        root.Find("Bg/CloseButton").GetComponent<Button>().onClick.AddListener(OnClickCloseButton);

    }


    private void OnClickCloseButton()
    {
        Application.Quit();
    }

    private void OnClickStartButton()
    {
        SceneManager.LoadScene("main");
    }
}

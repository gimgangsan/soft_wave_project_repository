using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverUI : MonoBehaviour
{
    public GameObject[] text;
    private CanvasGroup cvGroup;

    private void Start()
    {
        cvGroup = GetComponent<CanvasGroup>();
    }

    private void UpdateText(bool isWin)
    {

        foreach (GameObject obj in text)
        {
            if (isWin)
            {
                obj.GetComponent<TextMeshPro>().text = "WIN!";
            }
            else
            {
                obj.GetComponent<TextMeshPro>().text = "GAME OVER";
            }
        }
    }

    public void DoGameOver()
    {
        General.Instance.isPause = true;
        General.Instance.canOpenMenu = false;
        StartCoroutine("FadeIn");
        UpdateText(false);
    }

    public void DoWin()
    {
        General.Instance.isPause = true;
        General.Instance.canOpenMenu = false;
        StartCoroutine("FadeIn");
        UpdateText(true);
    }

    public void OnClickButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeIn()
    {
        while (cvGroup.alpha != 1f)
        {
            cvGroup.alpha += 0.1f;
            yield return new WaitForFixedUpdate();
        }
    }
}

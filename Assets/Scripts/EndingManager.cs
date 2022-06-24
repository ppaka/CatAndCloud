using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingManager : MonoBehaviour
{
    public TMP_Text text1, text2, text3;
    
    void Start()
    {
        StartCoroutine(Routine());
    }

    private IEnumerator Routine()
    {
        text1.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        text2.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2.5f);
        text3.gameObject.SetActive(true);
        yield return null;
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        SceneLoader.Instance.ChangeSceneImmediate("MainMenu");
    }
}

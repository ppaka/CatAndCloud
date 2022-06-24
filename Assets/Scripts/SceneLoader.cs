using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneLoader : MonoBehaviour
{
    private static SceneLoader _instance;

    public static SceneLoader Instance
    {
        get
        {
            if (_instance != null) return _instance;
            var obj = FindObjectOfType<SceneLoader>();
            _instance = obj != null ? obj : Create();
            return _instance;
        }
        private set => _instance = value;
    }

    private static SceneLoader Create()
    {
        var sceneLoaderPrefab = Resources.Load<SceneLoader>("SceneLoader");
        return Instantiate(sceneLoaderPrefab);
    }

    public CanvasGroup fadeImg;
    public float fadeDuration = 1; //암전되는 시간
    public float fadeValue = 1;
    public GameObject loading;

    //public Text Loading_text; //퍼센트 표시할 텍스트


    private void Awake()
    {
        if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
            DOTween.KillAll();

        fadeImg.DOFade(0, fadeDuration)
            .OnStart(() => {  })
            .OnComplete(() => { fadeImg.blocksRaycasts = false; });
    }

    public void ChangeScene(string sceneName) // 외부에서 전환할 씬 이름 받기
    {

        DOTween.KillAll();

        fadeImg.DOFade(fadeValue, fadeDuration)
            .OnStart(() => { fadeImg.blocksRaycasts = true; })
            .OnComplete(() =>
            {
                StartCoroutine(LoadScene(sceneName)); // 씬 로드 코루틴 실행
            });
    }
    
    public void ChangeSceneImmediate(string sceneName) // 외부에서 전환할 씬 이름 받기
    {
        DOTween.KillAll();

        SceneManager.LoadScene(sceneName);
    }

    public void NoLoadChangeScene(string sceneName)
    {
        
        // 두트윈 시퀀스 킬

        fadeImg.DOFade(fadeValue, fadeDuration)
            .OnStart(() => { fadeImg.blocksRaycasts = true; })
            .OnComplete(() => { StartCoroutine(NoLoadScene(sceneName)); });
    }

    public void ChangeSceneAdditive(string sceneName)
    {

        // 두트윈 시퀀스 킬
        
        StartCoroutine(LoadSceneAdditive(sceneName));
    }

    public void CloseScene(string sceneName)
    {
        StartCoroutine(UnloadScene(sceneName));
    }

    private IEnumerator LoadScene(string sceneName)
    {
        //loading.SetActive(true); //로딩 화면을 띄움

        var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        //async.allowSceneActivation = false; //퍼센트 딜레이용

        //float past_time = 0;
        //float percentage = 0;

        while (!(async.isDone))
        {
            yield return null;

            //past_time += Time.deltaTime;

            //if(percentage >= 90)
            //{
            //    percentage = Mathf.Lerp(percentage, 100, past_time);

            //    if(percentage == 100)
            //    {
            //        async.allowSceneActivation = true; //씬 전환 준비 완료
            //    }
            //}
            //else
            //{
            //    percentage = Mathf.Lerp(percentage, async.progress * 100f, past_time);
            //    if(percentage >= 90) past_time = 0;
            //}
            //Loading_text.text = percentage.ToString("0") + "%"; //로딩 퍼센트 표기
        }
    }

    private IEnumerator NoLoadScene(string sceneName)
    {
        var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        while (!(async.isDone))
        {
            yield return null;
        }
    }

    private IEnumerator LoadSceneAdditive(string sceneName)
    {
        var async = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!(async.isDone))
        {
            yield return null;
        }
    }

    private IEnumerator UnloadScene(string sceneName)
    {
        var async = SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        while (!(async.isDone))
        {
            yield return null;
        }
    }
}
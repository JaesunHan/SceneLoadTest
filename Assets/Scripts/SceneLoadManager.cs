using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    public Slider progressbar;
    public Text loadText;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    void Update()
    {
        //A 키를 눌렀을 때 MainScene 으로 넘어가게 한다.
        if (Input.GetKeyDown(KeyCode.A))
        {
            //===== 동기 방식 로딩 ======
            //SceneManager.LoadScene(1);
            //SceneManager.LoadScene("MainScene");
            //SceneManager.LoadScene(1, LoadSceneMode.Single); // ==  SceneManager.LoadScene(1);
            //SceneManager.LoadScene("MainScene", LoadSceneMode.Additive);
            //SceneManager.LoadScene("MainScene", new LoadSceneParameters(LoadSceneMode.Single, LocalPhysicsMode.Physics3D));

            //===== 비동기 방식 로딩 =====
            StartCoroutine(nameof(OnCoroutine_LoadSceneAsync));

        }
        //B 키를 눌렀을 때 TitleScene 으로 넘어가게 한다.
        else if (Input.GetKeyDown(KeyCode.B))
        {
            //SceneManager.LoadScene(0);
            //SceneManager.LoadScene("TitleScene");
        }
    }


    private IEnumerator OnCoroutine_LoadSceneAsync()
    {
        yield return null;

        AsyncOperation pAsyncOperation = SceneManager.LoadSceneAsync("MainScene");

        //작업의 완료 유무를 boolean 형으로 반환합니다.
        bool bIsDone  = pAsyncOperation.isDone;

        //진행도를 float 타입으로 반환하는데, 0, 1 을 반환합니다. (0 : 진행중, 1 : 진행완료)
        float fProgress = pAsyncOperation.progress;
        //true 면 로딩이 완료됐을 때 바로 씬을 넘기는 거고, false  면 Progress 가 0.9f 에서 멈춥니다.  이 값을 다시 true 로 해야 불러운 Scene 으로 넘어갈 수 있다.
        bool bAllowSceneActivation = pAsyncOperation.allowSceneActivation;

        //로딩이 끝나도 멈추게 하기
        pAsyncOperation.allowSceneActivation = false;


        Debug.Log($"Progress : {pAsyncOperation.progress}");

        //아직 전부 로딩하지 못했을 때 는 아래 반복문을 처리한다.
        while (!pAsyncOperation.isDone)
        {
            yield return null;

            if (progressbar.value < 1f)
            {
                progressbar.value = Mathf.MoveTowards(progressbar.value, 1f, Time.deltaTime);
            }
            else
            {
                loadText.text = "Press SpaceBar";
            }

            if (Input.GetKeyDown(KeyCode.Space) && pAsyncOperation.progress >= 0.9f && progressbar.value >= 1f)
            {
                pAsyncOperation.allowSceneActivation = true;
            }
        }
    }



    //심화
    //public void DoChangeScene_BySceneName(string strSceneName)
    //{ 
        
    //}
}

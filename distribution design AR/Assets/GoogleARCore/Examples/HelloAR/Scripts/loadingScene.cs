using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class loadingScene : MonoBehaviour {

    [SerializeField]
    Image progressBar;
    public GameObject asd;
    public Text text;
    private void Start()
    {
        //싱글톤으로 부터 문자열을 받고 그 문자열을 바탕으로 씬을 전환하자
        text.text = "LOADING... 0%";
        asd.SetActive(false);
    }

    public void CallNextScene(string sceneName) {
        gameObject.SetActive(true);
        asd.SetActive(true);
        StartCoroutine(LoadScene(sceneName));
    }
    //넘어가느 컨텐츠마다 세로운 가이드 장면으로 한번 더 사용방법을 익힐 수 있게 함..
    IEnumerator LoadScene(string sceneName) {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        float timer = 0.0f;

        while(!op.isDone){
            yield return null;

            timer += Time.deltaTime;

            if (op.progress >= 0.9f) {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);

                if (progressBar.fillAmount == 1.0f)
                {
                    Debug.Log("it is 1.0");
                    op.allowSceneActivation = true;
                }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                {
                    timer = 0f;
                }
                

            }
            text.text = "LOADING..." + (progressBar.fillAmount * 100).ToString("##") + "%";
        }
        
    }
}

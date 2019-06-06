using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
public class helper : MonoBehaviour {
    public GameObject helperPanel;
    public Sprite[] sprites;
    public Text page;
    bool activePage;
    int currentPage;
    int maxPage;
    GameObject[] gears;
    public string BundleURL;
    public string[] AssetName;
    string[] images;
    string name1;
    // Use this for initialization
    void Start () {
        StartCoroutine(downLoadImages());
        currentPage = 0;
        maxPage = sprites.Length - 1;
        helperPanel.SetActive(false);
        activePage = false;

    }
    IEnumerator downLoadImages() {

        if (SceneManager.GetActiveScene().name == "HelloAR") {
            name1 = "imagepole";
            images = new string[8];
            images[0] = "imgpole1";
            images[1] = "imgpole2";
            images[2] = "imgpole3";
            images[3] = "imgpole4";
            images[4] = "imgpole5";
            images[5] = "imgpole6";
            images[6] = "imgpole7";
            images[7] = "imgpole8";
        }
        if (SceneManager.GetActiveScene().name == "switchGear")
        {
            name1 = "imagegear";
            images = new string[8];
            images[0] = "imggear1";
            images[1] = "imggear2";
            images[2] = "imggear3";
            images[3] = "imggear4";
            images[4] = "imggear5";
            images[5] = "imggear6";
            images[6] = "imggear7";
            images[7] = "imggear8";
        }
        if (SceneManager.GetActiveScene().name == "rulerer")
        {
            name1 = "imagemeasure";

            images = new string[9];
            images[0] = "imageangle1";
            images[1] = "imageangle2";
            images[2] = "imageangle3";
            images[3] = "imageangle4";
            images[4] = "imgruler1";
            images[5] = "imgruler2";
            images[6] = "imgruler3";
            images[7] = "imgruler4";
            images[8] = "imgruler5";
            //images = new string[5];
            //images[0] = "imgruler1";
            //images[1] = "imgruler2";
            //images[2] = "imgruler3";
            //images[3] = "imgruler4";
            //images[4] = "imgruler5";
        }
        if (SceneManager.GetActiveScene().name == "Distance")
        {
            name1 = "imagedist";
            images = new string[3];
            images[0] = "imgdist1";
            images[1] = "imgdist2";
            images[2] = "imgdist3";
        }
        while (!Caching.ready)
            yield return null;
        using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL,0))
        {
            yield return www;
            if (www.error != null)
            {
                throw new Exception("WWW download had an error : " + www.error);
            }


            AssetBundle bundle = www.assetBundle;

            sprites = new Sprite[images.Length];
            for (int i = 0; i < images.Length; i++) {
                sprites[i] = bundle.LoadAsset<Sprite>(images[i]);
            }
            
            yield return new WaitForSeconds(0.1f);

            helperPanel.GetComponent<Image>().sprite = sprites[0];

            yield return new WaitForSeconds(0.1f);
            bundle.Unload(false);
        }

    }


    public void ActiveHelperPanel() {
        if (activePage == false) {
            helperPanel.SetActive(true);
            activePage = true;
            page.text = (currentPage + 1) + " / " + (maxPage + 1);
        }
    }

    public void CancelHelperPanel() {
        if (activePage == true) {
            activePage = false;
            helperPanel.SetActive(false);
        }
    }

    public void PreviewPage() {
        if (currentPage != 0) {
            currentPage--;
            helperPanel.GetComponent<Image>().sprite = sprites[currentPage];
            page.text = (currentPage + 1) + " / " + (maxPage + 1);
        }
    }

    public void NextPage()
    {
        if (currentPage < maxPage)
        {
            currentPage++;
            helperPanel.GetComponent<Image>().sprite = sprites[currentPage];
            page.text = (currentPage + 1) + " / " + (maxPage + 1);
        }
    }


    // Update is called once per frame
    void Update () {
		
	}
}

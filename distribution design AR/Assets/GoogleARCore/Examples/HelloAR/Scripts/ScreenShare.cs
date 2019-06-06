using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.UI;

public class ScreenShare : MonoBehaviour {
	public static ScreenShare instance;

	//for android
	private bool isProcessing = false;

    private bool first = false;
    private bool second = false;
    private bool car1 = false;
    public GameObject firstLine;
    public GameObject secondLine;
    public GameObject carmerabutton1;

    bool gpsInit = false;

    LocationInfo currentGPSPosition;

    int gps_connect = 0;

    double detailed_num = 1.0;//기존 좌표는 float형으로 소수점 자리가 비교적 자세히 출력되는 double을 곱하여 자세한 값을 구합니다.

    public Text text_latitude;

    public Text text_longitude;

    public Text text_refresh;


    void Awake()
	{
		MakeInstance();
	}

	//method whihc make this object instance
	void MakeInstance()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	//function called from a button
	public void ButtonShare()
	{
        
        if (!isProcessing)
		{	
			
			StartCoroutine(ShareScreenshot());

		}

        

    }
    private void makeFalse(bool obj) {
        obj = false;
    }
	public IEnumerator ShareScreenshot()
	{
        
        isProcessing = true;
        if (firstLine.activeSelf == true) {
            first = true;
            firstLine.SetActive(false);
        }
        if (secondLine.activeSelf == true)
        {
            second = true;
            secondLine.SetActive(false);
        }
        if (carmerabutton1.activeSelf == true)
        {
            car1 = true;
            carmerabutton1.SetActive(false);
        }
        

        // wait for graphics to render
        yield return new WaitForEndOfFrame();
		string date = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
		string myFilename = "myScreenshot_"+date+".png" ;
		// debugText.text = ": " + myFilename;
		string myDefaultLocation = Application.persistentDataPath + "/" + myFilename;
		//EXAMPLE OF DIRECTLY ACCESSING THE Camera FOLDER OF THE GALLERY
		//string myFolderLocation = "/storage/emulated/0/DCIM/Camera/";
		//EXAMPLE OF BACKING INTO THE Camera FOLDER OF THE GALLERY
		//string myFolderLocation = Application.persistentDataPath + "/../../../../DCIM/Camera/";
		//EXAMPLE OF DIRECTLY ACCESSING A CUSTOM FOLDER OF THE GALLERY
		string myFolderLocation = "/storage/emulated/0/DCIM/ElectricPole/";
		string myScreenshotLocation = myFolderLocation + myFilename;
        string path = Application.persistentDataPath + "/MyPicture.png";
        //ENSURE THAT FOLDER LOCATION EXISTS
        if (!System.IO.Directory.Exists(myFolderLocation))
        {
            System.IO.Directory.CreateDirectory(myFolderLocation);
        }

        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        // create the texture
        Texture2D screenTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, true);
        // put buffer into texture
        screenTexture.ReadPixels(new Rect(0f, 0f, Screen.width, Screen.height), 0, 0);
        // apply
        screenTexture.Apply();
        //----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- PHOTO
        byte[] dataToSave = screenTexture.EncodeToPNG();
        string destination = Path.Combine(Application.persistentDataPath, System.DateTime.Now.ToString("yyyy-MM-dd-HHmmss") + ".png");
        File.WriteAllBytes(myScreenshotLocation, dataToSave);
        File.WriteAllBytes(destination, dataToSave);
        File.WriteAllBytes(path, dataToSave);
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS BEGUN 
        AndroidJavaClass classPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject objActivity = classPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass classUri = new AndroidJavaClass("android.net.Uri");
        // "android.intent.action.MEDIA_SCANNER_SCAN_FILE" <--- 요거 햇갈림.. 원래 찾은건 "android.intent.action.MEDIA_MOUNTED" 요렇게 하라고 나와있는데 안되서 저렇게 하니 됨. 
        AndroidJavaObject objIntent = new AndroidJavaObject("android.content.Intent", new object[2] { "android.intent.action.MEDIA_SCANNER_SCAN_FILE", classUri.CallStatic<AndroidJavaObject>("parse", "file://" + myScreenshotLocation) });
        objActivity.Call("sendBroadcast", objIntent);


        // debugText.text = "Complete! - " + myScreenshotLocation; 
        //REFRESHING THE ANDROID PHONE PHOTO GALLERY IS COMPLETE 

        //AUTO LAUNCH/VIEW THE SCREENSHOT IN THE PHOTO GALLERY!!! 
        // Application.OpenURL(myScreenshotLocation); 
        //AFTERWARDS IF YOU MANUALLY GO TO YOUR PHOTO GALLERY, 
        //YOU WILL SEE THE FOLDER WE CREATED CALLED "myFolder" 
        
        if (first == true)
        {
            firstLine.SetActive(true);
            first = false;
        }
        if (second == true)
        {
            secondLine.SetActive(true);
            second = false;
        }
        if (car1 == true)
        {
            carmerabutton1.SetActive(true);
            car1 = false;
        }
        if (!Application.isEditor)
        {
            yield return new WaitForSeconds(2);
            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "image/*");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "검측 사진 자료");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "검측 사진 자료");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "각도: " + Mathf.Abs(GameObject.FindGameObjectsWithTag("anglePole")[1].transform.localEulerAngles.z - 90).ToString("#.#"));

            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", myScreenshotLocation);
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
            bool fileExist = fileObject.Call<bool>("exists");
            if (fileExist)
                intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share Via");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            currentActivity.Call("startActivity", jChooser);
}
        
        
        isProcessing = false;

	}
    
}

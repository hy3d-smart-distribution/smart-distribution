using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class ScreenShot : MonoBehaviour {
	public static ScreenShot instance;

	//for android
	private bool isProcessing = false;

    private bool first = false;
    private bool second = false;
    private bool car1 = false;
    private bool car2 = false;
    public GameObject firstLine;
    public GameObject secondLine;
    public GameObject carmerabutton1;
    public GameObject carmerabutton2;


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
        if (carmerabutton2.activeSelf == true)
        {
            car2 = true;
            carmerabutton2.SetActive(false);
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
		//ENSURE THAT FOLDER LOCATION EXISTS
		if(!System.IO.Directory.Exists(myFolderLocation)){
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

        /*if (!Application.isEditor)
		{
			// block to open the file and share it ------------START
			AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
			AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");
			intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
			AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
			AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + destination);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

			intentObject.Call<AndroidJavaObject>("setType", "text/plain");
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), "" + message);
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), "SUBJECT");

			intentObject.Call<AndroidJavaObject>("setType", "image/jpeg");
			AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");

			currentActivity.Call("startActivity", intentObject);
		}*/
        DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat")+"/" + PlayerPrefs.GetString("long"), "사진촬영");
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
        if (car2 == true)
        {
            carmerabutton2.SetActive(true);
            car2 = false;
        }
        isProcessing = false;

	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class DataTransmission : MonoBehaviour {
    static int signal;
    static string[] mode = { "pole", "transistor", "ruler", "distance" };
    static string nextMode;
    static int count = 0;

    string dept;
    public Text myInfo;
    public Text userName;
    public Dropdown dropdown;
    public int a = 0;
    public static DataTransmission instance = null;



    //changeNumber 다른 씬에서 가공파트로 넘어갈때 코드로 쓴다..x
    public void changeFirst(int index)
    {
        //load Scene을 비동기식으로 놓고 로딩중을 보여주는 패널을 수시로 보여주기, 
        signal = index;
        nextMode = mode[0];
        StartCoroutine(SendData());
        //SceneManager.LoadScene("HelloAR");
        if (SceneManager.GetActiveScene().name != "HelloAR") {
            //현재씬과 진행하려는 씬이 일치 하지 않기 때문에.. 씬을 비동기식으로 전환해주기..
            GameObject.Find("Example Controller").SendMessage("CallNextScene", "HelloAR");
        }
        
    }

    public void jumpTransistor(int index)
    {
        nextMode = mode[1];
        signal = index;
        StartCoroutine(SendData());
        //SceneManager.LoadScene("switchGear");
        if (SceneManager.GetActiveScene().name != "switchGear")
        {
            //현재씬과 진행하려는 씬이 일치 하지 않기 때문에.. 씬을 비동기식으로 전환해주기..
            GameObject.Find("Example Controller").SendMessage("CallNextScene", "switchGear");
        }

    }


    public void jumpMassive(int index)
    {
        signal = index;
        nextMode = mode[2];
        StartCoroutine(SendData());
        //SceneManager.LoadScene("rulerer");
        if (SceneManager.GetActiveScene().name != "rulerer")
        {
            //현재씬과 진행하려는 씬이 일치 하지 않기 때문에.. 씬을 비동기식으로 전환해주기..
            GameObject.Find("Example Controller").SendMessage("CallNextScene", "rulerer");
        }

    }

    public void jumpDistance()
    {
        nextMode = mode[3];
        StartCoroutine(SendData());
        //SceneManager.LoadScene("Distance");
        if (SceneManager.GetActiveScene().name != "Distance")
        {
            //현재씬과 진행하려는 씬이 일치 하지 않기 때문에.. 씬을 비동기식으로 전환해주기..
            GameObject.Find("Example Controller").SendMessage("CallNextScene", "Distance");
        }
    }
    public void jumpDistribution()
    {
        nextMode = mode[3];
        StartCoroutine(SendData());
        //SceneManager.LoadScene("AugmentedImage");
        if (SceneManager.GetActiveScene().name != "AugmentedImage")
        {
            //현재씬과 진행하려는 씬이 일치 하지 않기 때문에.. 씬을 비동기식으로 전환해주기..];
            GameObject.Find("Example Controller").SendMessage("CallNextScene", "AugmentedImage");
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this) {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
        
    }



    // Use this for initialization
    void Start() {
        
        string data = PlayerPrefs.GetString("userName") + "\n@" + PlayerPrefs.GetString("dept");
        if (count == 0) {
            GameObject.Find("Example Controller").SendMessage("UpperPanel");
        }
        if (!PlayerPrefs.HasKey("register"))
        {
            PlayerPrefs.SetInt("register", 0);
            GameObject.Find("Example Controller").SendMessage("UpperJoinPanel");
        }

        else {
            GameObject.Find("Example Controller").SendMessage("getMyInfo", data);
        }

        
    }

    IEnumerator SendData() {
        count++;
        yield return new WaitForSeconds(2);
        switch (nextMode)
        {
            case "pole":
                GameObject.Find("Example Controller").SendMessage("ListenFromMenu", signal);
                Debug.Log("success data transmission");
                break;
            case "transistor":
                GameObject.Find("Example Controller").SendMessage("ListenFromMenu", signal);
                Debug.Log("success data transmission transistor");
                break;
            case "ruler":
                GameObject.Find("Example Controller").SendMessage("ListenFromMenu", signal);
                break;
            case "distance":
                GameObject.Find("Example Controller").SendMessage("ListenFromMenu");
                break;
        }
    }

    public void TrackedPlaneGuide(GameObject movedObj, Vector3[] path) {
        //처음 바닥 생성하는 방법 가이드
        //show가 true일때 활성화, false이면 비활성화
        //비활성화되어 있는 데이터도 참조 하려면 인수를 갖는게 좋긴한데...
        //GameObject1,GameObject2,Vector3[] 이렇게 인수로 받으면 될 듯
        LeanTween.moveSpline(movedObj, path, 100f).setOrientToPath2d(true).setSpeed(100f).setLoopPingPong();
    }

    public void InstantiateObjeactGuide(GameObject tap1, GameObject tap2, GameObject dot1, GameObject dot2)
    {
        StartCoroutine(StartInstantiateGuide(tap1, tap2, dot1, dot2));
    }

    public void MoveControlGuide(GameObject movedObj, Vector3[] path) {
        //오브젝트를 터치로 이동할 수 있다는 사실을 알려주기 위한 가이드
        //show가 true일때 활성화, false이면 비활성화
        LeanTween.moveSpline(movedObj, path, 100f).setOrientToPath2d(true).setSpeed(100f).setLoopPingPong();
    }
    public void RotationControlGuide(GameObject hand, GameObject triangle) {
        //오브젝트를 두 손가락으로 트윈하여 회전시킬 수 있다는 사실을 알려주기 위한 가이드
        //show가 true일때 활성화, false이면 비활성화
        StartCoroutine(StartRotateGuide(hand, triangle));
    }

    public void Loggging(string dept, string name, string gps, string log) {
        StartCoroutine(Join(dept, name, gps, log));
    }

    IEnumerator StartInstantiateGuide(GameObject obj1, GameObject obj2, GameObject dot1, GameObject dot2)
    {
        obj1.SetActive(true);
        obj2.SetActive(false);
        dot1.SetActive(false);
        dot2.SetActive(false);
        LeanTween.rotateAround(obj1, Vector3.forward, 115f, 3f);
        while ((int)obj1.transform.eulerAngles.z != 115)
        {
            yield return new WaitForFixedUpdate();
        }
        dot1.SetActive(true);

        yield return new WaitForSeconds(0.5f);
        obj1.transform.localRotation = Quaternion.Euler(0, 0, 0);
        obj1.SetActive(false);
        obj2.SetActive(true);
        dot1.SetActive(false);
        dot2.SetActive(false);
        LeanTween.rotateAround(obj2, Vector3.forward, -115f, 3f);
        while ((int)obj2.transform.eulerAngles.z != 245)
        {
            yield return new WaitForFixedUpdate();
        }
        dot2.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        obj2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //yield return new WaitForSeconds(0.1f);
        StartCoroutine(StartInstantiateGuide(obj1, obj2, dot1, dot2));
    }
    IEnumerator StartRotateGuide(GameObject obj1, GameObject obj2)
    {
        obj1.transform.localRotation = Quaternion.Euler(0, 0, 0);
        obj2.transform.localRotation = Quaternion.Euler(0, 0, 0);
        yield return null;
        LeanTween.rotateAround(obj1, Vector3.forward, 35f, 3f).setLoopPingPong();
        LeanTween.rotateAround(obj2, Vector3.up, 35f, 3f).setLoopPingPong();

    }

    IEnumerator Join(string dept, string name, string gps, string log)
    {


        WWWForm form = new WWWForm();
        //string url = "http://106.10.51.228/token/join";
        //string url = "http://106.10.51.228/token/login";
        string url = "http://106.10.51.228/log";

        //string str = "{\"email\":\"ewq@gmail.com\",\"password\":\"asdasd\",\"name\":\"고홍식\",\"company_id\" : 1}";
        //Hashtable header = new Hashtable();
        //header.Add("Content-Type", "application/json");
        string str = "{\"dept\":\"" + dept + "\",\"email\":\"" + name + "\",\"place\":\"" + gps + "\",\"message\":\"" + log + "\"}";
        Hashtable header = new Hashtable();
        header.Add("Content-Type", "application/json");
        WWW www = new WWW(url, System.Text.Encoding.UTF8.GetBytes(str), header);    //수정된 부분 

        yield return www;

        if (www.error == null)
        {
            Debug.Log("WWWOK : " + www.text);
        }
        else
        {
            Debug.Log("error : " + www.error);
        }
    }
    // Update is called once per frame
    void Update () {
        
        
    }
}

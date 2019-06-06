using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class productList : MonoBehaviour {
    public GameObject productPanel;
    public GameObject joinPanel;
    private int screenHeight;
    private bool up;
    private bool joinUp;
    public GameObject cancelPanel;
    private bool cancel;
    public GameObject listManager;
    public Text myInfo;
    string[] depts = { "경기지역본부직할", "안양지사", "안산지사", "성남지사", "오산지사",
                            "평택지사", "화성지사", "광주지사", "서수원지사", "서용인지사",
                            "안성지사", "이천지사", "동용인지사", "여주지사", "하남지사",
                            "광명지사", "성남전력지사", "군포전력지사", "평택전력지사"};
    string[] headquaters = {"본사", "서울지역본부", "남서울지역본부", "경기지역본부", "경기북부지역본부", "인천지역본부", "강원지역본부",
                                "대전세종충남지역본부", "충북지역본부", "전북지역본부", "광주전남지역본부", "경북지역본부", "대구지역본부",
                                "경남지역본부", "부산울산지역본부", "제주지역본부" };
    List<List<string>> localOfficeList = new List<List<string>>();
    string str1 =  "한전본사";
    string[] str2 = { "서울본부지역직할", "동대문중랑지사", "서대문은평지사", "강북성북지사", "광진성동지사", "마포용산지사", "노원도봉지사", "성동전력지사", "중부전력지사" };
    string[] str3 = { "남서울지역본부직할", "강서양천지사", "관악동작지사", "강동송파지사", "강남지사", "서초지사", "영등포전력지사", "동서울전력지사", "강남전력지사" };
    string[] str4 = { "경기지역본부직할", "안양지사", "안산지사", "성남지사", "오산지사", "평택지사", "화성지사", "광주지사", "서수원지사", "서용인지사", "안성지사", "이천지사", "동용인지사", "여주지사", "하남지사", "광명지사", "성남전력지사", "군포전력지사", "평택전력지사", "동부전력지사" };
    string[] str5 = { "경기북부지역본부직할", "고양지사", "파주지사", "구리지사", "양평지사", "포천지사", "남양주지사", "동두천지사", "가평지사", "연천지사", "구리전력지사", "고양전력지사" };
    string[] str6 = { "인천지역본부직할", "남인천지사", "부천지사", "김포지사", "제물포지사", "서인천지사", "시흥지사", "강화지사", "영종지사", "백령지사", "부평전력지사", "시흥전력지사", "김포전력지사" };
    string[] str7 = { "강원지역본부직할", "강릉지사", "홍천지사", "원주지사", "횡성지사", "속초지사", "철원지사", "삼척지사", "영월지사", "동해지사", "인제지사", "양구지사", "태백지사", "양양지사", "화천지사", "고성지사", "평창지사", "정선지사", "태백전력지사", "동해전력지사", "원주전력지사", "강릉전력지사" };
    string[] str8 = { "대전세종충남지역본부직할", "천안지사", "대덕유성지사", "아산지사", "서대전지사", "세종지사", "당진지사", "서산지사", "보령지사", "논산지사", "공주지사", "홍성지사", "태안지사", "부여지사", "예산지사", "금산지사", "서천지사", "청양지사", "아산전력지사", "서산전력지사", "청양전력지사", "대전전력지사", "세종전력지사", };
    string[] str9 = { "충북지역본부직할", "동청주지사", "충주지사", "제천지사", "진천지사", "증평괴산지사", "음성지사", "영동지사", "보은지사", "옥천지사", "단양지사", "청주전력지사", "충주전력지사" };
    string[] str10 = { "전북본부지역직할", "익산지사", "군산지사", "남전주지사", "김제지사", "정읍지사", "남원지사", "고창지사", "부안지사", "임실지사", "진안지사", "장수지사", "순창지사", "무주지사", "군산전력지사", "김제전력지사" };
    string[] str11 = { "광주전남지역본부직할", "여수지사", "순천지사", "광산지사", "서광주지사", "목포지사", "나주지사", "해남지사", "고흥지사", "영암지사", "화순지사", "광양지사", "보성지사", "무안지사", "영광지사", "강진지사", "장성지사", "장흥지사", "담양지사", "진도지사", "곡성지사", "완도지사", "신안지사", "구례지사", "함평지사", "순천전력지사", "강진전력지사", "광주전력지사", "목포전력지사" };
    string[] str12 = { "경북지역본부직할", "구미지사", "상주지사", "영주지사", "의성지사", "문경지사", "예천지사", "봉화지사", "울진지사", "군위지사", "청송지사", "영양지사", "영주젼력지사", "구미전력지사" };
    string[] str13 = { "대구지역본부직할", "서대구지사", "경주지사", "남대구지사", "서대구지사", "포항지사", "경산지사", "김천지사", "영천지사", "칠곡지사", "성주지사", "청도지사", "북포항지사", "고령지사", "영덕지사", "울릉지사", "포한전력지사", "달성전력지사", "칠곡전력지사", "경산전력지사" };
    string[] str14 = { "경남지역본부직할", "진주지사", "마산지사", "거제지사", "밀양지사", "사천지사", "통영지사", "거창지사", "함안지사", "창녕지사", "합천지사", "진해지사", "하동지사", "고성지사", "산청지사", "남해지사", "함양지사", "의령지사", "함안전력지사", "전주전력지사", "통영전력지사" };
    string[] str15 = { "부산울산지역본부직할", "울산지사", "김해지사", "동래지사", "남부산지사", "양산지사", "중부산지사", "북부산지사", "동울산지사", "서부산지사", "기장지사", "서울산지사", "영도지사", "북부산전력지사", "울산전력지사", "동북산전력지사", "서부산전력지사" };
    string[] str16 = { "제주지역본부직할", "제주전력지사", "서귀포지사" };

    
    int oneTIme;

    public Dropdown dropdown;
    public void changeFirst() {
        DataTransmission.instance.changeFirst(1);
        
    }

    public void changeSecond()
    {
        DataTransmission.instance.changeFirst(2);
    }
    public void changeThird()
    {
        DataTransmission.instance.changeFirst(3);

    }
    public void changeFourth()
    {
        DataTransmission.instance.changeFirst(4);


    }
    public void changeFifth()
    {
        DataTransmission.instance.changeFirst(5);


    }
    public void changeSix()
    {
        DataTransmission.instance.changeFirst(6);

    }
    public void changeSeven()
    {
        DataTransmission.instance.changeFirst(7);

    }
    public void changeEight()
    {
        DataTransmission.instance.changeFirst(8);

    }

    public void jumpTransistor1() {

        DataTransmission.instance.jumpTransistor(1);
    }

    public void jumpTransistor2()
    {
        DataTransmission.instance.jumpTransistor(2);

    }
    public void jumpTransistor3()
    {
        DataTransmission.instance.jumpTransistor(3);

    }
    public void jumpTransistor4()
    {
        DataTransmission.instance.jumpTransistor(4);

    }
    public void jumpTransistor5()
    {
        DataTransmission.instance.jumpTransistor(5);

    }
    public void jumpTransistor6()
    {
        DataTransmission.instance.jumpTransistor(6);

    }
    public void jumpTransistor7()
    {
        DataTransmission.instance.jumpTransistor(7);

    }
    public void jumpTransistor8()
    {
        DataTransmission.instance.jumpTransistor(8);

    }
    public void jumpTransistor9()
    {
        DataTransmission.instance.jumpTransistor(9);

    }
    
    public void jumpMassive() {
        DataTransmission.instance.jumpMassive(1);

    }
    public void jumpMassive2()
    {
        DataTransmission.instance.jumpMassive(2);

    }

    public void jumpDistance() {
        DataTransmission.instance.jumpDistance();

    }
    public void jumpDistribution()
    {
        DataTransmission.instance.jumpDistribution();

    }
    public void UpperPanel()
    {
        if (up == false) {
            iTween.MoveBy(productPanel, iTween.Hash("y", screenHeight, "easeType", "easeInOutExpo", "delay", .1));
            up = true;
        }
        if (SceneManager.GetActiveScene().name == "AugmentedImage") {
            GameObject.Find("ExampleController").SendMessage("HideScanCanvas", true);
        }


    }
    public void UpperJoinPanel()
    {
        if (joinUp == false)
        {
            iTween.MoveBy(joinPanel, iTween.Hash("y", screenHeight, "easeType", "easeInOutExpo", "delay", .1));
            joinUp = true;
        }
    }
    public void Register() {
        string str = GameObject.FindGameObjectWithTag("myName").GetComponent<Text>().text;
        string dept = depts[GameObject.FindGameObjectWithTag("deptNum").GetComponent<Dropdown>().value];
        if (str == "")
        {
            Debug.Log("이름 미입력");
        }
        else
        {
            PlayerPrefs.SetString("dept", dept);
            PlayerPrefs.SetString("userName", str);
            Debug.Log(PlayerPrefs.GetString("dept"));
            Debug.Log(PlayerPrefs.GetString("userName"));
            string data = PlayerPrefs.GetString("userName") + "\n@" + PlayerPrefs.GetString("dept");
            GameObject.Find("Example Controller").SendMessage("getMyInfo", data);
        }
    }

    public void DownPanel()
    {
        if (up == true) {
            iTween.MoveBy(productPanel, iTween.Hash("y", -screenHeight, "easeType", "easeInOutExpo", "delay", .1));
            up = false;
        }
        if (SceneManager.GetActiveScene().name == "AugmentedImage")
        {
            GameObject.Find("ExampleController").SendMessage("HideScanCanvas", false);
        }

        if (DataTransmission.instance.a == 0) {
            DataTransmission.instance.a = 1;
        }

    }
    public void DownJoinPanel()
    {
        if (joinUp == true)
        {
            iTween.MoveBy(joinPanel, iTween.Hash("y", -screenHeight, "easeType", "easeInOutExpo", "delay", .1));
            joinUp = false;
        }

    }

    public void QuitBtn() {
        Application.Quit();
    }

    public void CancelBtn() {
        cancel = false;
        cancelPanel.SetActive(false);
    }
    // Use this for initialization
    void Start () {
        screenHeight = Screen.height;
        productPanel.transform.position += new Vector3(0, -screenHeight, 0);
        joinPanel.transform.position += new Vector3(0, -screenHeight, 0);
        up = false;
        cancel = false;
        cancelPanel.SetActive(false);
        joinUp = false;

        changeLocalOfficeList(0);
        localOfficeList.Add(new List<string>());
        localOfficeList[0].Add(str1);
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str2.Length; i++) {
            localOfficeList[1].Add(str2[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str3.Length; i++)
        {
            localOfficeList[2].Add(str3[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str4.Length; i++)
        {
            localOfficeList[3].Add(str4[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str5.Length; i++)
        {
            localOfficeList[4].Add(str5[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str6.Length; i++)
        {
            localOfficeList[5].Add(str6[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str7.Length; i++)
        {
            localOfficeList[6].Add(str7[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str8.Length; i++)
        {
            localOfficeList[7].Add(str8[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str9.Length; i++)
        {
            localOfficeList[8].Add(str9[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str10.Length; i++)
        {
            localOfficeList[9].Add(str10[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str11.Length; i++)
        {
            localOfficeList[10].Add(str11[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str12.Length; i++)
        {
            localOfficeList[11].Add(str12[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str13.Length; i++)
        {
            localOfficeList[12].Add(str13[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str14.Length; i++)
        {
            localOfficeList[13].Add(str14[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str15.Length; i++)
        {
            localOfficeList[14].Add(str15[i]);
        }
        localOfficeList.Add(new List<string>());
        for (int i = 0; i < str16.Length; i++)
        {
            localOfficeList[15].Add(str16[i]);
        }





    }
    public void getMyInfo(string data)
    {
        myInfo.text = data;
        DownJoinPanel();
    }

    public void changeLocalOfficeList(int sig)
    {


        //temp = new List<string> { localOfficeList[sig]};
        for (int j = 0; j < localOfficeList.Count; j++) {
            for (int k = 0; k < localOfficeList[j].Count; k++) {
                Debug.Log(localOfficeList[j][k]);
            }
        }

        
        //GameObject.FindGameObjectWithTag("deptNum").GetComponent<Dropdown>().ClearOptions();
        //GameObject.FindGameObjectWithTag("deptNum").GetComponent<Dropdown>().op


    }
    // Update is called once per frame
    void Update () {
        
        if (Input.GetKey(KeyCode.Escape) && up == true)
        {
            //Application.Quit();
            iTween.MoveBy(productPanel, iTween.Hash("y", -screenHeight, "easeType", "easeInOutExpo", "delay", .1));
            up = false;
        }
        else if (Input.GetKey(KeyCode.Escape) && up == false) {

            cancelPanel.SetActive(true);
            
         
            
        }
        if (oneTIme == 0) {

        }
        
    }
}

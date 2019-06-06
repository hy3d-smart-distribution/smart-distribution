//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google">
//
// Copyright 2017 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using System.Collections.Generic;
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
    using System;
    using UnityEngine.SceneManagement;
    using Lean.Touch;
	using System.Collections;
#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class switchGear : MonoBehaviour
    {
        //지상개폐기의 부분인데... 터치로 무얼할 지.. 예상해보는 시간을 가집시다...
        private AndroidPerm.AndroidPermission permissionCheck;

        private const int SINGLE_PERMISSION = 0;
        private const int MULTIPLE_PERMISSION = 1;

        //public PermissionPopup popup;

        public string permission;
        public string[] permissions;

        public string[] needPermissions;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab for tracking and visualizing detected planes.
        /// </summary>
        public GameObject DetectedPlanePrefab;

        /// <summary>
        /// A model to place when a raycast from a user touch hits a plane.
        /// </summary>
        public GameObject AndyAndroidPrefab;

        /// <summary>
        /// A gameobject parenting UI for displaying the "searching for planes" snackbar.
        /// </summary>
        public GameObject SearchingForPlaneUI;

        /// <summary>
        /// The rotation in degrees need to apply to model when the Andy model is placed.
        /// </summary>
        private const float k_ModelRotation = 180.0f;

        /// <summary>
        /// A list to hold all planes ARCore is tracking in the current frame. This object is used across
        /// the application to avoid per-frame allocations.
        /// </summary>
        private List<DetectedPlane> m_AllPlanes = new List<DetectedPlane>();

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error, otherwise false.
        /// </summary>
        private bool m_IsQuitting = false;


        private int poleCount;

        ///
        private float theta = 0.0f;


        /// <summary>
        /// The electric pole can be built if elPole is true.
        /// </summary>
        private bool elPole;
        public GameObject supportedLine; //지선 담을 변수
        private Vector3 polePos; // 전신주와 지선의 설치위치
        private Vector3 supportPos;
        private float distance; //전신주와 지선의 거리를 저장
        private int supportCount; //지선의 개수를 1개로 제한
        public GameObject rotation;
        public GameObject addRotation;
        private bool setRotation;
        //public GameObject secondLine;
        private bool changeScale = false;
        //public GameObject scaleObject;
        private Transform poleTrans;
        public GameObject supportSize;
        private float size = 13.0f;
        //public GameObject topCapture;
        public GameObject bottomCapture;
        private Quaternion poleRotate;
        public GameObject rotationBTN;
        /// <summary>
        /// the type of electric poles.
        /// </summary>

        public Dropdown type;
        public GameObject first;
        public GameObject second;
        public GameObject third;
        public GameObject fourth;
        public GameObject five;
        public GameObject six;
        public GameObject seven;
        public GameObject eight;
        public GameObject nine;

        GameObject[] gears;
        public string BundleURL;
        public string[] AssetName;
        
        public GameObject moreBTN;
        public GameObject deleteBTN;
        public GameObject listInOBJ;
        public GameObject switchBTN;

        private int[] fullshotNUM;

        //private bool seeMore = false;

        private bool toss = false;

        public Text currentOBJ;
        private int chance;

        //public GameObject fullshot;
        public GameObject moves;

        public Sprite more;
        public Sprite hide;

        int doorCount = 0;

        private GameObject[] one = new GameObject[2];
        private GameObject[] two = new GameObject[2];
        private GameObject[] three = new GameObject[2];
        private GameObject leftModel;
        private GameObject rightModel;


        int planeCount = 0;
        public GameObject DetectedPlaneGuide;
        public GameObject phoneIcon;
        public GameObject[] planeGuidePath;
        private Vector3[] planePath;

        int InstantiateCount = 0;
        public GameObject InstantiateGuide;
        public GameObject tapIcon1;
        public GameObject tapIcon2;
        public GameObject pointing1;
        public GameObject pointing2;

        float moveTime = 0f;
        int MoveCount = 0;
        public GameObject MoveGuide;
        public GameObject moveIcon;
        public GameObject[] moveGuidePath;
        private Vector3[] movePath;

        float rotationTime = 0f;
        int RotationCount = 0;
        public GameObject rotationGuide;
        public GameObject twoTouchIcon;
        public GameObject triangle;

        int[] doorCountFullshot = { 0, 0, 0 };

        string[] kinds = { "개폐기 수동 3회로", "개폐기 수동 4회로", "개폐기 자동 3회로", "개폐기 자동 4회로", "변압기 50KVA", "변압기 75KVA", "변압기 200KVA", "변압기 300KVA", "변압기 500KVA" };
        /// <summary>
        /// possible is true, we can build electric pole. but if not, we can build supported line.
        /// </summary>
        /// <param name="possible">If set to <c>true</c> possible.</param>
        public void buildElPole(bool possible)
        {
            if (possible == true)
            {
                elPole = true;
                switch (type.value)
                {
                    case 0:
                        AndyAndroidPrefab = first;
                        setRotation = false;
                        break;
                    case 1:
                        AndyAndroidPrefab = second;
                        setRotation = false;
                        break;
                    case 2:
                        setRotation = false;
                        AndyAndroidPrefab = third;
                        break;
                    case 3:
                        setRotation = false;
                        AndyAndroidPrefab = fourth;
                        break;
                    case 4:
                        setRotation = true;
                        AndyAndroidPrefab = five;
                        break;
                    case 5:
                        setRotation = true;
                        AndyAndroidPrefab = six;
                        break;
                    case 6:
                        setRotation = true;
                        AndyAndroidPrefab = seven;
                        break;
                    case 7:
                        setRotation = true;
                        AndyAndroidPrefab = eight;
                        break;
                }
            }
            //else
            //{
            //    if (seeMore == true)
            //    {
            //        elPole = false;
            //        AndyAndroidPrefab = supportedLine;
            //        addRotation.SetActive(true);
            //        //supportSize.SetActive (true);
            //        setRotation = true;
            //        ExecMenu(seeMore);
            //    }

            //    else {
            //        ExecMenu(seeMore);
            //    }

            //}
            if (possible)
            {
                toss = false;
                ActiveFullshot(toss);
            }
            else {
                toss = true;
                ActiveFullshot(toss);
            }
        }

        public void isSetRotation(bool yeah)
        {
            if (yeah == true)
            {

                //secondLine.SetActive(true);
                // topCapture.SetActive(true);
                bottomCapture.SetActive(false);
                rotation.SetActive(true);
                if (elPole == true)
                {
                    // secondLine.SetActive(true);
                    rotation.SetActive(true);
                    addRotation.SetActive(false);
                }
                else
                {
                    addRotation.SetActive(true);
                }
                if (changeScale == true)
                {
                    changeScale = false;
                    sizeControl(changeScale);
                }

            }
            else
            {
                //secondLine.SetActive(false);
                //topCapture.SetActive(false);
                bottomCapture.SetActive(true);
                rotation.SetActive(false);
                if (elPole == true)
                {
                    addRotation.SetActive(false);
                }
                else
                {
                    addRotation.SetActive(true);
                }
            }
        }
        public void rotationLeft()
        {
            if (elPole == false && supportCount == 1)
            {
                GameObject.Find("support").transform.Rotate(0, -10, 0, Space.World);
            }
            else if (elPole == true && poleCount == 1)
            {
                GameObject.Find("pole").transform.Rotate(0, -10, 0, Space.World);
            }
        }

        public void rotationRight()
        {
            if (elPole == false && supportCount == 1)
            {
                GameObject.Find("support").transform.Rotate(0, 10, 0, Space.World);

            }
            else if (elPole == true && poleCount == 1)
            {
                GameObject.Find("pole").transform.Rotate(0, 10, 0, Space.World);
            }
        }
        public void rotationUp()
        {
            if (elPole == false && supportCount == 1 && theta > -65)
            {
                GameObject.Find("support").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                GameObject.Find("support").transform.Rotate(5, 0, 0, Space.Self);
                theta -= 5;
                float distance2 = distance * (1 / (float)Math.Cos((theta + 70.0f) * (Math.PI / 180)));
                transformSupport(size, distance2);
            }
        }
        public void rotationDown()
        {
            if (elPole == false && supportCount == 1 && theta < 15)
            {
                GameObject.Find("support").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                GameObject.Find("support").transform.Rotate(-5, 0, 0, Space.Self);
                theta += 5;
                float distance2 = distance * (1 / (float)Math.Cos((theta + 70.0f) * (Math.PI / 180)));
                transformSupport(size, distance2);
            }
        }
        public void sizeUp()
        {
            if (elPole == false && supportCount == 1)
            {
                GameObject.Find("support").transform.localScale += new Vector3(0, 0.1f, 0);
            }
        }
        public void sizeDown()
        {
            if (elPole == false && supportCount == 1)
            {
                GameObject.Find("support").transform.localScale -= new Vector3(0, 0.1f, 0);
            }
        }
        public void showRotation()
        {
            if (setRotation)
            {
                setRotation = false;
            }
            else
            {
                setRotation = true;
            }
            isSetRotation(setRotation);
        }

        public void controlSize()
        {
            if (changeScale)
            {
                changeScale = false;
            }
            else
            {
                changeScale = true;
            }
            sizeControl(changeScale);

        }
        private void sizeControl(bool yes)
        {
            if (yes)
            {
                if (setRotation == true)
                {
                    setRotation = false;
                    isSetRotation(setRotation);
                }
                //secondLine.SetActive(true);
                // topCapture.SetActive(true);
                // bottomCapture.SetActive(false);
                // scaleObject.SetActive(true);
            }
            else
            {
                // secondLine.SetActive(false);
                // topCapture.SetActive(false);
                bottomCapture.SetActive(true);
                //  scaleObject.SetActive(false);
            }
        }

        public void delete()
        {
            if (elPole == true)
            {
                Destroy(GameObject.Find("pole"));
                poleCount = 0;
                DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 삭제했음");
            }
            else
            {
                Destroy(GameObject.Find("support"));
                supportCount = 0;
            }
            if (toss == true)
            {
                switch (chance)
                {
                    case 0:
                        Destroy(GameObject.Find("one"));
                        fullshotNUM[0] = 0;
                        DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "복수건설 1번개폐기를 삭제했음");
                        break;
                    case 1:
                        Destroy(GameObject.Find("two"));
                        fullshotNUM[1] = 0;
                        DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "복수건설 2번개폐기를 삭제했음");
                        break;
                    case 2:
                        Destroy(GameObject.Find("three"));
                        fullshotNUM[2] = 0;
                        DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "복수건설 2번개폐기를 삭제했음");
                        break;

                }
            }
        }
        private void transformSupport(float length, float dist)
        {
            length = 13.0f;
            float deltaSize = length * 0.01f;
            GameObject.Find("support").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            if (length < dist)
            {
                while (length < dist)
                {
                    GameObject.Find("support").transform.localScale += new Vector3(0, 0.01f, 0);
                    length += deltaSize;
                }
            }
            else
            {
                while (length > dist)
                {
                    GameObject.Find("support").transform.localScale -= new Vector3(0, 0.01f, 0);
                    length -= deltaSize;
                }
            }

        }
        /*public void activeMenu(){
			SceneManager.LoadScene (4);
		}*/


        public void onFullshot()
        {
            if (elPole == true)
            {
                if (toss == true)
                {
                    toss = false;
                    ActiveFullshot(toss);

                }
                else
                {
                    toss = true;
                    ActiveFullshot(toss);
                }
            }
            else
            {
                toss = false;
            }

        }

        private void ActiveFullshot(bool howDo)
        {
            if (howDo == true)
            {
                if (poleCount == 1)
                {
                    Destroy(GameObject.Find("pole"));
                    poleCount = 0;
                }
                if (supportCount == 1)
                {
                    Destroy(GameObject.Find("support"));
                    supportCount = 0;
                }
                switchBTN.SetActive(true);
                moves.SetActive(true);
            }
            else
            {
                switchBTN.SetActive(true);
                moves.SetActive(false);
                Destroy(GameObject.Find("one"));
                Destroy(GameObject.Find("two"));
                Destroy(GameObject.Find("three"));
                fullshotNUM[0] = 0;
                fullshotNUM[1] = 0;
                fullshotNUM[2] = 0;
            }
        }

        public void UpperMove()
        {
            switch (currentOBJ.GetComponent<Text>().text)
            {
                case "1":
                    GameObject.Find("one").transform.position += new Vector3(0f, 0.1f, 0f);
                    break;
                case "2":
                    GameObject.Find("two").transform.position += new Vector3(0f, 0.1f, 0f);
                    break;
                case "3":
                    GameObject.Find("three").transform.position += new Vector3(0f, 0.1f, 0f);
                    break;
            }
        }
        public void Backward()
        {
            switch (currentOBJ.GetComponent<Text>().text)
            {
                case "1":
                    GameObject.Find("one").transform.position += new Vector3(0f, -0.1f, 0f);
                    break;
                case "2":
                    GameObject.Find("two").transform.position += new Vector3(0f, -0.1f, 0f);
                    break;
                case "3":
                    GameObject.Find("three").transform.position += new Vector3(0f, -0.1f, 0f);
                    break;
            }
        }
        public void GiveChance()
        {
            chance += 1;
            chance %= 3;
            switch (chance)
            {
                case 0:
                    currentOBJ.text = "1";
                    GameObject.Find("one").GetComponent<LeanSelectable>().enabled = true;
                    GameObject.Find("two").GetComponent<LeanSelectable>().enabled = false;
                    GameObject.Find("three").GetComponent<LeanSelectable>().enabled = false;
                    break;
                case 1:
                    currentOBJ.text = "2";
                    GameObject.Find("one").GetComponent<LeanSelectable>().enabled = false;
                    GameObject.Find("two").GetComponent<LeanSelectable>().enabled = true;
                    GameObject.Find("three").GetComponent<LeanSelectable>().enabled = false;
                    break;
                case 2:
                    currentOBJ.text = "3";
                    GameObject.Find("one").GetComponent<LeanSelectable>().enabled = false;
                    GameObject.Find("two").GetComponent<LeanSelectable>().enabled = false;
                    GameObject.Find("three").GetComponent<LeanSelectable>().enabled = true;
                    break;
            }

        }

        public void openDoor() {
            if (poleCount == 1 && doorCount == 0 && toss == false) {
                StartCoroutine(DoorAction());
            }
            else if (toss == true && doorCountFullshot[chance] == 0) {
                StartCoroutine(DoorAction());
            }
        }
        IEnumerator DoorAction() {

            if (toss == false) {
                doorCount = 1;
                iTween.RotateAdd(GameObject.Find("pole").transform.Find("796").gameObject, iTween.Hash("y", 120));
                iTween.RotateAdd(GameObject.Find("pole").transform.Find("1117").gameObject, iTween.Hash("y", -120));
                yield return new WaitForSeconds(2);
                iTween.RotateAdd(GameObject.Find("pole").transform.Find("796").gameObject, iTween.Hash("y", -120));
                iTween.RotateAdd(GameObject.Find("pole").transform.Find("1117").gameObject, iTween.Hash("y", 120));
                yield return new WaitForSeconds(0.2f);
            }

            if (toss == true) {

                switch (chance)
                {
                    case 0:
                        doorCountFullshot[0] = 1;
                        leftModel = one[0];
                        rightModel = one[1];
                        break;
                    case 1:
                        doorCountFullshot[1] = 1;
                        leftModel = two[0];
                        rightModel = two[1];
                        break;
                    case 2:
                        doorCountFullshot[2] = 1;
                        leftModel = three[0];
                        rightModel = three[1];
                        break;
                }
                iTween.RotateAdd(leftModel, iTween.Hash("y", 120));
                iTween.RotateAdd(rightModel, iTween.Hash("y", -120));
                yield return new WaitForSeconds(2);
                iTween.RotateAdd(leftModel, iTween.Hash("y", -120));
                iTween.RotateAdd(rightModel, iTween.Hash("y", 120));
                yield return new WaitForSeconds(0.2f);
            }
            doorCount = 0;
            doorCountFullshot[chance] = 0;
        }

        private void Start()
        {
            StartCoroutine(downLoadGears());
            elPole = true;
            poleCount = 0;
            supportCount = 0;
            fullshotNUM = new int[3] { 0, 0, 0 };
            chance = 0;
            doorCount = 0;
            setRotation = false;
            toss = false;
            rotation.SetActive(false);
            AndyAndroidPrefab = first;
            // secondLine.SetActive(false);
            // topCapture.SetActive(false);
            bottomCapture.SetActive(true);
            supportSize.SetActive(false);
            // scaleObject.SetActive(false);
            rotationBTN.SetActive(true);


            planePath = new Vector3[planeGuidePath.Length];
            for (int i = 0; i < 5; i++)
            {
                planePath[i] = planeGuidePath[i].transform.position;
            }
            DetectedPlaneGuide.SetActive(false);



            InstantiateGuide.SetActive(false);



            movePath = new Vector3[moveGuidePath.Length];
            for (int i = 0; i < 5; i++)
            {
                movePath[i] = moveGuidePath[i].transform.position;
            }
            MoveGuide.SetActive(false);
            moves.SetActive(false);
            rotationGuide.SetActive(false);

            if (permissionCheck == null)
            {
                permissionCheck = new AndroidPerm.AndroidPermission();
                permissionCheck.Init();

                permissionCheck.OnCheckExplainAction = OnCheckExplain;
                permissionCheck.OnCheckNonExplainAction = OnCheckNonExplain;
                permissionCheck.OnCheckAlreadyAction = OnCheckAlready;
                permissionCheck.OnCheckFailedAction = OnCheckFailed;

                permissionCheck.OnResultAction = OnRequestResult;
            }


            permissionCheck.CheckPermission("android.permission.WRITE_EXTERNAL_STORAGE", 0);
            permissionCheck.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE", 0);
            string data = PlayerPrefs.GetString("userName") + "\n@" + PlayerPrefs.GetString("dept");
            GameObject.Find("Example Controller").SendMessage("getMyInfo", data);
            //permissionCheck.ShowDialog ("android.permission.WRITE_EXTERNAL_STORAGE", 0, "please", "please allow this");

        }
        IEnumerator downLoadGears() {
            using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL,0)) {
                yield return www;
                if (www.error != null) {
                    throw new Exception("WWW download had an error : " + www.error);
                }

                AssetBundle bundle = www.assetBundle;
                first = Instantiate(bundle.LoadAsset(AssetName[0])) as GameObject;
                second = Instantiate(bundle.LoadAsset(AssetName[1])) as GameObject;
                third = Instantiate(bundle.LoadAsset(AssetName[2])) as GameObject;
                fourth = Instantiate(bundle.LoadAsset(AssetName[3])) as GameObject;
                five = Instantiate(bundle.LoadAsset(AssetName[4])) as GameObject;
                six = Instantiate(bundle.LoadAsset(AssetName[5])) as GameObject;
                seven = Instantiate(bundle.LoadAsset(AssetName[6])) as GameObject;
                eight = Instantiate(bundle.LoadAsset(AssetName[7])) as GameObject;
                nine = Instantiate(bundle.LoadAsset(AssetName[8])) as GameObject;
                yield return new WaitForSeconds(0.1f);
                first.transform.position = new Vector3(-2000,-6000,-42);
                second.transform.position = new Vector3(-1900, -6000, -42);
                third.transform.position = new Vector3(-2100, -6000, -42);
                fourth.transform.position = new Vector3(-2200, -6000, -42);
                five.transform.position = new Vector3(-2300, -6000, -42);
                six.transform.position = new Vector3(-2400, -6000, -42);
                seven.transform.position = new Vector3(-2500, -6000, -42);
                eight.transform.position = new Vector3(-2600, -6000, -42);
                nine.transform.position = new Vector3(-2700, -6000, -42);
                yield return new WaitForSeconds(0.1f);
                bundle.Unload(false);
            }
        }
        public void ListenFromMenu(int sig)
        {
            switch (sig)
            {
                case 1:
                    AndyAndroidPrefab = first;
                    Debug.Log(sig);
                    type.value = 0;
                    break;
                case 2:
                    AndyAndroidPrefab = second;
                    Debug.Log(sig);
                    type.value = 1;
                    break;
                case 3:
                    AndyAndroidPrefab = third;
                    type.value = 2;
                    Debug.Log(sig);
                    break;
                case 4:
                    AndyAndroidPrefab = fourth;
                    type.value = 3;
                    Debug.Log(sig);
                    break;
                case 5:
                    AndyAndroidPrefab = five;
                    type.value = 4;
                    Debug.Log(sig);
                    break;
                case 6:
                    AndyAndroidPrefab = six;
                    type.value = 5;
                    Debug.Log(sig);
                    break;
                case 7:
                    AndyAndroidPrefab = seven;
                    type.value = 6;
                    Debug.Log(sig);
                    break;
                case 8:
                    AndyAndroidPrefab = eight;
                    type.value = 7;
                    Debug.Log(sig);
                    break;
                case 9:
                    AndyAndroidPrefab = eight;
                    type.value = 8;
                    Debug.Log(sig);
                    break;
            }
        }
        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            _UpdateApplicationLifecycle();

            // Hide snackbar when currently tracking at least one plane.
            Session.GetTrackables<DetectedPlane>(m_AllPlanes);
            bool showSearchingUI = true;
            for (int i = 0; i < m_AllPlanes.Count; i++)
            {
                if (m_AllPlanes[i].TrackingState == TrackingState.Tracking)
                {
                    showSearchingUI = false;
                    break;
                }
            }

            SearchingForPlaneUI.SetActive(showSearchingUI);

            if (showSearchingUI == true && planeCount == 0) {
                DetectedPlaneGuide.SetActive(true);
                DataTransmission.instance.TrackedPlaneGuide(phoneIcon, planePath);
                planeCount = 1;
            }
            if (showSearchingUI == false && planeCount == 1) {
                DetectedPlaneGuide.SetActive(false);
                planeCount = 2;
            }
            if(showSearchingUI == true && planeCount > 1)
            {
                DetectedPlaneGuide.SetActive(true);
                planeCount = 1;
            }
            if (showSearchingUI == false && poleCount == 0 && toss == false && InstantiateCount == 0) {
                InstantiateGuide.SetActive(true);
                DataTransmission.instance.InstantiateObjeactGuide(tapIcon1, tapIcon2, pointing1, pointing2);
                InstantiateCount = 1;
            }
            if (((showSearchingUI == false && poleCount == 1) || toss == true) && InstantiateCount == 1) {
                InstantiateGuide.SetActive(false);
                InstantiateCount = 2;
            }
            if (poleCount == 1 && MoveCount == 0) {
                MoveGuide.SetActive(true);
                DataTransmission.instance.MoveControlGuide(moveIcon, movePath);
                MoveCount = 1;
                
            }
            if (MoveCount == 1)
            {
                moveTime += Time.deltaTime;
                if (moveTime > 5) {
                    MoveGuide.SetActive(false);
                    MoveCount = 2;
                }
                
            }

            if (MoveCount == 2 && poleCount == 1 && RotationCount == 0) {
                rotationGuide.SetActive(true);
                DataTransmission.instance.RotationControlGuide(twoTouchIcon, triangle);
                RotationCount = 1;
            }
            if (MoveCount == 2 && poleCount == 1 && RotationCount == 1 && rotationGuide.activeSelf == true)
            {
                rotationTime += Time.deltaTime;
                if (rotationTime > 5) {
                    rotationGuide.SetActive(false);
                    MoveCount = 3;
                    RotationCount = 2;
                }
                
            }
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }
            if (toss == true)
            {
                switch (type.value)
                {
                    case 0:
                        AndyAndroidPrefab = first;


                        break;
                    case 1:
                        AndyAndroidPrefab = second;


                        break;
                    case 2:
                        AndyAndroidPrefab = third;


                        break;
                    case 3:


                        AndyAndroidPrefab = fourth;
                        break;
                    case 4:
                        AndyAndroidPrefab = five;


                        break;
                    case 5:
                        AndyAndroidPrefab = six;


                        break;
                    case 6:
                        AndyAndroidPrefab = seven;


                        break;
                    case 7:
                        AndyAndroidPrefab = eight;

                        break;

                    case 8:
                        AndyAndroidPrefab = nine;

                        break;
                }
            }
            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;
            if (poleCount == 1 && elPole == true && toss == false)
            {
                switch (type.value)
                {
                    case 0:
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != first)
                        {
                            AndyAndroidPrefab = first;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 1:
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != second)
                        {
                            AndyAndroidPrefab = second;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 2:
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != third)
                        {
                            AndyAndroidPrefab = third;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 3:
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != fourth)
                        {
                            AndyAndroidPrefab = fourth;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 4:
                        rotationBTN.SetActive(true);
                        if (AndyAndroidPrefab != five)
                        {
                            AndyAndroidPrefab = five;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 5:
                        rotationBTN.SetActive(true);
                        if (AndyAndroidPrefab != six)
                        {
                            AndyAndroidPrefab = six;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 6:
                        rotationBTN.SetActive(true);
                        if (AndyAndroidPrefab != seven)
                        {
                            AndyAndroidPrefab = seven;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 7:
                        rotationBTN.SetActive(true);
                        if (AndyAndroidPrefab != eight)
                        {
                            AndyAndroidPrefab = eight;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                    case 8:
                        rotationBTN.SetActive(true);
                        if (AndyAndroidPrefab != eight)
                        {
                            AndyAndroidPrefab = nine;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                            /*changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);*/
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        }
                        break;
                }

            }

            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && poleCount == 0 && elPole == true && toss == false)
            {
                // Use hit pose and camera pose to check if hittest is from the
                // back of the plane, if it is, no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    switch (type.value)
                    {
                        case 0:
                            AndyAndroidPrefab = first;

                            break;
                        case 1:
                            AndyAndroidPrefab = second;
 

                            break;
                        case 2:
                            AndyAndroidPrefab = third;


                            break;
                        case 3:


                            AndyAndroidPrefab = fourth;
                            break;
                        case 4:
                            AndyAndroidPrefab = five;
                            
                            break;
                        case 5:
                            AndyAndroidPrefab = six;
                            
                            break;
                        case 6:
                            AndyAndroidPrefab = seven;
                            
                            break;
                        case 7:
                            AndyAndroidPrefab = eight;
                            
                            break;
                        case 8:
                            AndyAndroidPrefab = nine;
                            
                            break;
                    }
                    poleCount += 1;
                    // Instantiate Andy model at the hit pose.
                    var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                    andyObject.name = "pole";
                    GameObject.Find("pole").transform.Rotate(0, -90, 0, Space.World);
                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    //andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    polePos = hit.Pose.position;
                    poleRotate = hit.Pose.rotation;
                    // Make Andy model a child of the anchor.
                    andyObject.transform.parent = anchor.transform;
                    poleTrans = andyObject.transform;
                    if (supportCount == 1)
                    {
                        GameObject.Find("support").transform.LookAt(poleTrans);
                        GameObject.Find("support").transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                        GameObject.Find("support").transform.Rotate(20, 0, 0);
                        theta = 0.0f;
                        size = 13.0f;
                        distance = Vector3.Distance(polePos, supportPos);
                        float distance2 = distance * (1 / (float)Math.Cos((theta + 70.0f) * (Math.PI / 180)));
                        transformSupport(size, distance2);
                    }
                    DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                    //GameObject.Find("pole").GetComponent<LeanSelectable>().enabled = false;
                }
            }
            
            else if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && elPole == true && toss == true)
            {
                switch (type.value)
                {
                    case 0:
                        AndyAndroidPrefab = first;


                        break;
                    case 1:
                        AndyAndroidPrefab = second;


                        break;
                    case 2:
                        AndyAndroidPrefab = third;


                        break;
                    case 3:


                        AndyAndroidPrefab = fourth;
                        break;
                    case 4:
                        AndyAndroidPrefab = five;
                        

                        break;
                    case 5:
                        AndyAndroidPrefab = six;
                        

                        break;
                    case 6:
                        AndyAndroidPrefab = seven;
                        

                        break;
                    case 7:
                        AndyAndroidPrefab = eight;
                        
                        break;
                    case 8:
                        AndyAndroidPrefab = nine;

                        break;
                }
                switch (chance)
                {
                    case 0:
                        if (fullshotNUM[0] == 0)
                        {
                            fullshotNUM[0]++;
                            var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                            andyObject.name = "one";
                            GameObject.Find("one").transform.Rotate(0, -90, 0, Space.World);
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            polePos = hit.Pose.position;
                            poleRotate = hit.Pose.rotation;
                            // Andy should look at the camera but still be flush with the plane.
                            //andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // Make Andy model a child of the anchor.
                            andyObject.transform.parent = anchor.transform;
                            poleTrans = andyObject.transform;
                            one[0] = GameObject.Find("one").transform.Find("796").gameObject;
                            one[1] = GameObject.Find("one").transform.Find("1117").gameObject;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + " 복수건설 1번 개폐기를 생성했음");
                        }
                        break;
                    case 1:
                        if (fullshotNUM[1] == 0)
                        {
                            fullshotNUM[1]++;
                            var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                            andyObject.name = "two";
                            GameObject.Find("two").transform.Rotate(0, -90, 0, Space.World);
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            polePos = hit.Pose.position;
                            poleRotate = hit.Pose.rotation;
                            // Andy should look at the camera but still be flush with the plane.
                            //andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // Make Andy model a child of the anchor.
                            andyObject.transform.parent = anchor.transform;
                            poleTrans = andyObject.transform;
                            two[0] = GameObject.Find("two").transform.Find("796").gameObject;
                            two[1] = GameObject.Find("two").transform.Find("1117").gameObject;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + " 복수건설 2번 개폐기를 생성했음");
                        }
                        break;
                    case 2:
                        if (fullshotNUM[2] == 0)
                        {
                            fullshotNUM[2]++;
                            var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                            andyObject.name = "three";
                            GameObject.Find("three").transform.Rotate(0, -90, 0, Space.World);
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            polePos = hit.Pose.position;
                            poleRotate = hit.Pose.rotation;
                            // Andy should look at the camera but still be flush with the plane.
                            //andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // Make Andy model a child of the anchor.
                            andyObject.transform.parent = anchor.transform;
                            poleTrans = andyObject.transform;
                            three[0] = GameObject.Find("three").transform.Find("796").gameObject;
                            three[1] = GameObject.Find("three").transform.Find("1117").gameObject;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + " 복수건설 3번 전주를 생성했음");
                        }
                        break;
                }
                if (poleCount == 1 && elPole == true) {

                }


            }
        }
        private void FixedUpdate()
        {
            if (toss == false && poleCount == 1)
            {
                if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    GameObject.Find("pole").GetComponent<LeanTranslate>().enabled = true;
                    GameObject.Find("pole").GetComponent<LeanRotate1>().enabled = false;
                }
                if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    GameObject.Find("pole").GetComponent<LeanTranslate>().enabled = false;
                    GameObject.Find("pole").GetComponent<LeanRotate1>().enabled = true;
                }
            }
            if (toss == true)
            {
                if (fullshotNUM[chance] == 1 && Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                {
                    switch (chance)
                    {
                        case 0:
                            GameObject.Find("one").GetComponent<LeanTranslate>().enabled = false;
                            GameObject.Find("one").GetComponent<LeanRotate1>().enabled = true;
                            break;
                        case 1:
                            GameObject.Find("two").GetComponent<LeanTranslate>().enabled = false;
                            GameObject.Find("two").GetComponent<LeanRotate1>().enabled = true;
                            break;

                        case 2:
                            GameObject.Find("three").GetComponent<LeanTranslate>().enabled = false;
                            GameObject.Find("three").GetComponent<LeanRotate1>().enabled = true;
                            break;
                    }
                }

                if (fullshotNUM[chance] == 1  && Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
                {
                    switch (chance)
                    {
                        case 0:
                            GameObject.Find("one").GetComponent<LeanTranslate>().enabled = true;
                            GameObject.Find("one").GetComponent<LeanRotate1>().enabled = false;
                            break;
                        case 1:
                            GameObject.Find("two").GetComponent<LeanTranslate>().enabled = true;
                            GameObject.Find("two").GetComponent<LeanRotate1>().enabled = false;
                            break;

                        case 2:
                            GameObject.Find("three").GetComponent<LeanTranslate>().enabled = true;
                            GameObject.Find("three").GetComponent<LeanRotate1>().enabled = false;
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void _UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                //Application.Quit();
                SceneManager.LoadScene("chooseMode");
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                const int lostTrackingSleepTimeout = 15;
                Screen.sleepTimeout = lostTrackingSleepTimeout;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (m_IsQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                _ShowAndroidToastMessage("Camera permission is needed to run this application.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                _ShowAndroidToastMessage("ARCore encountered a problem connecting.  Please start the app again.");
                m_IsQuitting = true;
                Invoke("_DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void _DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void _ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject = toastClass.CallStatic<AndroidJavaObject>("makeText", unityActivity,
                        message, 0);
                    toastObject.Call("show");
                }));
            }
        }
        public void OnCheckExplain(AndroidPerm.CheckEventArgs args)
        {
            // Show Explain Dialog
            if (permissionCheck != null)
            {
                permissionCheck.ShowDialog(permission, 0, "Request Permission", "Need allow permission.");
            }
        }

        public void OnCheckNonExplain(AndroidPerm.CheckEventArgs args)
        {
            if (permissionCheck != null)
            {
                permissionCheck.RequestPermission(args.permission, 0);
            }
        }

        public void OnCheckAlready(AndroidPerm.CheckEventArgs args)
        {

        }

        public void OnCheckFailed(AndroidPerm.ErrorEventArgs errArgs)
        {

        }

        public void OnRequestResult(AndroidPerm.ResultEventArgs args)
        {
            if (args.denined.Length > 0)
            {
                bool forceQuit = false;

                for (int i = 0; i < args.denined.Length; i++)
                {
                    for (int j = 0; j < needPermissions.Length; j++)
                    {
                        if (args.denined[i] == needPermissions[j])
                        {
                            forceQuit = true;
                            break;
                        }
                    }

                    if (forceQuit) break;
                }
                if (forceQuit)
                {
                    Application.Quit();
                }
            }

        }
    }
}

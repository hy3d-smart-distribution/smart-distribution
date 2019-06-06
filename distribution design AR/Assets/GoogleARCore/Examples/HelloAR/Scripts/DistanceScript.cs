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
    public class DistanceScript : MonoBehaviour
    {
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

        /// <summary>
        /// An
        /// </summary>
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
        public GameObject secondLine;
        private bool changeScale = false;
        public GameObject scaleObject;
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

        public GameObject moreBTN;
        public GameObject deleteBTN;
        public GameObject listInOBJ;
        public GameObject switchBTN;

        private int[] fullshotNUM;

        private bool seeMore = false;

        private bool toss = false;

        public Text currentOBJ;
        private int chance;

        public GameObject fullshot;
        public GameObject moves;

        public Sprite more;
        public Sprite hide;

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

        public string BundleURL;
        public string AssetName;

        /// <summary>
        /// possible is true, we can build electric pole. but if not, we can build supported line.
        /// </summary>
        /// <param name="possible">If set to <c>true</c> possible.</param>
        public void buildElPole(bool possible)
        {
            if (possible == true)
            {
                elPole = true;
                if (seeMore == true)
                {
                    switch (type.value)
                    {
                        case 0:
                            AndyAndroidPrefab = first;
                            setRotation = false;
                            rotationBTN.SetActive(false);
                            break;
                        case 1:
                            AndyAndroidPrefab = second;
                            setRotation = false;
                            rotationBTN.SetActive(false);
                            break;
                        case 2:
                            setRotation = false;
                            AndyAndroidPrefab = third;
                            rotationBTN.SetActive(false);
                            break;
                        case 3:
                            setRotation = false;
                            AndyAndroidPrefab = fourth;
                            rotationBTN.SetActive(false);
                            break;
                        case 4:
                            setRotation = true;
                            AndyAndroidPrefab = five;
                            rotationBTN.SetActive(true);
                            break;
                        case 5:
                            setRotation = true;
                            AndyAndroidPrefab = six;
                            rotationBTN.SetActive(true);
                            break;
                        case 6:
                            setRotation = true;
                            AndyAndroidPrefab = seven;
                            rotationBTN.SetActive(true);
                            break;
                        case 7:
                            setRotation = true;
                            AndyAndroidPrefab = eight;
                            rotationBTN.SetActive(true);
                            break;
                    }
                    ExecMenu(seeMore);
                }
                else {
                    ExecMenu(seeMore);
                }
            }
            else
            {
                if (seeMore == true)
                {
                    elPole = false;
                    AndyAndroidPrefab = supportedLine;
                    addRotation.SetActive(true);
                    //supportSize.SetActive (true);
                    setRotation = true;
                    rotationBTN.SetActive(true);
                    ExecMenu(seeMore);
                }

                else {
                    ExecMenu(seeMore);
                }
                
            }
        }

        public void isSetRotation(bool yeah)
        {
            if (yeah == true)
            {

                secondLine.SetActive(true);
                //topCapture.SetActive(true);
                bottomCapture.SetActive(false);
                rotation.SetActive(true);
                if (elPole == true)
                {
                    secondLine.SetActive(true);
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
                secondLine.SetActive(false);
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
                secondLine.SetActive(true);
                //topCapture.SetActive(true);
                bottomCapture.SetActive(false);
                scaleObject.SetActive(true);
            }
            else
            {
                secondLine.SetActive(false);
               // topCapture.SetActive(false);
                bottomCapture.SetActive(true);
                scaleObject.SetActive(false);
            }
        }

        public void delete()
        {
            if (elPole == true)
            {
                Destroy(GameObject.Find("pole"));
                poleCount = 0;
                iTween.MoveBy(deleteBTN, iTween.Hash("y", -300, "easeType", "easeInOutExpo", "delay", .1));
                DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), "이격거리 전주를 삭제했음");
            }
            else
            {
                Destroy(GameObject.Find("support"));
                supportCount = 0;
            }
            /*if (toss == true)
            {
                switch (chance)
                {
                    case 0:
                        Destroy(GameObject.Find("one"));
                        fullshotNUM[0] = 0;
                        break;
                    case 1:
                        Destroy(GameObject.Find("two"));
                        fullshotNUM[1] = 0;
                        break;
                    case 2:
                        Destroy(GameObject.Find("three"));
                        fullshotNUM[2] = 0;
                        break;

                }
            }*/
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
        public void activeMenu()
        {
            if (seeMore == false)
            {
                seeMore = true;
                ExecMenu(seeMore);
            }
            else
            {
                seeMore = false;
                ExecMenu(seeMore);
            }
        }
        private void ExecMenu(bool howDo)
        {
            if (howDo == true)
            {
                moreBTN.GetComponent<Image>().sprite = hide;
                if (elPole == true)
                {
                    //전신주는 스케일이 필요가 없다. 안해도 되지만 스위칭 하는 과정에서 데이터를 받기때문에 다시 초기화 작업 시행
                    changeScale = false;
                    supportSize.SetActive(false);
                    sizeControl(changeScale);
                }
                else {
                    //지선은 미세한 스케일을 조정할 가능성이 있기 때문에 스케일 버튼을 켜야 한다.
                    supportSize.SetActive(false);
                    changeScale = false;
                    sizeControl(changeScale);
                }
                setRotation = false;
                isSetRotation(setRotation);
                rotationBTN.SetActive(false);
                listInOBJ.SetActive(false);
                deleteBTN.SetActive(true);
                fullshot.SetActive(false);
            }
            else
            {
                moreBTN.GetComponent<Image>().sprite = more;
                setRotation = false;
                isSetRotation(setRotation);
                rotationBTN.SetActive(false);
                listInOBJ.SetActive(false);
                deleteBTN.SetActive(false);
                fullshot.SetActive(false);
            }
        }

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
                switchBTN.SetActive(false);
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
        public void ListenFromMenu() {
            Debug.Log("This is a Distance section.");
        }

        private void Start()
        {
            StartCoroutine(downLoadDist());
            elPole = true;
            poleCount = 0;
            supportCount = 0;
            fullshotNUM = new int[3] { 0, 0, 0 };
            chance = 0;
            setRotation = false;
            toss = false;
            seeMore = false;
            //ExecMenu(seeMore);
            rotation.SetActive(false);
            AndyAndroidPrefab = first;
            secondLine.SetActive(false);
            //topCapture.SetActive(false);
            bottomCapture.SetActive(true);
            supportSize.SetActive(false);
            scaleObject.SetActive(false);
            rotationBTN.SetActive(false);
            moves.SetActive(false);
            //deleteBTN.SetActive(true);
            //iTween.MoveBy(deleteBTN, iTween.Hash("y", -300, "easeType", "easeInOutExpo", "delay", .1));

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

            //permissionCheck.ShowDialog ("android.permission.WRITE_EXTERNAL_STORAGE", 0, "please", "please allow this");
            string data = PlayerPrefs.GetString("userName") + "\n@" + PlayerPrefs.GetString("dept");
            GameObject.Find("Example Controller").SendMessage("getMyInfo", data);
        }
        IEnumerator downLoadDist()
        {
            using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL,0))
            {
                yield return www;
                if (www.error != null)
                {
                    throw new Exception("WWW download had an error : " + www.error);
                }

                AssetBundle bundle = www.assetBundle;
                first = Instantiate(bundle.LoadAsset(AssetName)) as GameObject;

                yield return new WaitForSeconds(0.1f);
                first.transform.position = new Vector3(-2000, -6000, -42);

                yield return new WaitForSeconds(0.1f);
                bundle.Unload(false);
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
            if (showSearchingUI == true && planeCount == 0)
            {
                DetectedPlaneGuide.SetActive(true);
                DataTransmission.instance.TrackedPlaneGuide(phoneIcon, planePath);
                planeCount = 1;
            }
            if (showSearchingUI == false && planeCount == 1)
            {
                DetectedPlaneGuide.SetActive(false);
                planeCount = 2;
            }
            if (showSearchingUI == true && planeCount > 1)
            {
                DetectedPlaneGuide.SetActive(true);
                planeCount = 1;
            }
            if (showSearchingUI == false && poleCount == 0 && toss == false && InstantiateCount == 0)
            {
                InstantiateGuide.SetActive(true);
                DataTransmission.instance.InstantiateObjeactGuide(tapIcon1, tapIcon2, pointing1, pointing2);
                InstantiateCount = 1;
            }
            if (((showSearchingUI == false && poleCount == 1) || toss == true) && InstantiateCount == 1)
            {
                InstantiateGuide.SetActive(false);
                InstantiateCount = 2;
            }
            if (poleCount == 1 && MoveCount == 0)
            {
                MoveGuide.SetActive(true);
                DataTransmission.instance.MoveControlGuide(moveIcon, movePath);
                MoveCount = 1;

            }
            if (MoveCount == 1)
            {
                moveTime += Time.deltaTime;
                if (moveTime > 5)
                {
                    MoveGuide.SetActive(false);
                    MoveCount = 2;
                }

            }

            if (MoveCount == 2 && poleCount == 1 && RotationCount == 0)
            {
                if (type.value > 3 && type.value < 8)
                {
                    rotationGuide.SetActive(true);
                    DataTransmission.instance.RotationControlGuide(twoTouchIcon, triangle);
                    RotationCount = 1;
                }
            }
            if (MoveCount == 2 && poleCount == 1 && RotationCount == 1 && rotationGuide.activeSelf == true)
            {
                rotationTime += Time.deltaTime;
                if (rotationTime > 5)
                {
                    rotationGuide.SetActive(false);
                    MoveCount = 3;
                    RotationCount = 2;
                }

            }
            SearchingForPlaneUI.SetActive(showSearchingUI);

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
                        rotationBTN.SetActive(false);

                        break;
                    case 1:
                        AndyAndroidPrefab = second;
                        rotationBTN.SetActive(false);

                        break;
                    case 2:
                        AndyAndroidPrefab = third;
                        rotationBTN.SetActive(false);

                        break;
                    case 3:
                        rotationBTN.SetActive(false);

                        AndyAndroidPrefab = fourth;
                        break;
                    case 4:
                        AndyAndroidPrefab = five;
                        rotationBTN.SetActive(false);

                        break;
                    case 5:
                        AndyAndroidPrefab = six;
                        rotationBTN.SetActive(false);

                        break;
                    case 6:
                        AndyAndroidPrefab = seven;
                        rotationBTN.SetActive(false);

                        break;
                    case 7:
                        AndyAndroidPrefab = eight;
                        rotationBTN.SetActive(false);
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
                        rotationBTN.SetActive(false);
                        secondLine.SetActive(false);
                        rotation.SetActive(false);
                        addRotation.SetActive(false);
                        supportSize.SetActive(false);
                        //topCapture.SetActive(false);
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != first)
                        {
                            AndyAndroidPrefab = first;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
                            DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), "이격거리 전주를 생성했음");
                        }
                        break;
                    case 1:
                        rotationBTN.SetActive(false);
                        secondLine.SetActive(false);
                        rotation.SetActive(false);
                        addRotation.SetActive(false);
                        supportSize.SetActive(false);
                        //topCapture.SetActive(false);
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != second)
                        {
                            AndyAndroidPrefab = second;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
                        }
                        break;
                    case 2:
                        rotationBTN.SetActive(false);
                        secondLine.SetActive(false);
                        rotation.SetActive(false);
                        addRotation.SetActive(false);
                        supportSize.SetActive(false);
                        //topCapture.SetActive(false);
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != third)
                        {
                            AndyAndroidPrefab = third;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
                        }
                        break;
                    case 3:
                        rotationBTN.SetActive(false);
                        secondLine.SetActive(false);
                        rotation.SetActive(false);
                        addRotation.SetActive(false);
                        supportSize.SetActive(false);
                        //topCapture.SetActive(false);
                        bottomCapture.SetActive(true);
                        if (AndyAndroidPrefab != fourth)
                        {
                            AndyAndroidPrefab = fourth;
                            Destroy(GameObject.Find("pole"));
                            var changePole = Instantiate(AndyAndroidPrefab, polePos, poleRotate) as GameObject;
                            changePole.name = "pole";
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
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
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
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
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
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
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
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
                            changePole.transform.LookAt(FirstPersonCamera.transform);
                            changePole.transform.rotation = Quaternion.Euler(0.0f,
                                changePole.transform.rotation.eulerAngles.y, changePole.transform.rotation.z);
                            poleTrans = changePole.transform;
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
                            rotationBTN.SetActive(false);

                            break;
                        case 1:
                            AndyAndroidPrefab = second;
                            rotationBTN.SetActive(false);

                            break;
                        case 2:
                            AndyAndroidPrefab = third;
                            rotationBTN.SetActive(false);

                            break;
                        case 3:
                            rotationBTN.SetActive(false);

                            AndyAndroidPrefab = fourth;
                            break;
                        case 4:
                            AndyAndroidPrefab = five;
                            if (toss == true)
                            {
                                rotationBTN.SetActive(false);
                            }
                            else
                            {
                                rotationBTN.SetActive(true);
                            }
                            break;
                        case 5:
                            AndyAndroidPrefab = six;
                            if (toss == true)
                            {
                                rotationBTN.SetActive(false);
                            }
                            else {
                                rotationBTN.SetActive(true);
                            }
                            break;
                        case 6:
                            AndyAndroidPrefab = seven;
                            if (toss == true)
                            {
                                rotationBTN.SetActive(false);
                            }
                            else
                            {
                                rotationBTN.SetActive(true);
                            }
                            break;
                        case 7:
                            AndyAndroidPrefab = eight;
                            if (toss == true)
                            {
                                rotationBTN.SetActive(false);
                            }
                            else
                            {
                                rotationBTN.SetActive(true);
                            }
                            break;
                    }
                    poleCount += 1;
                    iTween.MoveBy(deleteBTN, iTween.Hash("y", 300, "easeType", "easeInOutExpo", "delay", .1));
                    // Instantiate Andy model at the hit pose.
                    var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                    andyObject.name = "pole";
                    // Compensate for the hitPose rotation facing away from the raycast (i.e. camera).
                    andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                    // world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                    polePos = hit.Pose.position;
                    poleRotate = hit.Pose.rotation;
                    // Make Andy model a child of the anchor.
                    andyObject.transform.parent = anchor.transform;
                    poleTrans = andyObject.transform;
                    DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), "이격거리 전주를 생성했음");
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
                }
            }
            else if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && supportCount == 0 && elPole == false && poleCount == 1 && toss == false) {
                supportCount += 1;
                var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                andyObject.name = "support";

                // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                // world evolves.
                var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                supportPos = hit.Pose.position;
                // Andy should look at the camera but still be flush with the plane.
                andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                //	andyObject.transform.rotation = Quaternion.Euler (0.0f,
                //		andyObject.transform.rotation.eulerAngles.y, andyObject.transform.rotation.z);
                GameObject.Find("support").transform.Rotate(20, 0, 0);
                distance = Vector3.Distance(polePos, supportPos);
                float expectedDist = distance * (1 / (float)Math.Cos((theta + 70.0f) * (Math.PI / 180)));
                transformSupport(size, expectedDist);
                secondLine.SetActive(true);
                rotation.SetActive(true);
                addRotation.SetActive(true);
                //topCapture.SetActive(true);
                bottomCapture.SetActive(false);
                //GameObject.Find("support").transform.parent = GameObject.Find("pole").transform;
                // Make Andy model a child of the anchor.
                //andyObject.transform.parent = anchor.transform;
            }
            else if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && elPole == true && toss == true)
            {
                switch (type.value)
                {
                    case 0:
                        AndyAndroidPrefab = first;
                        rotationBTN.SetActive(false);

                        break;
                    case 1:
                        AndyAndroidPrefab = second;
                        rotationBTN.SetActive(false);

                        break;
                    case 2:
                        AndyAndroidPrefab = third;
                        rotationBTN.SetActive(false);

                        break;
                    case 3:
                        rotationBTN.SetActive(false);

                        AndyAndroidPrefab = fourth;
                        break;
                    case 4:
                        AndyAndroidPrefab = five;
                        rotationBTN.SetActive(true);

                        break;
                    case 5:
                        AndyAndroidPrefab = six;
                        rotationBTN.SetActive(true);

                        break;
                    case 6:
                        AndyAndroidPrefab = seven;
                        rotationBTN.SetActive(true);

                        break;
                    case 7:
                        AndyAndroidPrefab = eight;
                        rotationBTN.SetActive(true);
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
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            polePos = hit.Pose.position;
                            poleRotate = hit.Pose.rotation;
                            // Andy should look at the camera but still be flush with the plane.
                            andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // Make Andy model a child of the anchor.
                            andyObject.transform.parent = anchor.transform;
                            poleTrans = andyObject.transform;
                        }
                        break;
                    case 1:
                        if (fullshotNUM[1] == 0)
                        {
                            fullshotNUM[1]++;
                            var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                            andyObject.name = "two";
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            polePos = hit.Pose.position;
                            poleRotate = hit.Pose.rotation;
                            // Andy should look at the camera but still be flush with the plane.
                            andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // Make Andy model a child of the anchor.
                            andyObject.transform.parent = anchor.transform;
                            poleTrans = andyObject.transform;
                        }
                        break;
                    case 2:
                        if (fullshotNUM[2] == 0)
                        {
                            fullshotNUM[2]++;
                            var andyObject = Instantiate(AndyAndroidPrefab, hit.Pose.position, hit.Pose.rotation) as GameObject;
                            andyObject.name = "three";
                            // Create an anchor to allow ARCore to track the hitpoint as understanding of the physical
                            // world evolves.
                            var anchor = hit.Trackable.CreateAnchor(hit.Pose);
                            polePos = hit.Pose.position;
                            poleRotate = hit.Pose.rotation;
                            // Andy should look at the camera but still be flush with the plane.
                            andyObject.transform.Rotate(0, k_ModelRotation, 0, Space.Self);
                            // Make Andy model a child of the anchor.
                            andyObject.transform.parent = anchor.transform;
                            poleTrans = andyObject.transform;
                        }
                        break;
                }
                if (poleCount == 1 && elPole == true) {

                }


            }
        }
        private void FixedUpdate()
        {
            if (Input.touchCount == 1 && poleCount == 1)
            {
                GameObject.Find("pole").GetComponent<LeanTranslate>().enabled = true;
                GameObject.Find("pole").GetComponent<LeanRotate>().enabled = false;
            }

            else if (Input.touchCount == 2 && poleCount == 1) {
                GameObject.Find("pole").GetComponent<LeanTranslate>().enabled = false;
                GameObject.Find("pole").GetComponent<LeanRotate>().enabled = true;
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

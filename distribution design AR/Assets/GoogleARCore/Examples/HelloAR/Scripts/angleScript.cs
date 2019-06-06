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
    public class angleScript : MonoBehaviour
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
        //public GameObject addRotation;
        private bool setRotation;
        public GameObject secondLine;
        private bool changeScale = false;
        //public GameObject scaleObject;
        private Transform poleTrans;
        //public GameObject supportSize;
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
        //public GameObject third;
        //public GameObject fourth;
        //public GameObject five;
        //public GameObject six;
        //public GameObject seven;
        //public GameObject eight;

        GameObject[] gears;
        public string BundleURL;
        public string AssetName;

        public GameObject moreBTN;
        public GameObject deleteBTN;
        public GameObject listInOBJ;
        public GameObject switchBTN;


        private int[] fullshotNUM;


        private bool toss = false;

        public Text currentOBJ;
        private int chance;

        private bool isAngle;
        private bool isMoved;
        public GameObject escalate;
        public GameObject moves;

        public Sprite more;
        public Sprite hide;

        public Text angle;

        public Text on;
        public Text off;
        private int touchCount;
        float NewTime;
        float MaxDoubleTime;

        bool arrow;
        public GameObject arrowSwitch;
        GameObject[] arrows;

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


        string[] kinds = {"각도기", "AR자"};
        /// <summary>
        /// possible is true, we can build electric pole. but if not, we can build supported line.
        /// </summary>
        /// <param name="possible">If set to <c>true</c> possible.</param>
        public void buildElPole(bool possible)
        {
            //
            if (possible == true)
            {
                elPole = true;
                
                if (poleCount == 1 && isAngle == true)
                {
                    GameObject.Find("pole").transform.Find("anglePole").gameObject.GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find("pole").GetComponent<BoxCollider>().enabled = true;
                }
                else if (poleCount == 1 && isAngle == false)
                {
                    BeEscalated(isMoved);
                }
            }
            else
            {
                elPole = false;
               
                if (poleCount == 1 && isAngle == true)
                {
                    GameObject.Find("pole").transform.Find("anglePole").gameObject.GetComponent<BoxCollider>().enabled = true;
                    GameObject.Find("pole").GetComponent<BoxCollider>().enabled = false;
                }
                else if (poleCount == 1 && isAngle == false)
                {
                    BeEscalated(isMoved);
                }

            }
        }

        public void isSetRotation(bool yeah)
        {
            if (yeah == true)
            {
                secondLine.SetActive(true);
                bottomCapture.SetActive(true);
                rotation.SetActive(true);
                if (elPole == true)
                {
                    secondLine.SetActive(true);
                    rotation.SetActive(true);
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
                bottomCapture.SetActive(true);
                rotation.SetActive(false);
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
                bottomCapture.SetActive(true);
                //scaleObject.SetActive(true);
            }
            else
            {
                secondLine.SetActive(false);
                //topCapture.SetActive(false);
                bottomCapture.SetActive(true);
                // scaleObject.SetActive(false);
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
                DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 삭제했음");
            }

            if (poleCount == 1) {
                Destroy(GameObject.Find("pole"));
                poleCount = 0;
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
       

        private void autoRotation()
        {
            if (poleCount == 1)
            {
                Vector3 v = FirstPersonCamera.transform.position - GameObject.Find("pole").transform.position;
                v.x = 0;
                v.z = 0;
                GameObject.Find("pole").transform.LookAt(FirstPersonCamera.transform.position - v);
                if (isAngle == false)
                {
                    GameObject.Find("pole").transform.Rotate(0, 90, 0, Space.World);
                }
            }
        }
        /// <summary>
        /// Only Button Action.
        /// </summary>
        public void EscalateRuler()
        {
            //길이 측정 모드이면서 한개가 이미 거설되어 있고, 현재 fix상태라면, 위로올리고, 현재 moved상태라면 아래로 내려감.
            //fingfong을 통해서 약간의 애니메이션을 추가.
            if (poleCount == 1 && isAngle == false)
            {
                if (isMoved == true)
                {
                    isMoved = false;
                }
                else
                {
                    isMoved = true;
                }
                BeEscalated(isMoved);
            }

        }

        public void BeEscalated(bool yes)
        {
            if (yes == true)
            {
                //올라 가는구나..
                //전체적인 컨트롤 -> 전체적으로 움직임만 가능하게.
                if (poleCount == 1 && isAngle == false)
                {
                    //올라가자~!
                    //하강 이미지로 바꾸기
                    //iTween.MoveBy(GameObject.Find("pole"), iTween.Hash("y", 0.3f, "easeType", "easeInOutExpo", "delay", .1));
                    GameObject.Find("pole").GetComponent<BoxCollider>().enabled = true;
                    GameObject.Find("pole").GetComponent<LeanTranslate>().enabled = true;
                    GameObject.FindGameObjectsWithTag("Width")[1].GetComponent<BoxCollider>().enabled = false;
                    GameObject.FindGameObjectsWithTag("Width")[1].GetComponent<upLeanTranslate>().enabled = false;
                    GameObject.FindGameObjectsWithTag("rightFlag")[1].GetComponent<BoxCollider>().enabled = false;
                    GameObject.FindGameObjectsWithTag("rightFlag")[1].GetComponent<flagLeanTranslate>().enabled = false;
                    GameObject.FindGameObjectsWithTag("leftFlag")[1].GetComponent<BoxCollider>().enabled = false;
                    GameObject.FindGameObjectsWithTag("leftFlag")[1].GetComponent<flagLeanTranslate>().enabled = false;
                    GameObject.FindGameObjectsWithTag("parentWidthRod")[1].GetComponent<BoxCollider>().enabled = false;
                    GameObject.FindGameObjectsWithTag("Height")[1].GetComponent<BoxCollider>().enabled = false;
                    GameObject.FindGameObjectsWithTag("Height")[1].GetComponent<HeightLeanTranslate>().enabled = false;
                    GameObject.FindGameObjectsWithTag("heightRod")[1].GetComponent<BoxCollider>().enabled = false;
                    //Coroutine을 넣고 바로 애니메이션 넣자.
                }
            }
            else
            {
                //내려가는 구나
                //상승 이미지로 바꾸기
                //가로, 세로 나눠셔 컨트롤, switch에 따라서 기능을 나눠야함, 
                if (poleCount == 1 && isAngle == false)
                {
                    //iTween.MoveBy(GameObject.Find("pole"), iTween.Hash("y", -0.3f, "easeType", "easeInOutExpo", "delay", .1));
                    GameObject.Find("pole").GetComponent<BoxCollider>().enabled = false;
                    GameObject.Find("pole").GetComponent<LeanTranslate>().enabled = false;
                    if (elPole == true)
                    {
                        //가로활성화
                        GameObject.FindGameObjectsWithTag("Width")[1].GetComponent<BoxCollider>().enabled = true;
                        GameObject.FindGameObjectsWithTag("Width")[1].GetComponent<upLeanTranslate>().enabled = true;
                        GameObject.FindGameObjectsWithTag("rightFlag")[1].GetComponent<BoxCollider>().enabled = true;
                        GameObject.FindGameObjectsWithTag("rightFlag")[1].GetComponent<flagLeanTranslate>().enabled = true;
                        GameObject.FindGameObjectsWithTag("leftFlag")[1].GetComponent<BoxCollider>().enabled = true;
                        GameObject.FindGameObjectsWithTag("leftFlag")[1].GetComponent<flagLeanTranslate>().enabled = true;
                        GameObject.FindGameObjectsWithTag("parentWidthRod")[1].GetComponent<BoxCollider>().enabled = true;
                        GameObject.FindGameObjectsWithTag("Height")[1].GetComponent<BoxCollider>().enabled = false;
                        GameObject.FindGameObjectsWithTag("Height")[1].GetComponent<HeightLeanTranslate>().enabled = false;
                        GameObject.FindGameObjectsWithTag("heightRod")[1].GetComponent<BoxCollider>().enabled = false;
                    }
                    else if (elPole == false)
                    {
                        //세로활성화
                        GameObject.FindGameObjectsWithTag("Width")[1].GetComponent<BoxCollider>().enabled = false;
                        GameObject.FindGameObjectsWithTag("Width")[1].GetComponent<upLeanTranslate>().enabled = false;
                        GameObject.FindGameObjectsWithTag("rightFlag")[1].GetComponent<BoxCollider>().enabled = false;
                        GameObject.FindGameObjectsWithTag("rightFlag")[1].GetComponent<flagLeanTranslate>().enabled = false;
                        GameObject.FindGameObjectsWithTag("leftFlag")[1].GetComponent<BoxCollider>().enabled = false;
                        GameObject.FindGameObjectsWithTag("leftFlag")[1].GetComponent<flagLeanTranslate>().enabled = false;
                        GameObject.FindGameObjectsWithTag("parentWidthRod")[1].GetComponent<BoxCollider>().enabled = false;
                        GameObject.FindGameObjectsWithTag("Height")[1].GetComponent<BoxCollider>().enabled = true;
                        GameObject.FindGameObjectsWithTag("Height")[1].GetComponent<HeightLeanTranslate>().enabled = true;
                        GameObject.FindGameObjectsWithTag("heightRod")[1].GetComponent<BoxCollider>().enabled = true;
                    }
                }
            }
        }

        public void arrowActive() {
            if (arrow == true)
            {
                arrow = false;
                arrowEnable(arrow);
            }
            else {
                arrow = true;
                arrowEnable(arrow);
            }

        }

        private void arrowEnable(bool yes) {
            if (yes == true)
            {
                for (int i = 0; i < arrows.Length; i++) {
                    arrows[i].SetActive(true);
                }
            }
            else {
                for (int i = 0; i < arrows.Length; i++)
                {
                    arrows[i].SetActive(false);
                }
            }
        }

        private void Start()
        {
            StartCoroutine(downLoadAngle());
            elPole = true;
            poleCount = 0;
            supportCount = 0;
            fullshotNUM = new int[3] { 0, 0, 0 };
            chance = 0;
            setRotation = false;
            toss = false;
            isMoved = true;
            rotation.SetActive(false);
            AndyAndroidPrefab = first;
            secondLine.SetActive(false);
            //topCapture.SetActive(false);
            bottomCapture.SetActive(true);
            //supportSize.SetActive(false);
            //scaleObject.SetActive(false);
            rotationBTN.SetActive(false);
            moves.SetActive(false);
            arrow = true;

            angle.text = "";

            touchCount = 0;
            MaxDoubleTime = 0.05f;

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


            //permissionCheck.CheckPermission("android.permission.WRITE_EXTERNAL_STORAGE", 0);
            permissionCheck.RequestPermission("android.permission.WRITE_EXTERNAL_STORAGE", 0);
            //permissionCheck.CheckPermission("android.permission.READ_EXTERNAL_STORAGE", 0);
            permissionCheck.RequestPermission("android.permission.READ_EXTERNAL_STORAGE", 0);

            //permissionCheck.ShowDialog ("android.permission.WRITE_EXTERNAL_STORAGE", 0, "please", "please allow this");
            string data = PlayerPrefs.GetString("userName") + "\n@" + PlayerPrefs.GetString("dept");
            GameObject.Find("Example Controller").SendMessage("getMyInfo", data);
        }
        IEnumerator downLoadAngle() {
            using (WWW www = WWW.LoadFromCacheOrDownload(BundleURL,0))
            {
                //yield return www;
                while (!www.isDone) {
                    Debug.Log(www.progress);
                }

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

            yield return new WaitForSeconds(0.1f);

           
        }

        public void ListenFromMenu(int signal)
        {
            switch (signal)
            {
                case 1:
                    AndyAndroidPrefab = first;
                    type.value = 0;
                    isAngle = true;
                    Debug.Log(signal);
                    break;
                case 2:
                    AndyAndroidPrefab = second;
                    type.value = 1;
                    isAngle = false;
                    Debug.Log(signal);
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
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            //angle.text = "각도: " + GameObject.FindGameObjectsWithTag("anglePole")[1].transform.localEulerAngles.x.ToString("###.#");

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;


            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit) && poleCount == 0 && toss == false)
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
                            isAngle = true;
                            escalate.SetActive(false);
                            arrowSwitch.SetActive(false);
                            on.text = "이동";
                            off.text = "각도";
                            break;
                        case 1:
                            AndyAndroidPrefab = second;
                            isAngle = false;
                            escalate.SetActive(true);
                            arrowSwitch.SetActive(true);
                            on.text = "가로";
                            off.text = "세로";
                            break;
                    }
                    poleCount += 1;
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
                    buildElPole(elPole);
                    if (isAngle == true)
                    {
                       
                        DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                    }
                    if (isAngle == false)
                    {
                        DataTransmission.instance.Loggging(PlayerPrefs.GetString("dept"), PlayerPrefs.GetString("userName"), PlayerPrefs.GetString("lat") + "/" + PlayerPrefs.GetString("long"), kinds[type.value] + "를 생성했음");
                        GameObject.Find("pole").transform.Rotate(0, 90, 0, Space.World);
                        BeEscalated(isMoved);
                        arrows = new GameObject[GameObject.FindGameObjectsWithTag("arrows").Length];
                        for (int i = 0; i < arrows.Length; i++) {
                            arrows[i] = GameObject.FindGameObjectsWithTag("arrows")[i];
                        }
                    }

                }
            }


        }

        private void FixedUpdate()
        {
            
            if (isAngle == true)
            {
                if (poleCount == 1)
                {
                    angle.text = "각도: " + Mathf.Abs(GameObject.Find("pole").transform.Find("anglePole").gameObject.transform.localEulerAngles.z - 90).ToString("#.#");
                }
                else if (poleCount == 0)
                {
                    angle.text = "";
                }
                if (GameObject.Find("pole").transform.Find("anglePole").gameObject.transform.localEulerAngles.z > 180 && GameObject.Find("pole").transform.Find("anglePole").gameObject.transform.localEulerAngles.z < 270)
                {
                    GameObject.Find("pole").transform.Find("anglePole").gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                    angle.text = "각도: " + "90";

                }
                else if (GameObject.Find("pole").transform.Find("anglePole").gameObject.transform.localEulerAngles.z > 270)
                {
                    GameObject.Find("pole").transform.Find("anglePole").gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    angle.text = "각도: " + "90";
                }
            }

            if (poleCount == 1 && isAngle == false)
            {
                GameObject.FindGameObjectsWithTag("widthText")[1].GetComponent<TextMesh>().text = "가로: " + Vector3.Distance(GameObject.FindGameObjectsWithTag("leftFlag")[1].transform.position, GameObject.FindGameObjectsWithTag("rightFlag")[1].transform.position).ToString("#.#") + "m";
                GameObject.FindGameObjectsWithTag("heightText")[1].GetComponent<TextMesh>().text = "높이: " + (GameObject.FindGameObjectsWithTag("upperPivot")[1].transform.position.y - GameObject.FindGameObjectsWithTag("bottomPivot")[1].transform.position.y).ToString("#.#") + "m";
                //validation..
                //leftFlag
                if (GameObject.FindGameObjectsWithTag("leftFlag")[1].transform.localPosition.z <= GameObject.FindGameObjectsWithTag("leftEndPoint")[1].transform.localPosition.z)
                {
                    GameObject.FindGameObjectsWithTag("leftFlag")[1].transform.localPosition = new Vector3(0, 0, GameObject.FindGameObjectsWithTag("leftEndPoint")[1].transform.localPosition.z + 0.03f);
                }
                else if (GameObject.FindGameObjectsWithTag("leftFlag")[1].transform.localPosition.z >= GameObject.FindGameObjectsWithTag("upperPivot")[1].transform.localPosition.z)
                {
                    GameObject.FindGameObjectsWithTag("leftFlag")[1].transform.localPosition = new Vector3(0, 0, GameObject.FindGameObjectsWithTag("upperPivot")[1].transform.localPosition.z - 0.03f);
                }
                //rightFlag
                if (GameObject.FindGameObjectsWithTag("rightFlag")[1].transform.localPosition.z <= GameObject.FindGameObjectsWithTag("upperPivot")[1].transform.localPosition.z)
                {
                    GameObject.FindGameObjectsWithTag("rightFlag")[1].transform.localPosition = new Vector3(0, 0, GameObject.FindGameObjectsWithTag("upperPivot")[1].transform.localPosition.z + 0.03f);
                }
                else if (GameObject.FindGameObjectsWithTag("rightFlag")[1].transform.localPosition.z >= GameObject.FindGameObjectsWithTag("rightEndPoint")[1].transform.localPosition.z)
                {
                    GameObject.FindGameObjectsWithTag("rightFlag")[1].transform.localPosition = new Vector3(0, 0, GameObject.FindGameObjectsWithTag("rightEndPoint")[1].transform.localPosition.z - 0.03f);
                }
                //WidthRod
                if (GameObject.FindGameObjectsWithTag("Width")[1].transform.localPosition.y >= GameObject.FindGameObjectsWithTag("upperEndPoint")[1].transform.localPosition.y)
                {
                    GameObject.FindGameObjectsWithTag("Width")[1].transform.localPosition = new Vector3(0, GameObject.FindGameObjectsWithTag("upperEndPoint")[1].transform.localPosition.y - 0.03f, 0);
                }
                else if (GameObject.FindGameObjectsWithTag("Width")[1].transform.localPosition.y <= 0)
                {
                    GameObject.FindGameObjectsWithTag("Width")[1].transform.localPosition = new Vector3(0, 0.03f, 0);
                }
                //heightRod
                if (GameObject.FindGameObjectsWithTag("Height")[1].transform.localPosition.z <= GameObject.FindGameObjectsWithTag("leftEndPoint")[1].transform.localPosition.z)
                {
                    GameObject.FindGameObjectsWithTag("Height")[1].transform.localPosition = new Vector3(0, 0, GameObject.FindGameObjectsWithTag("leftEndPoint")[1].transform.localPosition.z + 0.03f);
                }
                else if (GameObject.FindGameObjectsWithTag("Height")[1].transform.localPosition.z >= GameObject.FindGameObjectsWithTag("rightEndPoint")[1].transform.localPosition.z)
                {
                    GameObject.FindGameObjectsWithTag("Height")[1].transform.localPosition = new Vector3(0, 0, GameObject.FindGameObjectsWithTag("rightEndPoint")[1].transform.localPosition.z - 0.03f);
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
                SceneManager.LoadScene("menu");
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
            if (Input.touchCount == 1)
            {
                Touch touch2 = Input.GetTouch(0);

                if (touch2.phase == TouchPhase.Ended)
                {
                    touchCount += 1;

                }


                if (touchCount == 1)
                {
                    NewTime = Time.time + MaxDoubleTime;
                }
                else if (touchCount == 2 && Time.time < NewTime)
                {
                    //double Tab section..

                    autoRotation();
                    touchCount = 0;
                }
            }
            if (Time.time > NewTime)
            {
                touchCount = 0;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System;

public class ARTrackedMultiImageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trackedPrefabs; // �̹����� �ν����� �� ��µǴ� ������ ���

    [SerializeField]
    //private GameObject nextScene;

    public Text imageTrackedText; // �νĵ� ��ü ǥ��
    public Text showFingerPosition; // ���� ��ġ(�����)
    public Text showThumbPosition; // ���� ��ġ(�����)
    public Text showPrefabPosition; // ��ü ����(�����)
    public Text touchText; // ��ġ ����(�����)
    public Text PickUpText; // �Ⱦ� ����(�����)
    //public Text distDebug; // �Ÿ� ���(�����)
    public Text TrackingText; // Ʈ��ŷ ����(�����)

    private Vector3 thumbPosition;
    private Vector3 indexPosition;
    private Vector3 prefabPosition;

    // �̹����� �ν����� �� ��µǴ� ������Ʈ ���
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    private void Awake()
    {
        // AR Session Origin ������Ʈ�� ������Ʈ�� �������� �� ��� ����
        trackedImageManager = GetComponent<ARTrackedImageManager>();

        // trackedPrefabs �迭�� �ִ� ��� �������� Instantiate()�� ������ ��
        // spawnedObjects Dictionary�� ����, ��Ȱ��ȭ
        // ī�޶� �̹����� �νĵǸ� �̹����� ������ �̸��� key�� �ִ� value ������Ʈ�� ���
        foreach(GameObject prefab in trackedPrefabs)
        {
            GameObject clone = Instantiate(prefab); // ������Ʈ ����
            clone.name = prefab.name;               // ������ ���������� �̸� ����
            clone.SetActive(false);                 // ������Ʈ ��Ȱ��ȭ
            spawnedObjects.Add(clone.name, clone);  // Dictionary �ڷᱸ���� ������Ʈ ����
        }
    }

    private void OnEnable()
    {
        trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // ī�޶� �̹����� �νĵǾ��� ��
        foreach(var trackedImage in eventArgs.added)
        {
            UpdateImage(trackedImage);
        }

        // ī�޶� �̹����� �νĵǾ� ������Ʈ�ǰ� ���� ��
        foreach(var trackedImage in eventArgs.updated)
        {
            UpdateImage(trackedImage);
        }

        // �νĵǰ� �ִ� �̹����� ī�޶󿡼� ������� ��
        foreach(var trackedImage in eventArgs.removed)
        {
            spawnedObjects[trackedImage.name].SetActive(false);
        }
    }

    private void UpdateImage(ARTrackedImage trackedImage)
    {
        string name = trackedImage.referenceImage.name;
        imageTrackedText.text = name;
        GameObject trackedObject = spawnedObjects[name];

        // �̹����� ���� ���°� ������(Tracking)�� ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            TrackingText.text = "Tracking";
            trackedObject.transform.position = trackedImage.transform.position;
            //trackedObject.transform.rotation = trackedImage.transform.rotation;

            if (trackedObject.tag == "item")
                trackedObject.SetActive(true);

            // ������ ������ ��ġ
            thumbPosition = new Vector3(ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[4].x,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[4].y,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation);
            indexPosition = new Vector3(ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].x,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].y,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation);

            // spawn�� prefab ��ġ : World �� Viewport
            prefabPosition = Camera.main.WorldToViewportPoint(trackedObject.transform.position);

            // ����׿�
            showPrefabPosition.text = "( " + prefabPosition.x.ToString("N2") + ", " + prefabPosition.y.ToString("N2") + " )";

            showFingerPosition.text = "( " + indexPosition.x.ToString("N2") + ", " + indexPosition.y.ToString("N2") + " )";

            showThumbPosition.text = "( " + thumbPosition.x.ToString("N2") + ", " + thumbPosition.y.ToString("N2") + " )";

            if (IsTouch() == true)
            {
                touchText.text = "Touch!";
                if (trackedObject.tag == "hint")
                {
                    //nextScene.SetActive(true);
                }

                else
                {
                    if (IsPickUp() == true)
                    {
                        PickUpText.text = "PickUp!";
                        if (trackedObject.tag == "item")
                        {
                            //nextScene.SetActive(true);
                            Destroy(trackedObject);
                            // �κ��丮�� ������ �ִ� ��� �߰�
                        }
                    }
                    else
                        PickUpText.text = "Non-Pickup";
                }
            }
            else
            {
                touchText.text = "Untouch";
                PickUpText.text = "X";
            }
        }

        else if (trackedImage.trackingState == TrackingState.Limited)
        {
            TrackingText.text = "Limited";
            trackedObject.SetActive(false);
            thumbPosition = new Vector3(-1,-1,0);
            indexPosition = new Vector3(1,1,0);
        }

        else if (trackedImage.trackingState == TrackingState.None)
            TrackingText.text = "None";

        /*
        else
        {
            trackedObject.SetActive(false);
            thumbPosition = Vector3.zero;
            indexPosition = Vector3.zero;
        }*/
    }

    private bool IsTouch()
    {
        float indexx = indexPosition.x;
        float indexy = indexPosition.y;
        float prefabx = prefabPosition.x;
        float prefaby = prefabPosition.y;

        double dist = Math.Sqrt(Math.Pow(indexx - prefabx, 2) + Math.Pow(indexy - prefaby, 2));

        if (dist < 0.15) return true;
        else return false;
    }

    private bool IsPickUp()
    {
        float thumbx = thumbPosition.x;
        float thumby = thumbPosition.y;
        float indexx = indexPosition.x;
        float indexy = indexPosition.y;

        double dist = Math.Sqrt(Math.Pow(indexx - thumbx, 2) + Math.Pow(indexy - thumby, 2));

        if (dist < 0.07)
            return true;
        else
            return false;
    }
}

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

    [SerializeField] AudioSource itemPkdSnd; // �Ⱦ� ����

    [SerializeField]
    private GameObject pickupPictogram;  // �Ⱦ� ����׷�

    public Text imageTrackedText; // �νĵ� ��ü ǥ��

    private bool hasKey = false;
    private bool hasVaccine = false;

    private Vector3 thumbPosition;  // ���� ��ġ
    private Vector3 indexPosition;  // ���� ��ġ
    private Vector3 prefabPosition; // ��ü ��ġ

    // �̹����� �ν����� �� ��µǴ� ������Ʈ ���
    private Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
    private ARTrackedImageManager trackedImageManager;

    [SerializeField]
    private PuzzleEffect puzzleEffect;

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
        GameObject trackedObject = spawnedObjects[name];

        // �̹����� ���� ���°� ������(Tracking)�� ��
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            imageTrackedText.text = name;
            trackedObject.transform.position = trackedImage.transform.position;
            
            // locker - key�� pictogram
            if (trackedObject.tag == "key" && !hasKey && Dial.instance.clearInst)
            {
                trackedObject.SetActive(true);
                pickupPictogram.SetActive(true);
            }
            else if (trackedObject.tag == "key" && !hasKey)
            {
                trackedObject.SetActive(true);
            }

            // charger - vaccine��
            //if (trackedObject.tag == "vaccine" && !hasVaccine && Dial.instance.clearChargerInst)
            if (trackedObject.tag == "vaccine" && !hasVaccine && InventoryManager.instance.equip_cardkey)
            {
                trackedObject.SetActive(true);
            }

            // ������ ������ ��ġ
            thumbPosition = new Vector3(ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[4].x,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[4].y,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation);
            indexPosition = new Vector3(ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].x,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].y,
                        ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.depth_estimation);

            // spawn�� prefab ��ġ : World �� Viewport
            prefabPosition = Camera.main.WorldToViewportPoint(trackedObject.transform.position);

            // key, vaccine ȹ�� �� �̺�Ʈ ó��
            if (HandTracking.isHandOn && IsTouch() && IsPickUp())
            {
                itemPkdSnd.Play();
                trackedObject.SetActive(false);

                switch (trackedObject.tag)
                {
                    case "key":
                        if (Dial.instance.clearInst)
                        {
                            pickupPictogram.SetActive(false);
                            hasKey = true;
                            showText.instance.ShowText("���踦 ȹ���Ͽ����ϴ�");
                            InventoryManager.instance.canUseItem[1] = true;
                        }
                        break;
                    case "vaccine":
                        if (Dial.instance.clearChargerInst)
                        {
                            hasVaccine = true;
                            showText.instance.ShowText("����� ȹ���Ͽ����ϴ�");
                            InventoryManager.instance.canUseItem[3] = true;
                        }
                        break;
                }
            }       
        }

        else if (trackedImage.trackingState == TrackingState.Limited)
        {
            trackedObject.SetActive(false);
            thumbPosition = new Vector3(0,0,0);
            indexPosition = new Vector3(1,1,0);
        }
    }

    private bool IsTouch() // ������ ��ü�� ������ ������ Touch
    {
        float indexx = indexPosition.x;
        float indexy = indexPosition.y;
        float prefabx = prefabPosition.x;
        float prefaby = prefabPosition.y;

        double dist = Math.Sqrt(Math.Pow(indexx - prefabx, 2) + Math.Pow(indexy - prefaby, 2));

        if (dist < 0.15) 
            return true;
        else 
            return false;
    }

    private bool IsPickUp() // ������ ������ ��ġ�� ������ PickUp
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

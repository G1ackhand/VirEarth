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

    [SerializeField] AudioSource itemPkdSnd;

    [SerializeField]
    private GameObject pickupPictogram;

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

    //public Vector2 trackedImageSize; // �̹��� ������ �� ����
    //public Vector3 trackedImagePosition; //������ �� ����

    private bool hasKey = false;

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

            TrackingText.text = "Tracking";
            trackedObject.transform.position = trackedImage.transform.position;
            //trackedImagePosition = trackedImage.transform.position; // ������ �� ����
            
            /*if (trackedImagePosition.y != 0)
                trackedImagePosition.y = 0;

            trackedImageSize = trackedImage.size;*/

            if (trackedObject.tag == "item" && !hasKey && Dial.instance.clearInst)
            {
                //puzzleEffect.puzzleEffect(true);
                trackedObject.SetActive(true);
                pickupPictogram.SetActive(true);
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

            // ����׿�
            showPrefabPosition.text = "( " + prefabPosition.x.ToString("N2") + ", " + prefabPosition.y.ToString("N2") + " )";
            showFingerPosition.text = "( " + indexPosition.x.ToString("N2") + ", " + indexPosition.y.ToString("N2") + " )";
            showThumbPosition.text = "( " + thumbPosition.x.ToString("N2") + ", " + thumbPosition.y.ToString("N2") + " )";

            if (HandTracking.isHandOn && IsTouch())
            {
                touchText.text = "Touch!";

                if (IsPickUp() == true)
                {
                    PickUpText.text = "PickUp!";
                    if (trackedObject.tag == "item")
                    {
                        //itemPkdSnd.Play();
                        trackedObject.SetActive(false);
                        pickupPictogram.SetActive(false);
                        hasKey = true;
                        showText.instance.ShowText("���踦 ȹ���Ͽ����ϴ�");
                        InventoryManager.instance.canUseItem[1] = true;
                    }
                }
                else
                    PickUpText.text = "Non-Pickup";
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
            thumbPosition = new Vector3(0,0,0);
            indexPosition = new Vector3(1,1,0);
        }

        else
            TrackingText.text = "None";
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

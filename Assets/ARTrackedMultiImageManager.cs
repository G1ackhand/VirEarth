using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedMultiImageManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] trackedPrefabs; // �̹����� �ν����� �� ��µǴ� ������ ���
    public Text imageTrackedText;
    public Text showPrefabPosition;
    public Text showFingerPosition;

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
        if(trackedImage.trackingState == TrackingState.Tracking)
        {
            //trackedObject.transform.position = trackedImage.transform.position;
            trackedObject.transform.position = new Vector3(0.5f, 0.5f, 0.5f);
            //trackedObject.transform.rotation = trackedImage.transform.rotation;
            showPrefabPosition.text = "( " + trackedObject.transform.position.x + ", " + trackedObject.transform.position.y + ", "
                + trackedObject.transform.position.z + " )";
            showFingerPosition.text = "( " + ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].x + ", " 
                + ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].y + ", "
                + ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[8].z + " )";
            trackedObject.SetActive(true);
        }
        else
        {
            trackedObject.SetActive(false);
        }
    }
}

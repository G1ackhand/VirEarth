using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

/// <summary>
/// Listens for touch events and performs an AR raycast from the screen touch point.
/// AR raycasts will only hit detected trackables like feature points and planes.
///
/// If a raycast hits a trackable, the <see cref="placedPrefab"/> is instantiated
/// and moved to the hit position.
/// </summary>
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceOnPlane : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Instantiates this prefab on a plane at the touch location.")]
    GameObject m_PlacedPrefab;

    [SerializeField] GameObject Flash_Effect;
    [SerializeField] AudioSource genSnd;


    /// <summary>
    /// The prefab to instantiate on touch.
    /// </summary>
    public GameObject placedPrefab
    {
        get { return m_PlacedPrefab; }
        set { m_PlacedPrefab = value; }
    }

    /// <summary>
    /// The object instantiated as a result of a successful raycast intersection with a plane.
    /// </summary>
    public GameObject spawnedObject { get; private set; }

    void Awake()
    {
        m_RaycastManager = GetComponent<ARRaycastManager>();
    }

    bool TrySwipeAutoPosition(out Vector2 position)
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger == ManoGestureTrigger.SWIPE_RIGHT)
        {
            position = new Vector2(0.5f * Screen.width, 0.7f * Screen.height);
            return true;
        }
        position = default;
        return false;
    }


    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            var mousePosition = Input.mousePosition;
            touchPosition = new Vector2(mousePosition.x, mousePosition.y);
            return true;
        }
#else
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
#endif

        touchPosition = default;
        return false;
    }

    void Update()
    {
        /*if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;*/
        /*if (!TrySwipeAutoPosition(out Vector2 touchPosition))
            return;*/
        if (!HandTracking.instance.swipe)
            return;
        Vector2 touchPosition = new Vector2(0.5f * Screen.width, 0.5f * Screen.height);
        if (m_RaycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            // Raycast hits are sorted by distance, so the first one
            // will be the closest hit.
            var hitPose = s_Hits[0].pose;

            if ((spawnedObject == null))
            {
                Flash_Effect.SetActive(true);
                genSnd.Play(); //소환 사운드

                Bgm_manager.intnstance.pasueBGM();
                spawnedObject = Instantiate(m_PlacedPrefab, hitPose.position, hitPose.rotation);
                InventoryManager.instance.equip_cardkey = false;
            }
            /*else
            {
                spawnedObject.transform.position = hitPose.position;
            }*/
        }
    }

    static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    ARRaycastManager m_RaycastManager;
}

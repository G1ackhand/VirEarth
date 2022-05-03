using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// akejkfjwek;
public class HandTracking : MonoBehaviour
{
    public static HandTracking instance; // �ν��Ͻ�

    // �ν�����
    //[SerializeField] private Image HandImage; // ȭ�� 11�� �� �̹���
    //[SerializeField] private Text test;

    // public ����
    public static bool isHandOn = false;
    public bool swipe;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        //HandImage.enabled = false;
        isHandOn = false;
    }


    private void Update()
    {
        //HandImage.enabled = false;
        isHandOn = false;
        //if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.is_right == -1) //ī�޶� ���� ������ ���̻� �������� ����.
        //    return;
        // -------------------------------------------------------------------------------
        //HandImage.enabled = true;
        isHandOn = true;
        //InventoryManager.instance.InventoryManagement(); // �κ��丮, inventoryManagement_enable�� false�� �۵�����
        if (InventoryManager.instance.equip_cardkey)
        {
            if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.mano_gesture_trigger == ManoGestureTrigger.SWIPE_RIGHT)
                if (!swipe)
                    StartCoroutine(swiping());
        }
    }
    private IEnumerator swiping()
    {
        swipe = true;
        yield return new WaitForSeconds(3.0f);
        swipe = false;
    }
    public bool IsFoldFinger(bool thumb, bool point, bool big, bool four, bool little) // ����, ����, ����, ����, ����
    {
        //int count = 0;
        TrackingInfo hand = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;
        //hand.skeleton.joints[0].x
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.is_right == 1) // �������̸�
        {
            if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side == HandSide.Backside) // �յ��̸�
            {
                if ((hand.skeleton.joints[4].x > hand.skeleton.joints[2].x) == (thumb)) { }
                else
                    return false;
            }
            else if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side == HandSide.Palmside)    // �չٴ��̸�
            {
                if ((hand.skeleton.joints[4].x < hand.skeleton.joints[2].x) == (thumb)) { }
                else
                    return false;
            }
        }
        else if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.is_right == 0) // �޼��̸�
        {
            if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side == HandSide.Backside) // �յ��̸�
            {
                if ((hand.skeleton.joints[4].x < hand.skeleton.joints[2].x) == (thumb)) { }
                else
                    return false;
            }
            else if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side == HandSide.Palmside)    // �չٴ��̸�
            {
                if ((hand.skeleton.joints[4].x > hand.skeleton.joints[2].x) == (thumb)) { }
                else
                    return false;
            }
        }

        if ((hand.skeleton.joints[8].y < hand.skeleton.joints[6].y) == (point)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[12].y < hand.skeleton.joints[10].y) == (big)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[16].y < hand.skeleton.joints[14].y) == (four)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[20].y < hand.skeleton.joints[18].y) == (little)) { } // ����
        else
            return false;

        return true;
    }

    public bool getFingers(bool point, bool big, bool four, bool little)
    {
        TrackingInfo hand = ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info;

        if ((hand.skeleton.joints[8].y < hand.skeleton.joints[6].y) == (point)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[12].y < hand.skeleton.joints[10].y) == (big)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[16].y < hand.skeleton.joints[14].y) == (four)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[20].y < hand.skeleton.joints[18].y) == (little)) { } // ����
        else
            return false;

        return true;
    }

    public float GetX(int index)
    {
        return ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[index].x;
    }

    public float GetY(int index)
    {
        return ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[index].y;
    }

}

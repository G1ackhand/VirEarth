using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandTracking : MonoBehaviour
{
    // �ν�����
    [SerializeField] private Image HandImage; // ȭ�� 11�� �� �̹���
    [SerializeField] private Text test;

    // public ����
    public static bool isHandOn = false;
    public static float[,] hand = new float[21, 2];   // landmark 21���� x, y �迭
    public static float zvalue;       // landmark 0���� z value


    // private ����
    private GestureInfo gesture;        // ����ó
    private bool inventoryOnFlag;

    // Start is called before the first frame update
    void Start()
    {
        HandImage.enabled = false;
        isHandOn = false;
        inventoryOnFlag = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandImage.enabled = false;
        isHandOn = false;
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.is_right == -1) //ī�޶� ���� ������ ���̻� �������� ����.
            return;
        // -------------------------------------------------------------------------------
        HandImage.enabled = true;
        isHandOn = true;

        test.text = "It is Debug Text\n";
        InventoryOn();
        /*gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
        test.text = "Hand_side : " + gesture.hand_side.ToString() + " || is_right = " + gesture.is_right.ToString() + "\n";
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.is_right == 1) // �������̸�
            test.text += "Right Hand\n";
        else if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.is_right == 0) // �޼��̸�
            test.text += "Left Hand\n";
        else        // ���� ������
            test.text += "Right ? Left\n";

        if (((int)gesture.mano_class) == 0)
            test.text += "\n";
        else if (((int)gesture.mano_class) == 1)
            test.text += "\n";
        else if (((int)gesture.mano_class) == 2)
            test.text += "Pointer\n";
        else
            test.text += "Gestrue?";*/
    }

    private void InventoryOn()
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side != HandSide.Palmside)
            return;
        if (IsFoldFinger(false, false, false, false, false))
        {
            test.text += "Inventory ON\n";
        }
        else
            test.text += "Inventory OFF\n";
        return;
    }

    private bool IsFoldFinger(bool thumb, bool point, bool big, bool four, bool little) // ����, ����, ����, ����, ����
    {
        int count = 0;
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

        if ((hand.skeleton.joints[12].y < hand.skeleton.joints[10].y) == (point)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[16].y < hand.skeleton.joints[14].y) == (point)) { } // ����
        else
            return false;

        if ((hand.skeleton.joints[20].y < hand.skeleton.joints[18].y) == (point)) { } // ����
        else
            return false;

        return true;
    }

}

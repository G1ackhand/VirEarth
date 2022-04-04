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
        if (inventoryOnFlag)
            test.text += "flag : True\n";
        else
            test.text += "flag : False\n";
        InventoryOn();  // �չٴ� ���¿��� �հ����� ���� �Ǹ� ��, �ָ��� ��� ����
        if (inventoryOnFlag)
        {
            DisplayInventory();
            SelectItem();
        }
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
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side != HandSide.Palmside) // �չٴ��� �ƴϸ� ��� ����
            return;
        if (IsFoldFinger(false, false, false, false, false) && !inventoryOnFlag)
        {
            test.text += "Inventory ON\n";
            inventoryOnFlag = true;
            
        }
        else if(IsFoldFinger(true, true, true, true, true) && inventoryOnFlag)
        {
            test.text += "Inventory OFF\n";
            inventoryOnFlag = false;
        }
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

    private void DisplayInventory()
    {
        // ItemSystem.get0;
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side != HandSide.Palmside) // �չٴ��� �ƴϸ� ��� ����
            return;
        /*if (ItemSystem.instance.get0)
            CoordinateSystem.instance.transCoord(0, GetX(8), GetY(8));
        if (ItemSystem.instance.get1)
            CoordinateSystem.instance.transCoord(1, GetX(12), GetY(12));
        if (ItemSystem.instance.get2)
            CoordinateSystem.instance.transCoord(2, GetX(16), GetY(16));
        if (ItemSystem.instance.get3)
            CoordinateSystem.instance.transCoord(3, GetX(20), GetY(20));*/

        CoordinateSystem.instance.transCoord(0, GetX(8), GetY(8));
        CoordinateSystem.instance.transCoord(1, GetX(12), GetY(12));
        CoordinateSystem.instance.transCoord(2, GetX(16), GetY(16));
        CoordinateSystem.instance.transCoord(3, GetX(20), GetY(20));

    }

    private float GetX(int index)
    {
        return ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[index].x;
    }

    private float GetY(int index)
    {
        return ManomotionManager.Instance.Hand_infos[0].hand_info.tracking_info.skeleton.joints[index].y;
    }

    private void SelectItem()
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side != HandSide.Palmside) // �չٴ��� �ƴϸ� ��� ����
        {
            test.text += "SelectItem() no palm\n";
        }

        //return;
        if (IsFoldFinger(true, false, true, true, true)) // 0�� �ε���
        {
            test.text += "item0 Select\n";
        }
        else if (IsFoldFinger(true, false, false, true, true)) // 1�� �ε���
        {
            test.text += "item1 Select\n";
        }
        else if (IsFoldFinger(true, false, false, false, true)) // 2�� �ε���
        {
            test.text += "item2 Select\n";
        }
        else if (IsFoldFinger(true, false, false, false, false)) // 3�� �ε���
        {
            test.text += "item3 Select\n";
        }
        else
            test.text += "no search item\n";
    }
}

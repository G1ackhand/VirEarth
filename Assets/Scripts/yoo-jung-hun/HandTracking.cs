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


    // private
    private GestureInfo gesture;        // ����ó

    // Start is called before the first frame update
    void Start()
    {
        HandImage.enabled = false;
        isHandOn = false;
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

        gesture = ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info;
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
            test.text += "Gestrue?";
    }



}

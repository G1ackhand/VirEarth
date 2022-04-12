using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance; // �ν��Ͻ�

    // �ν�����
    [SerializeField] private Text debug;
    [SerializeField] private Text debug_inventory;
    [SerializeField] private Image[] backImage = new Image[5];

    // public ����
    

    // FLAG
    private bool flag_inventoryOn;       // �κ��丮 �� ���� flag
    public bool inventoryManagement_enable;

    // private ����
    private GestureInfo gesture;        // ����ó
    private int selectItem;             // ������ ������ ��ȣ (0~3, 4��, 4�� null)
    private List<int> selectItemList = new List<int>();
    private int[] selectItemArray = new int[3];
    private int final_selectItem = 4;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        flag_inventoryOn = false;
        inventoryManagement_enable = true;
        selectItem = 4;
        for (int i = 0; i < 5; i++)
        {
            var tempColor = backImage[i].color;
            tempColor.a = 0f;
            backImage[i].color = tempColor;
        }
    }


    public void InventoryManagement()
    {
        if (inventoryManagement_enable == false)
            return;
        if (!HandTracking.isHandOn)
            return;
        debug.text = "It is Debug Text\n";
        if (flag_inventoryOn)
            debug.text += "flag : True\n";
        else
            debug.text += "flag : False\n";
        InventoryOn();  // �չٴ� ���¿��� �հ����� ���� �Ǹ� ��, �ָ��� ��� ����
        if (flag_inventoryOn)
        {
            Color tempColor;
            for (int i = 0; i < 5; i++)
            {
                tempColor = backImage[i].color;
                tempColor.a = 0f;
                backImage[i].color = tempColor;
            }
            CoordinateSystem.instance.transCoord(HandTracking.instance.GetX(0), HandTracking.instance.GetY(0)); // display inventory
            SelectItem();

            debug.text = debug.text + "item select : " + final_selectItem.ToString() + "\n";
            tempColor = backImage[selectItemList[2]].color;
            tempColor.a = 1f;
            backImage[final_selectItem].color = tempColor;
        }
        return;
    }

    private void InventoryOn()
    {
        if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side != HandSide.Palmside) // �չٴ��� �ƴϸ� ��� ����
            return;
        if (HandTracking.instance.IsFoldFinger(false, false, false, false, false) && !flag_inventoryOn)
        {
            debug.text += "Inventory ON\n";
            flag_inventoryOn = true;
            CoordinateSystem.instance.showImg();
            
        }
        else if (HandTracking.instance.IsFoldFinger(true, true, true, true, true) && flag_inventoryOn)
        {
            if (final_selectItem != 4)
            {
                debug_inventory.text = final_selectItem.ToString();
                // ������ ȿ�� ���⼭!
                // ������ db ���� ���⼭!
            }

            /*if ((selectItemList[0] != -1) && (selectItemList[0] == selectItemList[1]) && (selectItemList[1] == selectItemList[2]))
            {
                final_selectItem = selectItemList[2];
                // ������ ȿ�� ���⼭!
                // ������ db ���� ���⼭!
                debug_inventory.text = final_selectItem.ToString();
            }*/
            debug.text += "Inventory OFF\n";
            flag_inventoryOn = false;
            CoordinateSystem.instance.hideImg();
            
        }
    }


    private void SelectItem()
    {
        /*if (ManomotionManager.Instance.Hand_infos[0].hand_info.gesture_info.hand_side != HandSide.Palmside) // �չٴ��� �ƴϸ� ��� ����
        {
            debug.text += "SelectItem() no palm\n";
            return;
        }*/

        if (HandTracking.instance.IsFoldFinger(false, false, false, false, false)) // 4�� �ε���
        {
            debug.text += "no search item\n";
            selectItem = 4;
        }
        else if (HandTracking.instance.IsFoldFinger(true, false, true, true, true)) // 0�� �ε���
        {
            debug.text += "item0 Select\n";
            selectItem = 0;
        }
        else if (HandTracking.instance.IsFoldFinger(true, false, false, true, true)) // 1�� �ε���
        {
            debug.text += "item1 Select\n";
            selectItem = 1;
        }
        else if (HandTracking.instance.IsFoldFinger(true, false, false, false, true)) // 2�� �ε���
        {
            debug.text += "item2 Select\n";
            selectItem = 2;
        }
        else if (HandTracking.instance.IsFoldFinger(true, false, false, false, false)) // 3�� �ε���
        {
            debug.text += "item3 Select\n";
            selectItem = 3;
        }
        selectItemList.Add(selectItem);
        if (selectItemList.Count == 11)
            selectItemList.RemoveAt(0);

        final_selectItem = check_selectItemList(selectItemList);
    }

    private int check_selectItemList(List<int> ls)
    {
        int res = 0;
        int[] tmp = new int[5] { 0, 0, 0, 0, 0 };
        foreach(var a in ls)
        {
            tmp[a]++;
        }
        for (int i = 0; i < 4; i++)
        {
            for (int j = i+1; j < 5; j++)
            {
                if (tmp[i] < tmp[j])
                    res = j;
            }
        }
        return res;
    } 

    public void set_inventoryManagement_enable(bool flag)
    {
        inventoryManagement_enable = flag;
    }
}

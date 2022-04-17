using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleEffect : MonoBehaviour
{
    [SerializeField] AudioSource failSnd, clearSnd;
    [SerializeField] Text strtTxt;

    public static PuzzleEffect instance;
    void Start()
    {
        
    }
    private void Awake()
    {
        instance = this;
    }
    // Update is called once per frame
    public void puzzleEffect(bool isCleared)
    {
        if (isCleared)
        {
            clearSnd.Play();
            //������ �˾�.
        }
        else
        {
            failSnd.Play();
            StartCoroutine("Show");
        }
    }
    IEnumerator Show()
    {
        strtTxt.text = "[ERROR] : �� �Է��Ͻʽÿ�.";
        for (int i = 0; i < 10; i++)
        {
            strtTxt.color = new Vector4(1, 0, 0, 1);
            yield return new WaitForSeconds(0.02f);
        }
        strtTxt.text = " ";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    [Tooltip("Tail Node���(��λΪframe)")]
    public int TailNodeInterval = 3;

    [Tooltip("First Tail Node�������ʼλ�õ�ƫ��")]
    public int FirstTailNodeOffset = 2;

    // Start is called before the first frame update
    private void Awake()
    {
        Application.targetFrameRate = 30; //������ʮ֡
        TailNodeBehavior.FirstSearchPosOffset = FirstTailNodeOffset;
        TailNodeBehavior.SearchInterval = TailNodeInterval;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

/*********
 * TailNodeBehavior.cs : ʵ��TailNode����Ϊ������λ�ø��£���ײ����
 * 
 ********/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailNodeBehavior : MonoBehaviour
{
    //TODO(Hangyu) : �Ƿ���ҪEditorӳ�䣿
    public static int SearchInterval = 5;


    private GameObject mLeader;
    private int mCurrentNodeIdx;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //����tail nodeλ��
        int searchPosOnTrack = (mCurrentNodeIdx + 1) * SearchInterval;  //eg: (0 + 1) * 5 ��ʾnode0��SearchPos��Track��һֱΪ5

        List<Vector3> track = mLeader.GetComponent<TailController>().GetTrack();
        transform.position = track[searchPosOnTrack];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO(Hangyu) : Ŀǰֻ������TailNode֮�����ײ��������Ҫ���������������ײ

        if (collision.gameObject.tag == "Tail")
        {
            if (!mLeader) return;

            List<int> triggerFlags = mLeader.GetComponent<TailController>().GetTriggerFlags();
            int collidedNodeIdx = collision.gameObject.GetComponent<TailNodeBehavior>().mCurrentNodeIdx;
            if(Math.Abs(collidedNodeIdx - mCurrentNodeIdx) > 1)
                triggerFlags[mCurrentNodeIdx] = Math.Min(collidedNodeIdx, mCurrentNodeIdx);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Tail")
        {
            if (!mLeader) return;

            List<int> triggerFlags = mLeader.GetComponent<TailController>().GetTriggerFlags();
            triggerFlags[mCurrentNodeIdx] = 0;
        }
    }


    public void SetCurrentNodeIdx(int InSearchPos)
    {
        mCurrentNodeIdx = InSearchPos;
    }

    public int GetCurrentNodeIdx()
    {
        return mCurrentNodeIdx;
    }

    public void SetLeader(GameObject leader)
    {
        mLeader = leader;
    }

    public GameObject GetLeader()
    {
        return mLeader;
    }

}
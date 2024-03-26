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
    public static int SearchInterval;
    public static int FirstSearchPosOffset;
    [Tooltip("����")]
    public Material[] mat;
    [Tooltip("��ǰ���ʺ�")]
    static public int mat_num = 2;
    [Tooltip("��ͼ")]
    public Sprite[] pic;//��ͼ
    SpriteRenderer sr;//��ͼ������
    [Tooltip("��ǰ��ͼ��")]
    static public int sprite_num = 2;//��ͼ��

    //TODO(Hangyu) : ��Enemy����һ�£��ݶ�Ϊint��
    [Tooltip("��ͨ����������")]
    public int mAttack = 1000;

    private GameObject mLeader;
    private int mCurrentNodeIdx;

    private GameObject mCollidedObject = null;

    public void TailChangeSprite(int n)// ������ͼ������
    {
        if (n >= pic.Length && n < 0) { n = 0; }
        sr.sprite = pic[n];
        sr.material = mat[n];
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //����tail nodeλ��
        int searchPosOnTrack = mCurrentNodeIdx * SearchInterval + FirstSearchPosOffset;  //eg: (0 + 1) * 5 ��ʾnode0��SearchPos��Track��һֱΪ5

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
            if (Math.Abs(collidedNodeIdx - mCurrentNodeIdx) > 1)
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
        if (collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RemoteEnemy" || collision.gameObject.tag == "Bullet")
        {
            mCollidedObject = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RemoteEnemy" || collision.gameObject.tag == "Bullet") // �չ�����̨��Ч
        {
            mCollidedObject = collision.gameObject;
        }
    }

    public bool Attack()
    {
        if (!mCollidedObject) return false;

        if(mCollidedObject.gameObject.tag == "Bullet")
        {
            Bullet bullet = mCollidedObject.GetComponent<Bullet>();
            if (!bullet)
            {
                TraceBullet traceBullet = mCollidedObject.GetComponent<TraceBullet>();
                if (!traceBullet)
                    return false;
                traceBullet.Kill();
            }
            else
            {
                bullet.Kill();
            }
        }

        else
        {
            Enemy enemy = mCollidedObject.GetComponent<Enemy>();
            if (!enemy) return false;
            enemy.Damage(mAttack);
        }
        return true;
    }


    public void SetCurrentNodeIdx(int InNodeIdx)
    {
        mCurrentNodeIdx = InNodeIdx;
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

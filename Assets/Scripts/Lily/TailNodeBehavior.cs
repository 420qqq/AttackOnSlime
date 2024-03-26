/*********
 * TailNodeBehavior.cs : 实现TailNode的行为，包括位置更新，碰撞检测等
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
    [Tooltip("材质")]
    public Material[] mat;
    [Tooltip("当前材质号")]
    static public int mat_num = 2;
    [Tooltip("贴图")]
    public Sprite[] pic;//贴图
    SpriteRenderer sr;//贴图父对象
    [Tooltip("当前贴图号")]
    static public int sprite_num = 2;//贴图号

    //TODO(Hangyu) : 根Enemy保持一致，暂定为int型
    [Tooltip("普通攻击攻击力")]
    public int mAttack = 1000;

    private GameObject mLeader;
    private int mCurrentNodeIdx;

    private GameObject mCollidedObject = null;

    public void TailChangeSprite(int n)// 更换贴图及材质
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
        //更新tail node位置
        int searchPosOnTrack = mCurrentNodeIdx * SearchInterval + FirstSearchPosOffset;  //eg: (0 + 1) * 5 表示node0的SearchPos在Track上一直为5

        List<Vector3> track = mLeader.GetComponent<TailController>().GetTrack();
        transform.position = track[searchPosOnTrack];
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO(Hangyu) : 目前只考虑了TailNode之间的碰撞，后续需要考虑其他物体的碰撞
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
        if (collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RemoteEnemy" || collision.gameObject.tag == "Bullet") // 普攻对炮台无效
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

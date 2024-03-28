/*********
 * TailNodeBehavior.cs : ʵ��TailNode����Ϊ������λ�ø��£���ײ����
 * 
 ********/

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class TailNodeBehavior : MonoBehaviour
{
    public static int SearchInterval;
    public static int FirstSearchPosOffset;

    [Tooltip("����")]
    public Material[] mat;
    [Tooltip("��ͼ")]
    public Sprite[] pic;//��ͼ
    //TODO(Hangyu) : ��Enemy����һ�£��ݶ�Ϊint��
    [Tooltip("��ͨ����������")]
    public int mAttack = 5;
    [Tooltip("������Ч����ʱ��")]
    public float mAttackEffectTime = 0.15f;

    private GameObject mLeader;
    private int mCurrentNodeIdx;

    private GameObject mCollidedObject = null;
    private SpriteRenderer sr = null;

    private float mAttackEffectTimer = 0.0f;

    public void TailChangeSprite()// ������ͼ������
    {
        // if (n >= pic.Length && n < 0) { n = 0; }
        sr.sprite = pic[PlayerPrefs.GetInt("SkinNumber", 0)];
        sr.material = mat[PlayerPrefs.GetInt("SkinNumber", 0)];
    }

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        TailChangeSprite();
    }

    // Update is called once per frame
    void Update()
    {
        if (!mLeader) return;
        if (mLeader.GetComponent<TailController>().GetRetraceState()) return;

        //����tail nodeλ��
        int searchPosOnTrack = mCurrentNodeIdx * SearchInterval + FirstSearchPosOffset;  //eg: (0 + 1) * 5 ��ʾnode0��SearchPos��Track��һֱΪ5

        List<Vector3> track = mLeader.GetComponent<TailController>().GetTrack();
        transform.position = track[searchPosOnTrack];

        OperateAttackEffect();
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

    private void OperateAttackEffect()
    {
        if (mAttackEffectTimer > 0.0f)
        {
            switch (PlayerPrefs.GetInt("SkinNumber", 0))
            {
                case 0:
                    GetComponent<SpriteRenderer>().material.SetFloat("_ShineGlow", 0.6f);
                    float currentWidth = GetComponent<SpriteRenderer>().material.GetFloat("_ShineWidth");
                    float speed = 0.5f / mAttackEffectTime;
                    currentWidth += speed * Time.deltaTime;
                    GetComponent<SpriteRenderer>().material.SetFloat("_ShineWidth", currentWidth);
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().material.SetFloat("_FishEyeUvAmount", 0.37f);
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().material.SetFloat("_ZoomUvAmount", 1.8f);
                    break;
            }
            mAttackEffectTimer -= Time.deltaTime;
        }
        else
        {
            switch (PlayerPrefs.GetInt("SkinNumber", 0))
            {
                case 0:
                    GetComponent<SpriteRenderer>().material.SetFloat("_ShineGlow", 0.0f);
                    GetComponent<SpriteRenderer>().material.SetFloat("_ShineWidth", 0.05f);
                    break;
                case 1:
                    GetComponent<SpriteRenderer>().material.SetFloat("_FishEyeUvAmount", 0.0f);
                    break;
                case 2:
                    GetComponent<SpriteRenderer>().material.SetFloat("_ZoomUvAmount", 1.0f);
                    break;
            }
        }
    }


    public bool Attack()
    {
        if (!mCollidedObject) return false;

        if (mCollidedObject.gameObject.tag == "Bullet")
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
    
    public void SetHueShift(float hue)
    {
        GetComponent<SpriteRenderer>().material.SetFloat("_HsvShift", hue);
    }

    public void SetSaturation(float saturation)
    {
        GetComponent<SpriteRenderer>().material.SetFloat("_HsvSaturation", saturation);
    }

    public void SetBrightness(float brightness)
    {
        GetComponent<SpriteRenderer>().material.SetFloat("_HsvBright", brightness);
    }

    public void SetAttackEffectTimer()
    {
        mAttackEffectTimer = mAttackEffectTime;
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

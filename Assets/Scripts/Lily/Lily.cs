using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;

[RequireComponent(typeof(TailController))]
public class Lily : MonoBehaviour
{
    [Tooltip("�ƶ��ٶ�")]
    public float mSpeed = 5.0f;  //Lily�ƶ��ٶ�

    [Tooltip("����ֵ")]
    public float HP = 1000.0f;  //Lily����ֵ

    [Tooltip("�ܻ����޵�ʱ��")]
    public float mInvincibleTime = 1.0f;  //Lily�ܻ����޵�ʱ��

    private bool mFaceToward = true;  //Lily���� ���� trueΪ�ң�falseΪ��
    private float mInvincibleTimer = 0.0f;  //�޵�ʱ���ʱ��


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveVec = Vector2.zero;
        moveVec.y = Input.GetAxis("Vertical");
        moveVec.x = Input.GetAxis("Horizontal");
        if (moveVec.x > 0)
        {
            mFaceToward = true;
        }
        else if (moveVec.x < 0)
        {
            mFaceToward = false;
        }

        GetComponent<SpriteRenderer>().flipX = !mFaceToward;

        transform.Translate(moveVec.y * Vector3.up * mSpeed * Time.smoothDeltaTime, Space.World);
        transform.Translate(moveVec.x * Vector3.right * mSpeed * Time.smoothDeltaTime, Space.World);

        if (mInvincibleTimer > 0.0f) mInvincibleTimer -= Time.deltaTime;

        if (HP <= 0.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Damage(float damage) //Lily�ܵ��˺�
    {
        if (mInvincibleTimer <= 0.0f)
        {
            HP -= damage;
            mInvincibleTimer = mInvincibleTime;
        }
    }

    private void OnDestroy()
    {
        //if any animation is needed


        GetComponent<TailController>().ClearTail();
    }
}

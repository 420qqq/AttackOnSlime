using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using UnityEngine;

public class Lily : MonoBehaviour
{
    public float mSpeed = 5.0f;  //Lily�ƶ��ٶ�

    private bool mFaceToward = true;  //Lily���� ���� trueΪ�ң�falseΪ��

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
        if(moveVec.x > 0)
        {
            mFaceToward = true;
        }
        else if(moveVec.x < 0)
        {
            mFaceToward = false;
        }
        
        GetComponent<SpriteRenderer>().flipX = !mFaceToward;

        transform.Translate(moveVec.y * Vector3.up * mSpeed * Time.smoothDeltaTime, Space.World);
        transform.Translate(moveVec.x * Vector3.right * mSpeed * Time.smoothDeltaTime, Space.World);
    }

}

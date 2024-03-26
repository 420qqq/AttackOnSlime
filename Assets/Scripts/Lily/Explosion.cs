using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Tooltip("��ը����ʱ��")]
    public float mLifeTime = 0.25f;

    [Tooltip("��ը������")]
    public int mAttack = 1000;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        mLifeTime -= Time.deltaTime;
        if (mLifeTime <= 0.0f) Destroy(gameObject);
        GetComponent<SpriteRenderer>().material.SetFloat("_FadeAmount", 1 - mLifeTime * 4.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ( collision.gameObject.tag == "MeleeEnemy" || collision.gameObject.tag == "RemoteEnemy" || collision.gameObject.tag == "TowerEnemy")
        {
            collision.gameObject.GetComponent<Enemy>().Damage(mAttack);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteEnemy : Enemy
{
	private float escapeDistance = 35f;
	public bool isElite = false;
	public CoolDownBar coolDown = null;

    // Start is called before the first frame update
    protected override void Start()
    {
		base.Start();
        Init();
    }

    // Update is called once per frame
    protected override void Update()
    {
		base.Update();
		
		Bullet.SetTargetHero(targetHero);
		TraceBullet.SetTargetHero(targetHero);
		
        // alert mode
		if (GetTargetDistance() < alertDistance && coolDown.ReadyForNext())
		{
			Attack();
		}
		
		// escape mode
		if (GetTargetDistance() < escapeDistance)
		{
			Escape();
		}
    }

	public void SetElite()
	{
		isElite = true;
		SetLife(30); 
	}
    
    void Init()
	{
		SetSpeed(1.5f);
		SetLife(10);
		Bullet.SetTargetHero(targetHero);
		TraceBullet.SetTargetHero(targetHero);
		Bullet.SetAttack(1);
		TraceBullet.SetAttack(1);
	}

	void Attack()
	{
		GameObject newBullet = null;
		if (isElite) 
		{ 
			newBullet = Instantiate(Resources.Load("Prefabs/Enemy/TraceBullet") as GameObject);
		}
		else
		{
			newBullet = Instantiate(Resources.Load("Prefabs/Enemy/Bullet") as GameObject);
		}
		
		newBullet.transform.localPosition = transform.localPosition;
		coolDown.TriggerCoolDown();
	}

	void Escape()
	{
		Vector3 p = transform.position;
		p += -speed * Time.smoothDeltaTime * GetTargetDirection();
		transform.position = p;
	}

	protected override void OnCollisionEnter2D(Collision2D objectName)
    {
		base.OnCollisionEnter2D(objectName);
        if (objectName.gameObject.name == "Map")
        {
            ;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float velocity;
    public Vector3 travelDirection;
    public GameObject hitTarget;
    public int damage;
    public float knockBack;

    public void Init(Vector3 _travelDirection, int _damage, float _knockBack)
    {
        damage = _damage;
        travelDirection = _travelDirection;
        knockBack = _knockBack;
        transform.eulerAngles = new Vector3(0,0, GetAngleFromVectorFloat(travelDirection));
    }
    private void FixedUpdate() 
    {
        transform.position += travelDirection * Time.deltaTime * velocity;

    }

    private float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(n < 0 ) n+= 360;
        return n;
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if(collider.gameObject.layer == 8)
        {
            BulletHitZombie(collider);
        }
        else if(collider.gameObject.layer == 6)
        {
            BulletHitWall();
        }
    }

    private void BulletHitWall()
    {
        Debug.Log("Shot wall");
        Destroy(this.gameObject);
    }

    private void BulletHitZombie(Collider2D collider)
    {
        Unit zombie = collider.GetComponent<Unit>();
        if(!zombie.isDead)
        {
            Debug.Log("shot zombie");
            zombie.locomotionHandler.knockBack_Co = zombie.locomotionHandler.GetKnockedBack(knockBack, transform.position);
            zombie.locomotionHandler.StartCoroutine(zombie.locomotionHandler.knockBack_Co);
            zombie.locomotionHandler.slow_Co = zombie.locomotionHandler.SlowMovement();
            zombie.locomotionHandler.StartCoroutine(zombie.locomotionHandler.slow_Co);
            zombie.TakeDamage(damage);
            Destroy(this.gameObject);
        }

    }
}

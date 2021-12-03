using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    public float velocity;
    public Vector3 travelDirection;
    public GameObject hitTarget;
    public int damage;

    public void Init(Vector3 _travelDirection, int _damage)
    {
        damage = _damage;
        travelDirection = _travelDirection;

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
            zombie.TakeDamage(damage);
            Destroy(this.gameObject);
        }

    }
}

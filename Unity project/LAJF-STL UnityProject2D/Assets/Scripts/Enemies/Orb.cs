using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    // Floating Text
    public GameObject floatingTextPrefab; //, floatingCanvasPrefab;
    public GameObject floatingCanvasParent;
    public GameObject floatingTextInstance;

    public int health;

    public LayerMask layerMask; //Layers this object should interact with and damage.

    public GameObject particleExplosionObj;

    [Range(1, 30)]
    public int damage;

    private OrbEnemy parentOrb;

    [SerializeField]
    [Range(0.5f, 100f)]
    private float projectileSpeed;

    public void Start()
    {
        parentOrb = transform.parent.GetComponent<OrbEnemy>();
    }


    public void TakeDamage(int dmg)
    {
        health -= dmg;

        //TODO: Add some blinking effect or something.

        if (health <= 0)
        {
            Debug.Log("Orb died");
            Instantiate(particleExplosionObj);
            transform.parent.GetComponent<OrbEnemy>().isOrbDead = true;
            Destroy(gameObject);
            //TODO: Add death explosion, sounds and particles and stuff.

            ShowFloatingText(dmg);


        }
    }

    #region FloatingText

    public void ShowFloatingText(int damage)
    {

        if (floatingCanvasParent == null) // if the canvas is not yet instantiated
        {
            floatingCanvasParent = GameObject.Find("FloatingCanvas"); // find the canvas (parent) for text
        }

        float randomX = UnityEngine.Random.Range(-4.5f, 4.5f); // Random position.x
        float randomY = UnityEngine.Random.Range(-1.5f, 4.5f); // Random position.y
        Vector3 randomVector = new Vector3(randomX, randomY, 0); // Random combined position
        floatingTextInstance = Instantiate(floatingTextPrefab, transform.position + randomVector, Quaternion.identity, floatingCanvasParent.transform); // instantiate text object

        floatingTextInstance.GetComponent<FloatingTextEffects>().setText(damage.ToString()); //Sets the text of the text object - the rest will happen in the instance (FloatingTextEffects.cs)
    }


    #endregion FloatingText


    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        if (Projectile.IsInLayerMask(other.layer, layerMask))
        {
            //Visuals, sounds and other could be here, rather than in "takedamage". 
        }
    }

    public void Attack(Transform target)
    {
        Vector2 direction = (target.transform.position - transform.position).normalized;
        GameObject bullet = Instantiate(parentOrb.bulletObj, transform.position + (((Vector3)direction) * 0.2f), Quaternion.identity);

        bullet.GetComponent<Rigidbody2D>().AddForce(direction * projectileSpeed, ForceMode2D.Impulse);
        bullet.GetComponent<Projectile>().damage = damage;

    }
}

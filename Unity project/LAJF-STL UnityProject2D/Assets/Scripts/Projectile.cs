using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage;
    public LayerMask layerMask; //Layers this object should interact with and damage.

    public void OnCollisionEnter2D(Collision2D coll)
    {
        

        if(IsInLayerMask(coll.gameObject.layer, layerMask)){
            Debug.Log("HEY");
            //Do stuff then destroy self.
            GameObject.Destroy(gameObject);
        }
    }



    public static bool IsInLayerMask(int layer, LayerMask _layermask)
    {
        return _layermask == (_layermask | (1 << layer));
    }

}

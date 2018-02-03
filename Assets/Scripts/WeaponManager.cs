using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour{
}


public interface IWeapon
{
    void Shoot(float dmgVal);
}

public class Crossbow : MonoBehaviour, IWeapon
{
    public void Shoot(float dmgVal)
    {

        Vector3 initialPosition = transform.position + new Vector3(0, 0.5f, 0);//new Vector3(curr.transform.position.x, curr.transform.position.y + 1f, 0);
        GameObject arrow = Instantiate(Resources.Load("arrowPrefab", typeof(GameObject))) as GameObject;
        arrow.GetComponent<ArrowBehaviour>().setDmg(dmgVal);
        arrow.transform.position = initialPosition;
        //arrow.transform.rotation = Quaternion.Euler(transform.forward);
        //arrow.transform.LookAt(transform.forward);
        Quaternion rotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
        arrow.GetComponent<Rigidbody>().MoveRotation(rotation);
        //Debug.Log(transform.forward);
        arrow.GetComponent<Rigidbody>().velocity = transform.forward * (dmgVal*50);
        arrow.GetComponent<Rigidbody>().angularVelocity = transform.forward * 5;
    }
}

public class SMG : MonoBehaviour, IWeapon
{
    public void Shoot(float dmgVal)
    {

        //Vector3 initialPosition = new Vector3(this.transform.position.x, this.transform.position.y + 1f, 0);
        //GameObject missile = Instantiate(Resources.Load("MissilePrefab", typeof(GameObject))) as GameObject;
        //missile.transform.position = initialPosition;
        //missile.GetComponent<Rigidbody2D>().velocity = new Vector2(0f, 3f);

    }
}
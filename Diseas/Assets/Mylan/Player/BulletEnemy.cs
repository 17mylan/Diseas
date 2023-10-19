using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public PlayerHealth playerHealth;
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
        Destroy(gameObject, 3f);
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //Désactivé pour la Gym Room
            //playerHealth.RemovePlayerHealth(5);
        }
        Destroy(gameObject);
    }
}
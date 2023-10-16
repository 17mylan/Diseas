using UnityEngine;

public class BulletEnemy : MonoBehaviour
{
    public float bulletSpeed = 10f; // Vitesse de la balle

    void Start()
    {
        // Appliquer une force vers l'avant dès le début
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);

        // Détruire la balle après un certain délai (pour éviter les fuites)
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        // Détruire la balle lorsqu'elle entre en collision avec un autre objet (par exemple, le joueur)
        Destroy(gameObject);
    }
}
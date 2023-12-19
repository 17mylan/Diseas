using System.Collections;
using UnityEngine;

public class LifeCollectible : MonoBehaviour
{
    public float forceSaut = 7f;
    public float sideImpulseRange = 2f; // Nouvelle variable pour la portée de l'impulsion sur le côté
    public GameObject _playerReference;
    public Transform _exampleCharacter;
    public float detectionRange = 7f;
    public float minSpeed = 7f;
    public float maxSpeed = 15f;
    public float delayBeforeCollecting = 1f;

    private PlayerHealth playerHealth;
    private float currentSpeed;
    private bool canCollect = false;

    void Start()
    {
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        if (rigidbody != null)
        {
            // Ajoute l'impulsion vers le haut
            rigidbody.AddForce(Vector3.up * forceSaut, ForceMode.Impulse);

            // Ajoute une impulsion aléatoire sur le côté
            Vector3 sideImpulse = new Vector3(Random.Range(-sideImpulseRange, sideImpulseRange), 0f, 0f);
            rigidbody.AddForce(sideImpulse, ForceMode.Impulse);
        }
        else
        {
            Debug.LogError("Rigidbody non trouvé sur l'objet.");
        }

        _playerReference = GameObject.Find("ExampleCharacter");
        _exampleCharacter = _playerReference.transform;

        playerHealth = _playerReference.GetComponent<PlayerHealth>();

        StartCoroutine(EnableCollecting());
    }

    IEnumerator EnableCollecting()
    {
        yield return new WaitForSeconds(delayBeforeCollecting);
        canCollect = true;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

        if (canCollect && distanceToPlayer <= detectionRange)
        {
            float speedFactor = Mathf.Clamp01(1 - distanceToPlayer / detectionRange);
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);

            transform.position = Vector3.MoveTowards(transform.position, _exampleCharacter.position, currentSpeed * Time.deltaTime);

            if (distanceToPlayer < 1.0f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        float healthToAdd = 10f;
        float remainingHealth = playerHealth.maxPlayerHealth - playerHealth.playerHealth;

        if (healthToAdd > remainingHealth)
        {
            playerHealth.playerHealth = playerHealth.maxPlayerHealth;
        }
        else
        {
            playerHealth.playerHealth += healthToAdd;
        }

        playerHealth.UpdateUI();
        Destroy(gameObject);
        Debug.Log("Collecté");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeCollectible : MonoBehaviour
{
    public GameObject _playerReference;
    public Transform _exampleCharacter;
    public float detectionRange = 10f;
    public float minSpeed = 0.05f;
    public float maxSpeed = 0.2f;

    private PlayerHealth playerHealth; // Référence vers le script PlayerHealth
    private float currentSpeed; // Variable pour stocker la vitesse actuelle

    void Start()
    {
        _playerReference = GameObject.Find("ExampleCharacter");
        _exampleCharacter = _playerReference.transform;

        // Obtient la référence du script PlayerHealth
        playerHealth = _playerReference.GetComponent<PlayerHealth>();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _exampleCharacter.position);

        if (distanceToPlayer <= detectionRange)
        {
            float speedFactor = Mathf.Clamp01(1 - distanceToPlayer / detectionRange);
            currentSpeed = Mathf.Lerp(minSpeed, maxSpeed, speedFactor);

            transform.position = Vector3.MoveTowards(transform.position, _exampleCharacter.position, currentSpeed * Time.deltaTime);

            // Si l'objet est assez proche, déclenche la collecte
            if (distanceToPlayer < 1.0f)
            {
                Collect();
            }
        }
    }

    void Collect()
    {
        // Appelle la fonction RemovePlayerHealth du script PlayerHealth
        float healthToAdd = 10f; // Tu peux ajuster le montant de vie ajouté ici
        float remainingHealth = playerHealth.maxPlayerHealth - playerHealth.playerHealth; // Calcule la vie restante pour atteindre la limite maximale

        // Vérifie si le montant à ajouter dépasse la vie restante pour atteindre la limite maximale
        if (healthToAdd > remainingHealth)
        {
            playerHealth.playerHealth = playerHealth.maxPlayerHealth; // Atteint la vie maximale
        }
        else
        {
            playerHealth.playerHealth += healthToAdd; // Ajoute le montant de vie
        }

        // Met à jour l'UI
        playerHealth.UpdateUI();

        // Autres actions de collecte si nécessaire

        // Détruit l'objet collectible après la collecte
        Destroy(gameObject);
    }
}

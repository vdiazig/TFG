using UnityEngine;
using System.Collections;

using Entities.Class;
using InterfaceAdapters.Presentation.Player;
using Unity.VisualScripting;


namespace InterfaceAdapters.Presentation.Enemies
{
    public class EnemyController : MonoBehaviour
    {
        [Header("Detection Settings")]
        [SerializeField] private string targetTag;

        [Header("Combat Settings")]
        [SerializeField] private GameObject projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private int shotsRemainingMin;
        [SerializeField] private int shotsRemainingMax;
        [SerializeField] private int shotsRemaining;
        [SerializeField] private float timeBetweenAttacksMin;
        [SerializeField] private float timeBetweenAttacksMax;
        [SerializeField] private float timeBetweenAttacks;
        private bool isCharging;
        [SerializeField] private GameObject energyObject;
        [SerializeField] private float maxEnergy = 20;
        [SerializeField] private float energy;
        [SerializeField] private int chargeTimeMin;
        [SerializeField] private int chargeTimeMax;
        [SerializeField] private int chargeTime;

        [Header("Enemy Parts")]
        [SerializeField] private Transform bodyTransform; 
        [SerializeField] private Animator enemyAnimator; 

        private Transform playerTarget;
        

        private void Start()
        {
            shotsRemaining = Random.Range(shotsRemainingMin, shotsRemainingMax);
            timeBetweenAttacks = Random.Range(timeBetweenAttacksMin, timeBetweenAttacksMax);
            chargeTime = Random.Range(chargeTimeMin, chargeTimeMax);
            energy = maxEnergy;
        }


        // Detecta cuando el personaje entra en el área de detección del enemigo
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                Debug.Log($"Player detected: {other.transform.position}");
                playerTarget = other.transform;
            }

            if ((other.CompareTag("Weapon")) && (FindObjectOfType<ThirdPersonController>().IsAttack()))
            {
                UpdateEnergy(1);
            }

        }


        // Detecta cuando el personaje esta dentro del área de detección
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                if (playerTarget != null)
                {
                    AimAtPlayer();
                    Attack();
                }
                
            }

        }

       
        // Detecta cuando el personaje sale del área de detección del enemigo
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(targetTag))
            {
                Debug.Log("Player exit detection range.");
                playerTarget = null;
            }
        }


        // Apunta al jugador
        private void AimAtPlayer()
        {
            if (playerTarget != null)
            {
                Vector3 direction = playerTarget.position - bodyTransform.position;

                direction.Normalize();

                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

                Quaternion offsetRotation = Quaternion.Euler(0, 90f, 0);
                targetRotation *= offsetRotation;

                bodyTransform.rotation = Quaternion.Lerp(bodyTransform.rotation, targetRotation, Time.deltaTime * 5f);
            }
        }



        // Realiza el ataque
        private void Attack()
        {
            if (!isCharging) 
            {
                StartCoroutine(ShootWithDelay());
            }
        }

        // Dispara el proyectil
        private void ShootProjectile()
        {
            if (playerTarget != null)
            {
                var projectileObject = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
                projectileObject.GetComponent<Projectile>().Initialize(firePoint.position, playerTarget.position, this.gameObject);
            }
        }

        // Corrutina para disparar con retraso
        private IEnumerator ShootWithDelay()
        {
            if (isCharging) yield break; 
            isCharging = true;

            while (shotsRemaining > 0)
            {
                ShootProjectile();
                shotsRemaining--;
                yield return new WaitForSeconds(timeBetweenAttacks);
            }

            yield return StartCoroutine(ChargeAtPlayer()); 

            shotsRemaining = Random.Range(shotsRemainingMin, shotsRemainingMax);
            timeBetweenAttacks = Random.Range(timeBetweenAttacksMin, timeBetweenAttacksMax);

            isCharging = false; 
        }

        // Corrutina para la recarga
        private IEnumerator ChargeAtPlayer()
        {
            yield return new WaitForSeconds(chargeTime);

            chargeTime = Random.Range(chargeTimeMin, chargeTimeMax);
        }



        // Actualiza la energía
        public void UpdateEnergy(float damage)
        {
            // Reduce la energía del tanque
            energy = Mathf.Max(energy - damage, 0);

            // Calcula el factor de escala proporcional a la energía restante
            float scaleFactor = energy / maxEnergy; 

            // Actualiza la escala del objeto del tanque
            energyObject.transform.localScale = new Vector3(1, 1, scaleFactor);

            // Enemigo destruido
            if (energy <= 0)
            {
                Debug.Log("Enemy destroyed.");
                enemyAnimator.SetBool("destroy", true); 
                DisableTriggers(); 
            }
        
        }


        // Obtiene todos los colliders en el enemigo y desactiva los que sean triggers
        private void DisableTriggers()
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();
            foreach (var collider in colliders)
            {
                if (collider.isTrigger)
                {
                    collider.enabled = false;
                }
            }
        }
    }
}

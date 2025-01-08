using UnityEngine;
using System.Collections;

using Entities.Items;
using InterfaceAdapters.Managers;
using InterfaceAdapters.Presentation.Enemies;
using InterfaceAdapters.Presentation.Player;

namespace Entities.Class
{
    public class Projectile : MonoBehaviour
    {
        
        [SerializeField] private float speed; 
        [SerializeField] private float lifetime = 3f;
        private Vector3 direction;
        private GameObject enemy;


        // Configura el proyectil con la posición inicial y el destino
        public void Initialize(Vector3 startPosition, Vector3 destination, GameObject enemySource)
        {
            enemy = enemySource;
            destination += Vector3.up * 1.1f; 
            direction = (destination - startPosition).normalized;

            GetComponent<Rigidbody>().velocity = direction * speed;

            StartCoroutine(AutoDestroy());
        }


        // Manejo del impacto del proyectil
        private void OnTriggerEnter(Collider other)
        {

            if ((other.CompareTag("Weapon")) && (FindObjectOfType<ThirdPersonController>().IsAttack()))
            {
                StopCoroutine(AutoDestroy()); 
                StartCoroutine(MoveTowardsEnemy());
            }


            if (other.CompareTag("Enemy"))
            {
                OnHitEnemy();
            }


            if (other.CompareTag("Player"))
            {
                var managerUser = FindObjectOfType<ManagerUser>();
                if (managerUser != null)
                {
                    var damage = GetComponent<WeaponBase>()?.Damage ?? 0;
                    managerUser.Life(damage);
                }
                Destroy(gameObject);
            }


            if (other.CompareTag("Limit"))
            {   
                Destroy(gameObject);
            }
 
        }

        // Corutina para perseguir al enemigo
        private IEnumerator MoveTowardsEnemy()
        {
            while (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) > 0.1f)
            {
                Vector3 newDirection = (enemy.transform.position - transform.position).normalized;
                transform.position += newDirection * speed * Time.deltaTime;
                yield return null;
            }

            // Si alcanza al enemigo
            if (enemy != null && Vector3.Distance(transform.position, enemy.transform.position) <= 0.1f)
            {
                OnHitEnemy();
            }
        }

        // Ataca al enemigo
        private void OnHitEnemy()
        {
            if (enemy != null)
            {
                var damage = GetComponent<WeaponBase>()?.Damage ?? 0;
                enemy.GetComponent<EnemyController>()?.UpdateEnergy(damage);
            }
            Destroy(gameObject);
        }


        // Corrutina para destruir el proyectil tras un tiempo
        private IEnumerator AutoDestroy()
        {
            yield return new WaitForSeconds(lifetime);
            Destroy(gameObject);
        }
    }
}

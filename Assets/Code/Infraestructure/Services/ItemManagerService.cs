using System.Collections.Generic;
using UnityEngine;

using InterfaceAdapters.Interactables;

namespace Infraestructure.Services
{

    public class ItemManagerService : MonoBehaviour
    {
        // Lista pública de prefabs de ítems, máquinas, reciclables, enemigos y jefes
        public List<GameObject> itemPrefabs;
        public List<GameObject> AttackObjectsPrefabs;
        public List<GameObject> weaponPrefabs;
        public List<GameObject> machinePrefabs;
        public List<GameObject> recycledPrefabs;
        public List<GameObject> enemyPrefabs;
        public List<GameObject> bossPrefabs;
        

        // Método que se llama al iniciar el juego 
        void Awake()
        {
            LoadAllItems();  
            LoadAllAttackObjects();
            LoadAllMachines();
            LoadAllRecycled();
            LoadAllEnemies();
            LoadAllBosses();
        }



        //____ CARGA DE PREFABS ____//
        // Método para obtener un prefab de ítem basado en su ID
        public GameObject GetItemPrefab(int itemId)
        {
            return GetPrefabById(itemPrefabs, itemId);
        }

        public GameObject GetRandomPrefab()
        {
            int randomIndex = Random.Range(0, itemPrefabs.Count);
            return itemPrefabs[randomIndex];
        }

        // Método para obtener un prefab de máquina basado en su ID
        public GameObject GetMachinePrefab(int itemId)
        {
            return GetPrefabById(machinePrefabs, itemId);
        }

        // Método para obtener un prefab de objeto reciclado basado en su ID
        public GameObject GetRecycledPrefab(int itemId)
        {
            return GetPrefabById(recycledPrefabs, itemId);
        }

        // Método para obtener un prefab de enemigo basado en su ID
        public GameObject GetEnemyPrefab(int itemId)
        {
            return GetPrefabById(enemyPrefabs, itemId);
        }

        // Método para obtener un prefab de jefe basado en su ID
        public GameObject GetBossPrefab(int itemId)
        {
            return GetPrefabById(bossPrefabs, itemId);
        }




        //____ FUNCIONES INTERNAS ____//
        // Método para obtener un prefab basado en su ID
        private GameObject GetPrefabById(List<GameObject> prefabs, int itemId)
        {
            foreach (GameObject prefab in prefabs)
            {
                RecyclableItem item = prefab.GetComponent<RecyclableItem>();
                if (item != null && item.ObjectID == itemId)
                {
                    return prefab;
                }
            }

            Debug.LogError("No se encontró ningún prefab con el ID: " + itemId);
            return null;
        }



        //____ GENERAR LAS LISTAS DE PREFABS ____//
        // Método para cargar todos los prefabs de ítems
        private void LoadAllItems()
        {
            itemPrefabs.Clear();

            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Items/Reciclable");
            itemPrefabs = new List<GameObject>(prefabs);
            //Debug.Log("Se han cargado " + itemPrefabs.Count + " ítems.");
        }

        // Método para cargar todos los prefabs de AttackObjects
        private void LoadAllAttackObjects()
        {
            AttackObjectsPrefabs.Clear();

            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Items/AttackObjects");
            AttackObjectsPrefabs = new List<GameObject>(prefabs);
            //Debug.Log("Se han cargado " + AttackObjectsPrefabs.Count + " ítems.");
        }

        // Método para cargar todos los prefabs de máquinas
        private void LoadAllMachines()
        {
            machinePrefabs.Clear();

            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Items/Machines");
            machinePrefabs = new List<GameObject>(prefabs);
            //Debug.Log("Se han cargado " + machinePrefabs.Count + " máquinas.");
        }

        // Método para cargar todos los prefabs reciclables
        private void LoadAllRecycled()
        {
            recycledPrefabs.Clear();
            
            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Items/Recycled");
            recycledPrefabs = new List<GameObject>(prefabs);
            //Debug.Log("Se han cargado " + recycledPrefabs.Count + " reciclados.");
        }

        // Método para cargar todos los prefabs de enemigos
        private void LoadAllEnemies()
        {
            enemyPrefabs.Clear();

            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Enemies");
            enemyPrefabs = new List<GameObject>(prefabs);
            //Debug.Log("Se han cargado " + enemyPrefabs.Count + " enemigos.");
        }

        // Método para cargar todos los prefabs de jefes
        private void LoadAllBosses()
        {   
            bossPrefabs.Clear();

            GameObject[] prefabs = Resources.LoadAll<GameObject>("Prefabs/Bosses");
            bossPrefabs = new List<GameObject>(prefabs);
            //Debug.Log("Se han cargado " + bossPrefabs.Count + " jefes.");
        }

    }
}

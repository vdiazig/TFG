using System.Collections.Generic;
using UnityEngine;

using Entities.Class;

namespace InterfaceAdapters.Adapters
{
    public class JSONDataAdapter
    {
        // Serializa los datos de items
        public static string SerializeItems(List<NewItem> items)
        {
            return JsonUtility.ToJson(new ItemListWrapper { items = items });
        }

        // Recupera los datos de items
        public static List<NewItem> DeserializeItems(string json)
        {
            return JsonUtility.FromJson<ItemListWrapper>(json).items;
        }



        // Serializa los datos del jugador 
        public static string SerializePlayerStats(PlayerStatsData stats)
        {
            if (stats == null)
            {
                Debug.LogError("Cannot serialize null stats data.");
                return string.Empty;
            }

            return JsonUtility.ToJson(stats);
        }

        // Restaura los datos del jugador
        public static PlayerStatsData DeserializePlayerStats(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Empty or null JSON for stats deserialization.");
                return new PlayerStatsData(); // Devuelve un objeto vacío por defecto
            }

            return JsonUtility.FromJson<PlayerStatsData>(json);
        }



        // Serializa los datos de exploración
        public static string SerializeExplorationData(List<ExplorationData> explorationData)
        {
            return JsonUtility.ToJson(new ExplorationListWrapper { exploration = explorationData });
        }


        // Recupera los datos de exploración
        public static List<ExplorationData> DeserializeExplorationData(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                Debug.LogWarning("Empty or null JSON for exploration deserialization.");
                return new List<ExplorationData>(); // Devuelve una lista vacía por defecto
            }

            return JsonUtility.FromJson<ExplorationListWrapper>(json).exploration;
        }

        
    }
}
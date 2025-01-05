// Wrapper para serializar/deserializar la lista de datos de exploración

using System.Collections.Generic;

namespace Entities.Class{
    [System.Serializable]
    public class ExplorationListWrapper
    {
        public List<ExplorationData> exploration;
    }
}
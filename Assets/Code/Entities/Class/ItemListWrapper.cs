// Wrapper para serializar/deserializar la lista de ítems

using System.Collections.Generic;

namespace Entities.Class{
    [System.Serializable]
    public class ItemListWrapper
    {
        public List<NewItem> items;
    }
}
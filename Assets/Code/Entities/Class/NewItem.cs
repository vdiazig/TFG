namespace Entities.Class{
    [System.Serializable]
    public class NewItem
    {
        public int id;         // ID del ítem
        public int quantity;   // Cantidad del ítem

        // Constructor
        public NewItem(int id, int quantity)
        {
            this.id = id;
            this.quantity = quantity;
        }
    }
}

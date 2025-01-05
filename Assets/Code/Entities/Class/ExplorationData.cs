using UnityEngine;


namespace Entities.Class{
    [System.Serializable]
    public class ExplorationData
    {
        [SerializeField] private string id;
        [SerializeField] private bool isLocked;
        [SerializeField] private bool isCompleted;

        public string Id => id;
        public bool IsLocked => isLocked;
        public bool IsCompleted => isCompleted;

        public ExplorationData(string id, bool isLocked = true, bool isCompleted = false)
        {
            this.id = id;
            this.isLocked = isLocked;
            this.isCompleted = isCompleted;
        }

        // Métodos para actualizar el estado del área
        public void ChangeLock()
        {
            isLocked = !isLocked;
        }

        public void ChangeComplete()
        {
            isCompleted = !isCompleted;
        }
    }

}

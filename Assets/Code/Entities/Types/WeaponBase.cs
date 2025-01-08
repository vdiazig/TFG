using UnityEngine;
using Entities.Types;

namespace Entities.Items
{
    public class WeaponBase : MonoBehaviour
    {
        [SerializeField]private int weaponID;
            public int WeaponID => weaponID;
        [SerializeField]private string weaponName;
        [SerializeField]private InteractionType interaction;
        public InteractionType Interaction => interaction;
        [SerializeField]private AttackPlayerType attackPlayer;
        public AttackPlayerType AttackPlayer => attackPlayer;
        [SerializeField]private bool rightHand;
            public bool RightHand => rightHand;
        [SerializeField]private float damage;
            public float Damage => damage;

        [SerializeField] private bool isUnlocked;
            public bool IsUnlocked => isUnlocked;

        [SerializeField] private Sprite weaponIcon;
            public Sprite WeaponIcon => weaponIcon;



        [Header("IF APPLICABLE")]

        [SerializeField][Tooltip("Range of the weapon (if applicable).")]
        private float range;

        [SerializeField][Tooltip("Animator for weapon-specific animations (optional).")]
        private Animator weaponAnimator;


        public void PlayAnimation(string animationName)
        {
            if (weaponAnimator != null)
            {
                weaponAnimator.Play(animationName);
            }
        }
        
    }
}

using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewHints", menuName = "Hints/PlayerHintData")]
    public class PlayerHintData : ScriptableObject
    {
        public Sprite mistakeSprite;
        public Sprite workSprite;
    }
}
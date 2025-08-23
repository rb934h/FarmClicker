using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class TileChanger : MonoBehaviour
    {
        [SerializeField] private Tilemap tilemap;
        [SerializeField] private TileReplacementRule _rule;

        private bool reverse;


        public void ChangeTiles()
        {
            for (int i = 0; i < _rule.from.Length; i++)
            {
                if (reverse)
                    tilemap.SwapTile(_rule.from[i], _rule.to[i]);
                else
                {
                    tilemap.SwapTile(_rule.to[i], _rule.from[i]);
                }
            }
            
            reverse = !reverse;
        }
    }
}
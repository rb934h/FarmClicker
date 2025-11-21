using UnityEngine;
using UnityEngine.Tilemaps;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewTileReplacementRule", menuName = "TileReplacementRules/TileReplacementRule")]
    public class TileReplacementRule : ScriptableObject
    {
        public TileBase[] from;
        public TileBase[] to;
    }
}
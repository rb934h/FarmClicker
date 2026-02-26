using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace PointerObjects
{
    public class WaterTank : PointerObject
    {
        [Header("Tile changer")]
        [SerializeField] private Tilemap _tilemap;
        [SerializeField] private TileReplacementRule _rule;
    
        [HideInInspector]
        [FormerlySerializedAs("State")] public WaterTankState _state = WaterTankState.Empty;
    
        private TileChanger _tileChanger;

        private void Start()
        {
            _tileChanger = new TileChanger(_tilemap, _rule);
            _workTime = 1f;
        }
    
        public void ChangeTile()
        {
            _tileChanger.ChangeTiles();
        }
        
        
    }
}

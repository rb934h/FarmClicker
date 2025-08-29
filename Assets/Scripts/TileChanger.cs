using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger
{
    private Tilemap _tilemap;
    private TileReplacementRule _rule;

    private bool isSwapped = false;

    public TileChanger(Tilemap tilemap, TileReplacementRule rule)
    {
        _tilemap = tilemap;
        _rule = rule;
    }
    public void ChangeTiles()
    {
        var from = isSwapped ? _rule.to : _rule.from;
        var to   = isSwapped ? _rule.from : _rule.to;

        int length = Mathf.Min(from.Length, to.Length);
        for (int i = 0; i < length; i++)
            _tilemap.SwapTile(from[i], to[i]);
        
        isSwapped = !isSwapped;
    }
}
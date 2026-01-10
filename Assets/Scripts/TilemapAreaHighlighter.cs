using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapAreaHighlighter
{
    private readonly Tilemap _tilemap;
    private readonly Collider2D _areaCollider;
    private Vector3Int _minCell;
    private Vector3Int _maxCell;

    public TilemapAreaHighlighter(Tilemap tilemap, Collider2D areaCollider)
    {
        _tilemap = tilemap;
        _areaCollider = areaCollider;
        UpdateBounds();
    }
    
    public void ChangeTilesColor(float duration, Color targetColor)
    {
        foreach (var cellPos in GetCellsInCollider())
        {
            AnimateTileColor(cellPos, targetColor, duration);
        }
    }
    
    private void AnimateTileColor(Vector3Int cellPos, Color targetColor, float duration)
    {
        _tilemap.SetTileFlags(cellPos, TileFlags.None);

        var startColor = _tilemap.GetColor(cellPos);

        DOTween.To(
            () => startColor,
            c =>
            {
                startColor = c;
                _tilemap.SetColor(cellPos, startColor);
            },
            targetColor,
            duration
        );
    }
    
    private IEnumerable<Vector3Int> GetCellsInCollider()
    {
        for (var x = _minCell.x; x <= _maxCell.x; x++)
        {
            for (var y = _minCell.y; y <= _maxCell.y; y++)
            {
                var cellPos = new Vector3Int(x, y, 0);

                if (!_tilemap.HasTile(cellPos)) 
                    continue;

                var worldPos = _tilemap.GetCellCenterWorld(cellPos);

                if (_areaCollider.OverlapPoint(worldPos))
                    yield return cellPos;
            }
        }
    }
    
    private void UpdateBounds()
    {
        var bounds = _areaCollider.bounds;
        _minCell = _tilemap.WorldToCell(bounds.min);
        _maxCell = _tilemap.WorldToCell(bounds.max);
    }
}

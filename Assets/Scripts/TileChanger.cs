using Enum;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileChanger : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private EnumMap<TileReplacementRuleTypes, TileReplacementRule> _rules;

    private bool isSwapped;

    public void ChangeTiles(TileReplacementRuleTypes ruleType)
    {
        var rule = _rules[ruleType];

        var from = isSwapped ? rule.to : rule.from;
        var to   = isSwapped ? rule.from : rule.to;

        int length = Mathf.Min(from.Length, to.Length);
        for (int i = 0; i < length; i++)
            tilemap.SwapTile(from[i], to[i]);

        isSwapped = !isSwapped;
    }
}
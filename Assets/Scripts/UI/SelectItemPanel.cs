using ScriptableObjects;
using UI;
using UnityEngine;
using VContainer;

public class SelectItemPanel : MonoBehaviour
{
    [SerializeField] private AddToInventoryButton addToInventoryButton;

    private Player _player;
   
    [Inject]
    public void Construct(Player player)
    {
        _player = player;
    }
    public void AddButton(CollectableItemData collectableItemData)
    {
        var instance = Instantiate(addToInventoryButton, transform);
        instance.Construct(_player);
        instance.SetCollectableItemData(collectableItemData);
    }
}

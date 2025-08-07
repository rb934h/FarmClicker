using TMPro;
using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
   [SerializeField] private TMP_Text _coinsCountText;
   private float _coins;

   public void SetMoney(float seedingPrice)
   {
      _coins += seedingPrice;
      UpdateCoinsCountText();
   }
   
   private void UpdateCoinsCountText()
   {
      if (_coinsCountText != null)
      {
         _coinsCountText.text = $"Coins: {_coins}";
      }
   }
}

using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerWallet : MonoBehaviour
{
   [SerializeField] private TMP_Text coinsCountText;
   
   private float _coins;

   public void SetMoney(float seedingPrice)
   {
      _coins += seedingPrice;
      UpdateCoinsCountText();
   }
   
   public float GetMoney()
   {
      return _coins;
   }
   
   private void UpdateCoinsCountText()
   {
      if (coinsCountText != null)
      {
         coinsCountText.text = $"Coins: {_coins}";
      }
   }
}

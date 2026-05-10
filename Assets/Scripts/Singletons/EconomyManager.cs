using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static  EconomyManager Instance;
    
    int currentScrap;
    
    public int CurrentScrap => currentScrap;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else Instance = this;
    }

    void Start()
    {
        currentScrap = GameBalanceManager.Instance.StartingMoney;
    }

    public bool TryPurchase(int purchaseCost)
    {
        if (purchaseCost < 0) return false;
        if (currentScrap < purchaseCost) return false;

        currentScrap -= purchaseCost;
        return true;
    }
    
    public void AddScrap(int amount)
    {
        currentScrap += amount;
    }
}

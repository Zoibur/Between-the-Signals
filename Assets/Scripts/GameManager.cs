using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField]
    private int score = 0;
    
    public void Awake()
    {
        Instance = this;
    }

    public void AddScore(int amount)
    {
        if (amount <= 0)
            return;
        
        score += amount;
    }

    public void DeductScore(int amount)
    {
        if (amount <= 0)
            return;
        
        score -= amount;
    }
}

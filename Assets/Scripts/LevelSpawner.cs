using UnityEngine;

public class LevelSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject levelPrefab;
    public int numberLevels = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for(int i=0; i <numberLevels; i++)
        {
            GameObject newLevel = Instantiate(levelPrefab, transform); 
            LevelButton lb = newLevel.GetComponent<LevelButton>();
            lb.SetUp(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

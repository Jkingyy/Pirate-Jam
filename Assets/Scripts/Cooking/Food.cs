using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private FoodSO foodSO;

    public GameObject foodObj;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount == 0)
        {
            foodSO.ResetIndex();
            foodSO.currentFoodPrefab = foodSO.foodPrefabs[0];
            SpawnFood();
        }
    }

    public void Chop()
    {
        if (!foodSO.isChoppable) return;
        Destroy(foodObj);
        if (IncrementFoodIndex())
        {
            SpawnFood();
        }
    }
    private bool IncrementFoodIndex()
    {
        foodSO.foodIndex++;

        if (foodSO.foodIndex >= foodSO.foodPrefabs.Length)
        {
            Destroy(gameObject);
            return false;
        }
        foodSO.currentFoodPrefab = foodSO.foodPrefabs[foodSO.foodIndex];
        return true;
    }

    private void SpawnFood()
    {

        foodObj = Instantiate(foodSO.currentFoodPrefab, transform.position, Quaternion.identity, transform);
    }

    // Update is called once per frame
    void Update()
    {

    }
}

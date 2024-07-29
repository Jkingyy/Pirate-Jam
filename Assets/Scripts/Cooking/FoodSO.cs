using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Food", menuName = "Food")]
public class FoodSO : ScriptableObject
{
    public string foodName;
    public GameObject[] foodPrefabs;
    public int foodIndex = 0;
    public GameObject currentFoodPrefab;

    public bool isChoppable = false;
    public bool isCookable = false;
    public bool isMixable = false;

    public void ResetIndex(){
        foodIndex = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] private GameObject[] foodPrefabs;
    private GameObject currentFoodPrefab;
    public int foodIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        currentFoodPrefab = Instantiate(foodPrefabs[foodIndex], transform.position, Quaternion.identity,this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void IncrementFoodIndex(){
        foodIndex++;

        
    }
    public void ChangeFoodPrefab(Worktop worktop){
        if(foodIndex >= foodPrefabs.Length){
            worktop.heldItem = null;
            Destroy(this.gameObject);
            return;
        }
        Destroy(currentFoodPrefab);
        currentFoodPrefab = Instantiate(foodPrefabs[foodIndex], transform.position, Quaternion.identity,this.transform);
    }
}

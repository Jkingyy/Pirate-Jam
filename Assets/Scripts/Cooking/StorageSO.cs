using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Storage", menuName = "Storage")]
public class StorageSO : ScriptableObject
{
    public Dictionary<GameObject,int> storage = new Dictionary<GameObject, int>();



    public void AddItem(GameObject food, int quantity){
        if(storage.ContainsKey(food)){
            storage[food] += quantity;
        } else {
            storage.Add(food, quantity);
        }
    }

    public void RemoveItem(GameObject food, int quantity){
        if(storage.ContainsKey(food)){
            storage[food] -= quantity;
            if(storage[food] <= 0){
                storage.Remove(food);
            }
        }
    }
}

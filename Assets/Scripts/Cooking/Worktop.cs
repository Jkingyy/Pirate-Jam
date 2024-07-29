using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worktop : MonoBehaviour, IInteractable
{

    public GameObject heldItem;
    private PlayerInteractions _playerInteractions;
    private Transform _objAnchor;
    public bool hasChoppingBoard = false;
    public bool isBin = false;
    public bool isStorage = false;
    public bool isInfinite = false;

    private void Start() {
        _objAnchor = transform.Find("Obj Anchor");
        if(_objAnchor == null) return;
        if(_objAnchor.childCount > 0){
            heldItem = _objAnchor.GetChild(0).gameObject;
        } else {
            heldItem = null;
        }

        Transform choppingBoard = transform.Find("Chopping Board");
        if(choppingBoard != null){
            hasChoppingBoard = true;
        }
    }

    public void Interact(GameObject player, bool holdingItem, Transform playerObjAnchor){
        _playerInteractions = player.GetComponent<PlayerInteractions>();
        if(holdingItem){
            if(heldItem != null) return;
            Drop();
        } else {
            if(heldItem == null && !isStorage) return;
            PickUp(player, playerObjAnchor);
        }
    }
    public void Action(){
        if(hasChoppingBoard){
            Chop();
        }
    }
    private void Chop(){
        Food food = heldItem.GetComponent<Food>();
        if(food == null) return;
        food.Chop();
    }


    private void PickUp(GameObject player, Transform playerObjAnchor){

        if(isInfinite){
            GameObject item = Instantiate(heldItem, playerObjAnchor);
            _playerInteractions.heldItem  = item;
            ResetPositionAndRotation(_playerInteractions.heldItem);
            _playerInteractions.holdingItem = true;
            return;
        }

        if(isStorage){
            //remove item from storage
            Debug.Log("Item removed from storage");
            return;
        }

        heldItem.transform.SetParent(playerObjAnchor);
        _playerInteractions.heldItem = heldItem;
        heldItem = null;

        ResetPositionAndRotation(_playerInteractions.heldItem);
        _playerInteractions.holdingItem = true;
    }
    private void Drop(){
        heldItem = _playerInteractions.heldItem; 
        _playerInteractions.heldItem = null;
        _playerInteractions.holdingItem = false;
        if(isBin){
            Destroy(heldItem);
            if(isStorage){
                //add item to storage
                Debug.Log("Item added to storage");
            }
            return;
        }
        heldItem.transform.SetParent(_objAnchor);
        ResetPositionAndRotation(heldItem);    
    }

    private void ResetPositionAndRotation(GameObject obj){
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }
}

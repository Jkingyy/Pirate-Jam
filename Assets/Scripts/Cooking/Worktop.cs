using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worktop : MonoBehaviour, IInteractable
{

    public GameObject heldItem;
    private PlayerInteractions _playerInteractions;
    private Transform _objAnchor;
    public bool hasChoppingBoard = false;

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
            if(heldItem == null) return;
            PickUp(player, playerObjAnchor);
        }
    }

    private void PickUp(GameObject player, Transform playerObjAnchor){
        heldItem.transform.SetParent(playerObjAnchor);
        _playerInteractions.heldItem = heldItem;
        heldItem = null;
        ResetPositionAndRotation(_playerInteractions.heldItem);
        _playerInteractions.holdingItem = true;
    }
    private void Drop(){
        heldItem = _playerInteractions.heldItem; 
        _playerInteractions.heldItem = null;
        heldItem.transform.SetParent(_objAnchor);
        ResetPositionAndRotation(heldItem);    
        _playerInteractions.holdingItem = false;
    }

    private void ResetPositionAndRotation(GameObject obj){
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
    }
}

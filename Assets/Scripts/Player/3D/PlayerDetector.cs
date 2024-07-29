using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetector : MonoBehaviour
{
    public GameObject currentTargetCounter;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other) {
        currentTargetCounter = other.gameObject;
    }
    private void OnTriggerExit(Collider other) {
        currentTargetCounter = null;
    }
}

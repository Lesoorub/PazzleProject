using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StayedNodeVisualizer : MonoBehaviour
{
    public Button interactBtn;
    public MapHeroController heroController;
    void Start()
    {
        heroController.OnMovementStaring.AddListener((node) =>
        {
            interactBtn.interactable = false;
        });
        heroController.OnMovementEnding.AddListener((node) =>
        {
            interactBtn.interactable = node.OnInteract.GetPersistentEventCount() != 0;
        });
    }


    void Update()
    {
        
    }
}

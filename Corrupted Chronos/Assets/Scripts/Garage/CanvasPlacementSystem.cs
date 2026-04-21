using UnityEngine;


public class CanvasPlacementSystem : MonoBehaviour
{
    PlacementSystem placementSystem;

    //todo probably merge with inventoryManager/or complement it

    void Start()
    {
        placementSystem=transform.GetComponentInParent<PlacementSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartPlacementParts(int id)
    {
        placementSystem.StartPlacementGeneral(id, ConfigurationNau.Parts);
    }
    public void StartPlacementNau(int id)
    {
        placementSystem.StartPlacementGeneral(id, ConfigurationNau.Naus);
    }
    public void StartPlacementTraveler(int id)
    {
        placementSystem.StartPlacementGeneral(id, ConfigurationNau.Pilots);
    }

    public void StartPlacement()
    {
        
    }


    public void SaveButton()
    {
        placementSystem.SavePlacement();
    }

    public void ResetButton()
    {
        placementSystem.ResetPlacement();
    }

}

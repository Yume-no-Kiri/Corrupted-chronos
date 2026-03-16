using UnityEngine;


public class CanvasPlacementSystem : MonoBehaviour
{
    PlacementSystem placementSystem;



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

    public void SaveButton()
    {
        placementSystem.SavePlacement();
    }

    public void ResetButton()
    {
        placementSystem.ResetPlacement();
    }

}

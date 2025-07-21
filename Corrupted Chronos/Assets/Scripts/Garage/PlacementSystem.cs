using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    //[SerializeField]
    //private InputManager inputManager;
    
    [SerializeField]
    private GameObject mouseIndicator;
    
    
    [SerializeField]
    private Grid grid;
    
    private void Update()
    {
        //if (buildingState == null)
          //  return;
        //Vector3 mousePosition = inputManager.GetSelectedMapPosition();
       // mouseIndicator.transform.position = mousePosition;
        
        //Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        /*if(lastDetectedPosition != gridPosition)
        {
            buildingState.UpdateState(gridPosition);
            lastDetectedPosition = gridPosition;
        }*/
        
    }
}

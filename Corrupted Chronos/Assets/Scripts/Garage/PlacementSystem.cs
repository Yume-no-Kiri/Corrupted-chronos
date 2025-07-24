using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    //[SerializeField]
    //private InputManager inputManager;
    
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    
    [SerializeField]
    private GameObject inputManagerObject;
    private InputManager inputManager;
    
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private PartsDatabaseSO database;

    private int selectedObjectIndex = -1;

    /*[serializedFile]
     * private gameobject gridVisualization;
     */

    private void Awake()
    {
        inputManager = inputManagerObject.GetComponent<InputManager>();
    }

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int id)
    {
        selectedObjectIndex = database.AllParts.FindIndex(data => data.ID == id);
        if (selectedObjectIndex < 0)
        {
            Debug.LogError(id + " no existe");
            return;
        }
        //gridVisualization.SetActive(true);
        //cellIndicator.SetActive(true)
        inputManager.OnClick += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        //StopPlacement();
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3Int gridPosition = grid.WorldToCell(inputManager.MousePosition);
        //mouseIndicator.transform.position = inputManager.MousePosition;
        Debug.Log("VENGA OSTIA"+selectedObjectIndex);
        GameObject partToAdd= Instantiate(database.AllParts[selectedObjectIndex].Prefab);
        partToAdd.transform.position = grid.CellToWorld(gridPosition);
    }
    
    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        //gridVisualization.SetActive(false);
        //cellIndicator.SetActive(false)
        inputManager.OnClick -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;    }


    public void StartPart1()
    {
        Debug.Log("selected metralleta");
    }
     
    public void StartPart2()
    {
        Debug.Log("selected other");
    }
    
    private void Update()
    {
        
        // això està al player update i potser s'hauria de moure aquì 
        /*Vector3Int gridPosition = grid.WorldToCell(inputManager.MousePosition);
        mouseIndicator.transform.position = inputManager.MousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
        */
        if (inputManager.playerInputActions.Garage.enabled)
        {
            Vector3Int gridPosition = grid.WorldToCell(inputManager.MousePosition);
            mouseIndicator.transform.position = inputManager.MousePosition;
            cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        }
    }
}

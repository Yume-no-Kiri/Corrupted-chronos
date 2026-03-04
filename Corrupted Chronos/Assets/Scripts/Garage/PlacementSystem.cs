using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;




public class PlacementSystem : MonoBehaviour
{
    //[SerializeField]
    //private InputManager inputManager;
    
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    
    [SerializeField]
    //private GameObject inputManagerObject;
    private InputManager inputManager;
    
    [SerializeField]
    private Grid grid;

    private PartsDatabaseSO database;

    //index del objecte segons la llista de parts tots
    private int selectedObjectIndex = -1;

    private ObjectData _selectedObject=null;

    private List<ObjectData> _selectedListConfig;

    //el esquema, amb parts afeguides de la nau
    private GridData schemeShip = new GridData(); //, cablesShip;
    

    private List<GameObject> _placedObjects;

    private GameObject _previewObject;


    private List<GameObject> _debugCubes= new List<GameObject>();
    public GameObject debugCube;


    [SerializeField]
    private GameObject schemePlain;
    private Vector3 _baseNauPos= Vector3.zero;
    private Vector3 _OffSetPos=  new Vector3(-0.25f, 0, -0.25f);

    [SerializeField] private GameObject player;
    private Player nauAdded;
    int rotationToAdd=0;

    #region awake, start, update
    private void Awake()
    {
        _placedObjects = new List<GameObject>();
        
    }

    private void Start()
    {
        database=GameManager.Instance.dataBaseParts;
        StopPlacement();
        
        FirstStructure();
        
        GameManager.Instance.inputManager.RotateLeft+=RotateLeft; 
        GameManager.Instance.inputManager.RotateRight+=RotateRight; 

        
    }
   
    private void Update()
    {
        if (inputManager.playerInputActions.Garage.enabled)
        {

            Vector3Int gridPosition = grid.WorldToCell(inputManager.MousePosition);
            // Vector3 worldPos = grid.CellToWorld(gridPosition);

            bool placementValidity;
           
            if (_previewObject)
            {
                if(_selectedObject!= null){ 
                    placementValidity = CheckPlacementValidity(gridPosition,_selectedObject);
                }
                else return;
                
    
                // Ara apliques la rotació visual al GameObject
                float angle = rotationToAdd * 90f;
                _previewObject.transform.rotation = Quaternion.Euler(0, angle, 0);

                cellIndicator.SetActive(false);
                _previewObject.transform.position = grid.CellToWorld(gridPosition)+ new Vector3(0,0.5f,0);
               
                foreach (var sr in _previewObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (placementValidity)
                    {
                        sr.color = Color.green; 
                    }
                    else
                    {
                        sr.color = Color.red;
                    }
                }
            }
            else
            {
                cellIndicator.SetActive(true);
                cellIndicator.transform.position = grid.CellToWorld(gridPosition);

            }
            
        }
    }
    #endregion

    #region rotate
    //Change rotation, so the rotation is not saved "universaly", but only in the selected object, a copy 
    void RotateLeft(InputAction.CallbackContext ctx)
    {
        
        if (_previewObject)
        {
            rotationToAdd-=1;
            if(rotationToAdd<0) rotationToAdd=3;
            else if(rotationToAdd>3) rotationToAdd=0;  
        }
        Debug.Log("rotate left detected");

    }
    void RotateRight(InputAction.CallbackContext ctx)
    {
       
        if (_previewObject)
        {
            rotationToAdd+=1;
            if(rotationToAdd<0) rotationToAdd=3;
            else if(rotationToAdd>3) rotationToAdd=0;
        }
        Debug.Log("rotate right detected");
        
    }
    #endregion
    

    #region botons to place

    public void StartPlacementParts(int id)
    {
        StartPlacementGeneral(id, ConfigurationNau.Parts);
    }
    public void StartPlacementNau(int id)
    {
        StartPlacementGeneral(id, ConfigurationNau.Naus);
    }
    public void StartPlacementTraveler(int id)
    {
        StartPlacementGeneral(id, ConfigurationNau.Pilots);
    }


    public void ResetPlacement()
    {
        //this should be looked when player is reworked
        foreach (var placedObject in _placedObjects) Destroy(placedObject);
        _placedObjects.Clear();
        if (nauAdded != null)
        {    
            nauAdded.RemovePart();
        }
        schemeShip.ResetPlacementData();


        FirstStructure();
        // StopPlacement();
        
    }

    public void SavePlacement()
    {
        if(!nauAdded) nauAdded = player.GetComponent<Player>();
        player.transform.rotation=quaternion.identity;

        if (nauAdded == null) {
            Debug.LogError("didn't find nau");
            return;
        }
        if (nauAdded != null)
        {    
            nauAdded.RemovePart();
        }

       Vector3 origin= _placedObjects[0].transform.position;
        if (_placedObjects.Count > 0)
        {
            for (int i = 1; i < _placedObjects.Count; i++)
            {
                GameObject part=_placedObjects[i];
                
                nauAdded.AddPart(part,  origin);
            }
        }

        nauAdded.ActivateParts();

    }
    #endregion

    
    #region place stuff
    //els botons no permeten mètodes on es passen més de 2 parametres
    public void StartPlacementGeneral(int id, ConfigurationNau config)
    {
        switch (config)
        {
            case ConfigurationNau.Parts:
                _selectedListConfig = database.AllParts;
                selectedObjectIndex = _selectedListConfig.FindIndex(data => data.ID == id);
                break;
            case ConfigurationNau.Naus:
                _selectedListConfig = database.AllNaus;
                selectedObjectIndex = _selectedListConfig.FindIndex(data => data.ID == id);
                break;
            case ConfigurationNau.Pilots:
                _selectedListConfig= database.AllPilots;
                selectedObjectIndex = _selectedListConfig.FindIndex(data => data.ID == id);
                break;
            default:
                Debug.LogError("Configuració no possible, placementSystem.CS switch");
                break;
        }
        
        
        if (selectedObjectIndex < 0)
        {
            Debug.LogError(id + " no existe");
            return;
        }
 
        if(_previewObject !=null)          Destroy(_previewObject);
        _previewObject= Instantiate(_selectedListConfig[selectedObjectIndex].PrefabGaratge);

        
        _selectedObject=_selectedListConfig[selectedObjectIndex];
        Debug.LogWarning("això va 1?"+ _selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Occupied].Count);

        Debug.LogWarning("això va 2?"+ _selectedObject.ConfigGround[TypeGround.Occupied].Count);

        Transform coll = _previewObject.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);

        inputManager.OnClick += ()=> PlaceStructure();
        inputManager.OnExit += StopPlacement;
        
    }
    

    //escape de colocar parts
    private void StopPlacement()
    {
        rotationToAdd=0;
        selectedObjectIndex = -1;
        
        Destroy(_previewObject);
        _previewObject = null;
        _selectedObject=null;

        
        inputManager.OnClick -= ()=> PlaceStructure();
        inputManager.OnExit -= StopPlacement;
        
    }
    
    void FirstStructure()
    {
       
        StartPlacementGeneral(0,ConfigurationNau.Naus);
        PlaceStructure( true);
        StopPlacement();
    }
    
   //quan un objecte seleccionat fem click al terreny o primera estructura
    private void PlaceStructure( bool firstStructure=false)
    {
        Vector3Int gridPosition;
        GameObject partToAdd;
        if(firstStructure){ 
            _baseNauPos=schemePlain.GetComponent<Renderer>().bounds.center;
           
            if(_baseNauPos== Vector3.zero)  Debug.LogError("error amb _baseNausPos?");

            Vector3 newPosition=_baseNauPos- new Vector3(-0.5f,0,-0.5f);
            gridPosition = grid.WorldToCell(newPosition);
            partToAdd = Instantiate(_selectedListConfig[selectedObjectIndex].PrefabGaratge);
            Debug.Log("placestructure position instantiate nau:"+gridPosition);
            partToAdd.transform.position = grid.CellToWorld(gridPosition) + _OffSetPos;//+ Vector3Int.FloorToInt(_OffSetPos)
            Debug.Log("placestructure position instantiate nau:"+ grid.CellToWorld(gridPosition) + _OffSetPos);
        
        }else{ 
            if (inputManager.IsPointerOverUI())
            {
                Debug.Log("Pointer over UI");
                return;
            }
            gridPosition = grid.WorldToCell(inputManager.MousePosition);
            // Debug.Log("gridTo Pos PlaceStructure:"+ gridPosition);

            bool placementValidity=false;
            if(_selectedObject!=null) placementValidity = CheckPlacementValidity(gridPosition, _selectedObject);

            if (!placementValidity) return;

            var object2inst=_selectedObject;   //_selectedListConfig[selectedObjectIndex];
            if(!object2inst.PrefabGaratge) Debug.LogError("error prefab");
            if(!_placedObjects[0].transform.Find("Added").transform) Debug.LogError("No podem afegir parts");
            partToAdd= Instantiate(object2inst.PrefabGaratge, _placedObjects[0].transform.Find("Added").transform);
            

            partToAdd.transform.position = grid.CellToWorld(gridPosition);//+ Vector3Int.FloorToInt(_OffSetPos)
        }
        
        float angle = rotationToAdd * 90f;
        partToAdd.transform.rotation = Quaternion.Euler(0, angle, 0);

        Transform coll = partToAdd.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);

        _placedObjects.Add(partToAdd);
        // Debug.Log("placestructure position adding nau:"+gridPosition);    
        schemeShip.AddObjectAt(gridPosition, _selectedObject, rotationToAdd); // _placedObjects.Count-1
        StopPlacement();
    }

    bool CheckPlacementValidity(Vector3Int gridPosition, ObjectData partAColocar)
    {
        if(partAColocar==null) return false;
        
        // Debug.LogWarning("entres dintre de check placement validity???");
        return schemeShip.CanPlaceObejctAt(gridPosition,partAColocar, rotationToAdd);
        
    }
  
    #endregion
 
    #region enable/disable
    
    private void OnEnable()
    {
        Debug.Log("OnEnable1");
        if (_placedObjects.Count > 0)
        {
            foreach (GameObject placedObject in _placedObjects)
            {
                placedObject.SetActive(true);
            }
        }

        Debug.Log("OnEnable2");
    }

    private void OnDisable()
    {
        StopPlacement();
        if (_placedObjects!= null)
        {
            foreach (GameObject placedObject in _placedObjects)
            {
                //això és feo, I know, pero no tinc un error raro quan li dono stop, i aquesta és la solució 
                if (placedObject != null)
                {
                    placedObject.SetActive(false);
                }
            }
        }
    }
    #endregion

   #region debug
    private void ShowBuildGrid()
    {
        var buildPositions=_selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Buildable];

        int i = 0;
        foreach (var item in buildPositions)
        {
            if(i< _debugCubes.Count)
            {
                _debugCubes[i].transform.position = grid.CellToWorld(new Vector3Int(item.x,0,item.y));
                _debugCubes[i].SetActive(true);
            }else 
            {
                GameObject cube = Instantiate(debugCube,_previewObject.transform);
                cube.transform.position = grid.CellToWorld(new Vector3Int(item.x,0,item.y));
                _debugCubes.Add(cube);
                
            }
            print("position : x:"+item.x+" y:" +item.y);
        }
        for (int j = i; j < _debugCubes.Count; j++)
        {
            _debugCubes[j].SetActive(false);
        }
    }
  
    #endregion
} 

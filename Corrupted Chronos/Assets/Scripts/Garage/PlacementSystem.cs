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

    private List<ObjectData> _selectedListConfig;

    //el esquema, amb parts afeguides de la nau
    private GridData schemeShip; //, cablesShip;
    

    private List<GameObject> _placedObjects;

    private GameObject _previewObject;
    private List<GameObject> _debugCubes= new List<GameObject>();
    public GameObject debugCube;


    [SerializeField]
    private GameObject schemePlain;
    private Vector3 _baseNauPos= Vector3.zero;
    private Vector3 _OffSetPos= new Vector3(-1f,0,-1f);

    [SerializeField] private GameObject player;
    private Player nauAdded;
    Vector3 rotationToAdd;

    public bool canShowBuildGrid;
    private int rotationPart=0; //entre 0 i 3, right +1; left-1
    private void Awake()
    {
        _placedObjects = new List<GameObject>();
        
        //inputManager = inputManagerObject.GetComponent<InputManager>();
    }

    private void Start()
    {
        database=GameManager.Instance.dataBaseParts;
        StopPlacement();
        //cablesShip = new GridData();
        
        FirstStructure();
        StopPlacement();
        
        GameManager.Instance.inputManager.RotateLeft+=RotateLeft; 
        GameManager.Instance.inputManager.RotateRight+=RotateRight; 

        
    }
   
    private void Update()
    {
        
       
        if (inputManager.playerInputActions.Garage.enabled)
        {
            Vector3Int gridPosition = grid.WorldToCell(inputManager.MousePosition);
            
            int placementValidity =CheckPlacementValidityInt(gridPosition,selectedObjectIndex);
            //pre.material.color=placementValidity ? Color.red : Color.white;
            if (_previewObject)
            {
                cellIndicator.SetActive(false);
                _previewObject.transform.position = grid.CellToWorld(gridPosition)+ new Vector3(0,0.5f,0);
                foreach (var sr in _previewObject.GetComponentsInChildren<SpriteRenderer>())
                {
                    if (placementValidity==1)
                    {
                       sr.color = Color.green; 
                    }
                    else if(placementValidity==2)
                    {
                        sr.color = Color.blue;
                    }else if (placementValidity == 3)
                    {
                        sr.color = Color.red;
                    }else if (placementValidity == 4)
                    {
                        sr.color = Color.yellow;
                    }

                    
                }
            }
            else
            {
                cellIndicator.SetActive(true);
                cellIndicator.transform.position = grid.CellToWorld(gridPosition);

            }
            
            
            //mouseIndicator.transform.position = inputManager.MousePosition;

        }
    }

    void RotateLeft(InputAction.CallbackContext ctx)
    {
        /* rotationPart=-1;
        if(rotationPart<0) rotationPart=3;
        if(rotationPart>3) rotationPart=0; */

        Debug.Log("rotate left detected");
        if (_previewObject)
        {
            //  Dictionary<TypeGround, HashSet<Vector2Int>> llistaRotada;
            HashSet<Vector2Int> setRotada= new HashSet<Vector2Int>();
            
            _previewObject.transform.Rotate(0, -90, 0, Space.World);

            foreach (TypeGround tGround in Enum.GetValues(typeof(TypeGround)))
            {
                if (!_selectedListConfig[selectedObjectIndex].ConfigGround.ContainsKey(tGround)) continue;

                foreach (var item in _selectedListConfig[selectedObjectIndex].ConfigGround[tGround])
                {
                    setRotada.Add(new Vector2Int(-item.y, item.x));
                    /* Vector3 novaRotacio = _previewObject.transform.eulerAngles;
                    novaRotacio.y -= 45f;
                    rotationToAdd=novaRotacio;
                    _previewObject.transform.eulerAngles = novaRotacio; */
                    
                }
                _selectedListConfig[selectedObjectIndex].ConfigGround[tGround]=setRotada;

                if(canShowBuildGrid) ShowBuildGrid();
                
            }
            
            // .ConfigGround[TypeGround.Occupied]
        }
    }
    void RotateRight(InputAction.CallbackContext ctx)
    {
        /* rotationPart=+1;
        if(rotationPart<0) rotationPart=3;
        if(rotationPart>3) rotationPart=0; */
        if (_previewObject)
        {
            // Dictionary<TypeGround, HashSet<Vector2Int>> llistaRotada;
            HashSet<Vector2Int> setRotada= new HashSet<Vector2Int>();
            
            _previewObject.transform.Rotate(0, 90, 0, Space.World);
            

            foreach (TypeGround tGround in Enum.GetValues(typeof(TypeGround)))
            {
                if (!_selectedListConfig[selectedObjectIndex].ConfigGround.ContainsKey(tGround)) continue;
                
                foreach (var item in _selectedListConfig[selectedObjectIndex].ConfigGround[tGround])
                {
                    setRotada.Add(new Vector2Int(item.y, -item.x));

                    /* Vector3 novaRotacio = _previewObject.transform.eulerAngles;
                    novaRotacio.y += 45f;
                    rotationToAdd=novaRotacio;
                    _previewObject.transform.eulerAngles = novaRotacio; */



                }
                _selectedListConfig[selectedObjectIndex].ConfigGround[tGround] = setRotada;

                if(canShowBuildGrid) ShowBuildGrid();
            }

            // .ConfigGround[TypeGround.Occupied]
        }

        Debug.Log("rotate right detected");
        
    }

    private void ShowBuildGrid()
    {
        // float leng= 0.5f;
        var buildPositions=_selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Buildable];

        // int maxsize = _selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Buildable].Count;
        int i = 0;
        foreach (var item in buildPositions)
        {
            //grid.CellToWorld(new Vector3Int(item.x, 0, item.y));
            if(i< _debugCubes.Count)
            {
                
            
                _debugCubes[i].transform.localPosition = new Vector3(item.x, 0, item.y);
                _debugCubes[i].SetActive(true);
                // i++;
            }else //if (maxsize <= i)
            {
                
                GameObject cube = Instantiate(debugCube,_previewObject.transform);
                cube.transform.localPosition = new Vector3(item.x, 0, item.y);
                _debugCubes.Add(cube);
                
                /* cube.transform.SetParent(_previewObject.transform);
                _debugCubes.Add(cube); */
                // maxsize++;
            }
            print("position : x:"+item.x+" y:" +item.y);
        }
        for (int j = i; j < _debugCubes.Count; j++)
        {
            _debugCubes[j].SetActive(false);
        }
    }
    /* private void showBuildAll(GameObject father){
       
        foreach (TypeGround tGround in Enum.GetValues(typeof(TypeGround)))
        {
            
            if (!_selectedListConfig[selectedObjectIndex].ConfigGround.ContainsKey(tGround)) continue;
            foreach (var item in _selectedListConfig[selectedObjectIndex].ConfigGround[tGround])
            {
                
               
                    GameObject cube=Instantiate(debugCube,new Vector3( item.x,0,item.y), quaternion.identity); 
                    if(tGround==TypeGround.Occupied) cube.transform.localEulerAngles = new Vector3(0,50,0);

                    cube.transform.SetParent(father.transform);


                    _debugCubes.Add(cube);
                    // maxsize++;
           }
        }
    } */

    void FirstStructure()
    {
        
        schemeShip = new GridData();
        //_baseNauPos= schemePlain.GetComponentInChildren<Transform>().position;
        StartPlacementGeneral(0,ConfigurationNau.Naus);
        _baseNauPos=schemePlain.GetComponent<Renderer>().bounds.center;
        Debug.Log("abseNau Pos1:"+_baseNauPos);
        /* _baseNauPos+=_OffSetPos;
        Debug.Log("abseNau Pos2:"+_baseNauPos); */

        if(_baseNauPos== Vector3.zero)  Debug.LogError("error amb _baseNausPos?");
        PlaceFirstStructure(_baseNauPos);
        
       // _baseNauPos = 
    }
    

   //mètodes que es cridaran per botons

    #region botons

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
        foreach (var placedObject in _placedObjects) Destroy(placedObject);
        _placedObjects.Clear();
        if (nauAdded != null)
        {    
            nauAdded.RemovePart();
        }
        FirstStructure();
        StopPlacement();
        //WhereToAddParts();
    }

    public void SavePlacement()
    {


        //nauAdded= nauTransform.Find("Added").GameObject();

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
                //nauAdded.AddPart2(_placedObjects[0].transform.Find("Added").transform);
            }
        }

        nauAdded.ActivateParts();

    }
    #endregion
    /*public void WhereToAddParts()
    {
        Transform nauTransform = player.transform.Find("Nau");
        Transform addedTransform = nauTransform != null ? nauTransform.Find("Added") : null;
        nauAdded = addedTransform != null ? addedTransform.gameObject : null;
        if (player == null)
        {
            Debug.LogError("Player is null");   
        }
        else if (nauTransform == null)
        {
            Debug.LogError("Child 'Nau' not found under player.");
        }else if (addedTransform == null)
        {
            Debug.LogError("Child 'Added' not found under 'Nau'.");
        }
    }*/
    
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
        //gridVisualization.SetActive(true);
        //cellIndicator.SetActive(true)
        
        //objecte per veure el preview
        if(_previewObject !=null)          Destroy(_previewObject);
        _previewObject= Instantiate(_selectedListConfig[selectedObjectIndex].PrefabGaratge);
        Transform coll = _previewObject.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);

        foreach (var item in _selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Buildable])
        {
            GameObject cube=Instantiate(debugCube,new Vector3( item.x,0,item.y), quaternion.identity); 
            cube.transform.SetParent(_previewObject.transform);
            _debugCubes.Add(cube);
            

        }

        inputManager.OnClick += PlaceStructure;
        //inputManager.OnClick += () => StartCoroutine(DelayedPlace());
        inputManager.OnExit += StopPlacement;
        
    }
    

    //escape de colocar parts
    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        
        Destroy(_previewObject);
        _previewObject = null;
        
        //gridVisualization.SetActive(false);
        //cellIndicator.SetActive(false)
        inputManager.OnClick += PlaceStructure;
        //inputManager.OnClick -= () => StartCoroutine(DelayedPlace());
        inputManager.OnExit -= StopPlacement;
        
    }
    
    //quan un objecte seleccionat fem click al terreny
    // ReSharper disable Unity.PerformanceAnalysis
    private void PlaceStructure()
    {
        //StopPlacement();
        if (inputManager.IsPointerOverUI())
        {
            Debug.Log("Pointer over UI");
            return;
        }
        Vector3Int gridPosition = grid.WorldToCell(inputManager.MousePosition);
        
        bool placementValidity =CheckPlacementValidity(gridPosition,selectedObjectIndex);
        if (!placementValidity) return;
        //          ||||||

        var object2inst=_selectedListConfig[selectedObjectIndex];
        if(!object2inst.PrefabGaratge) Debug.LogError("error prefab");
        if(!_placedObjects[0].transform.Find("Added").transform) Debug.LogError("No podem afegir parts");
        //peta aquí vvvvvv        
        GameObject partToAdd= Instantiate(object2inst.PrefabGaratge, _placedObjects[0].transform.Find("Added").transform);
        // partToAdd.transform.eulerAngles=rotationToAdd;
        partToAdd.transform.position = grid.CellToWorld(gridPosition);
        partToAdd.transform.rotation= _previewObject.transform.rotation;
        
        // showBuildAll(partToAdd);

        Transform coll = partToAdd.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);
        
        _placedObjects.Add(partToAdd);
        //revisar 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip : 
            cablesShip;
        */
        //selectedData.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].Size,
        schemeShip.AddObjectAt(gridPosition,object2inst.ConfigGround[TypeGround.Occupied],
            object2inst.ConfigGround[TypeGround.Buildable],
            object2inst.ID,
            _placedObjects.Count-1);
        StopPlacement();
    }
    
    
    private void PlaceFirstStructure(Vector3 newPosition)//Vector3Int offset, bool applyToSprite)
    {
        //StopPlacement();
        /*if (inputManager.IsPointerOverUI())
        {
            Debug.Log("Pointer over UI");
            return;
        }*/
        //   _OffSetPos = new Vector3(grid.cellSize.x / 2, 0, grid.cellSize.z / 2);
          _OffSetPos = new Vector3(-0.25f, 0, -0.25f);
        Debug.Log("gridTo Pos2:"+ _OffSetPos);
        // newPosition+=_OffSetPos;
        Vector3Int gridPosition = grid.WorldToCell(newPosition) ;
        Debug.Log("gridTo Pos1:"+ gridPosition);
      
        //mouseIndicator.transform.position = inputManager.MousePosition;
        //Debug.Log("VENGA OSTIA"+selectedObjectIndex);
        
        //bool placementValidity =CheckPlacementValidity(gridPosition,selectedObjectIndex);
        //if (!placementValidity) return;
        
        GameObject partToAdd= Instantiate(_selectedListConfig[selectedObjectIndex].PrefabGaratge);

        partToAdd.transform.position = grid.CellToWorld(gridPosition);//+ Vector3Int.FloorToInt(_OffSetPos)
        
        Transform coll = partToAdd.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);

        
        newPosition-=_OffSetPos;
        gridPosition = grid.WorldToCell(newPosition) ;
        
        
        _placedObjects.Add(partToAdd);
        //revisar 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip : 
            cablesShip;
        */ 
        
        //selectedData.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].Size,
        
        foreach (var item in _selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Occupied])
        {
            GameObject cube=Instantiate(debugCube,new Vector3( item.x,0,item.y), quaternion.identity); 
            cube.transform.localEulerAngles = new Vector3(0,50,0);

            cube.transform.SetParent(partToAdd.transform);
        }
        
        foreach (var item in _selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Buildable])
        {
            GameObject cube=Instantiate(debugCube,new Vector3( item.x,0,item.y), quaternion.identity); 

            cube.transform.SetParent(partToAdd.transform);
        }

        // _debugCubes.Add(cube);
        
        schemeShip.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Occupied],
            _selectedListConfig[selectedObjectIndex].ConfigGround[TypeGround.Buildable],
            _selectedListConfig[selectedObjectIndex].ID,
            _placedObjects.Count-1);
        
        
        // showBuildAll(partToAdd);

        /*if (applyToSprite)
        {
            Transform sp = partToAdd.GetComponentInChildren<SpriteRenderer>().transform;
            if (sp != null)
            {
                Vector3Int gp1 = grid.WorldToCell(newPosition) -offset;
                sp.position = grid.CellToWorld(gp1);
                Debug.Log("PERFAVOR");
            }
        
        }*/
    }
    
    
    bool CheckPlacementValidity(Vector3Int gridPosition, int selectedIndex)
    {
        if(selectedIndex < 0 ) return false;
        
        //això és perque al tutorial podies col·locar un terra especial i tenia id=0, com aquesta es podia construïr per sota dels mobles
        //el separaba en un altre 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip :
                cablesShip;
        */

        return schemeShip.CanPlaceObejctAt(gridPosition,_selectedListConfig[selectedIndex].ConfigGround[TypeGround.Occupied]);
        
    }
    
    int CheckPlacementValidityInt(Vector3Int gridPosition, int selectedIndex)
    {
        if(selectedIndex < 0 ) return 0;
        
        //això és perque al tutorial podies col·locar un terra especial i tenia id=0, com aquesta es podia construïr per sota dels mobles
        //el separaba en un altre 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip : 
                cablesShip;
        */
        foreach (var pos in _selectedListConfig[selectedIndex].ConfigGround[TypeGround.Occupied])
        {
            print("occupied pos:" + pos.ToString());
        }
        return schemeShip.CanPlaceObejctAt2(gridPosition,_selectedListConfig[selectedIndex].ConfigGround[TypeGround.Occupied]);
        
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

   /*  public void DebugGridRepresentation(Vector2Int centre)
    {

        List<Vector3Int> ocuPos=grid.retur
       Vector3 radi3= grid.cellSize;
        float radix=radi3.x;
        float radiy=radi3.y;
    string visualGrid = "Visualització del Grid (Actual):\n";
    
    // Recorrem de dalt a baix (Y) i d'esquerra a dreta (X)
    for (float y = centre.y + radiy; y >= centre.y - radiy; y--)
    {
        string fila = "";
        for (float x = centre.x - radix; x <= centre.x + radix; x++)
        {
            Vector2Int posActual = new Vector2Int(x, y);
            
            // Comprovem què hi ha en aquesta posició dins del teu HashSet d'objectes seleccionats
            // (Nota: Adapta 'config' al nom de la teva variable de dades actual)
            var config = _selectedListConfig[selectedObjectIndex].ConfigGround;
            
            if (config[TypeGround.Occupied].Contains(posActual))
            {
                fila += "[ O ]";
            }
            else if (config[TypeGround.Buildable].Contains(posActual))
            {
                fila += "[ B ]";
            }
            else
            {
                fila += "[ X ]";
            }
        }
        visualGrid += fila + "\n";
    }
    
    Debug.Log(visualGrid);
    }*/
} 

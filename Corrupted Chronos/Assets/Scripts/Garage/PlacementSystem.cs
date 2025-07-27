using System;
using System.Collections;
using System.Collections.Generic;
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

    [SerializeField]
    private PartsDatabaseSO database;

    //index del objecte segons la llista de parts tots
    private int selectedObjectIndex = -1;

    private List<ObjectData> _selectedListConfig;

    //el esquema, amb parts afeguides de la nau
    //un seria per les parts a afegir de la nau, l'altra seria per els cables, conectar energia i altres modificacions
    // a sobre del esquema normal, encara no implementat i revisar segons la implementació de GridData
    private GridData schemeShip; //, cablesShip;
    
    /*[serializedFile]
     * private gameobject gridVisualization;
     */

    private List<GameObject> _placedObjects;

    private GameObject _previewObject;

    [SerializeField]
    private GameObject schemePlain;
    private Vector3 _baseNauPos;

    [SerializeField] private GameObject player;
    private Player nauAdded;

    private void Awake()
    {
        _placedObjects = new List<GameObject>();
        
        //inputManager = inputManagerObject.GetComponent<InputManager>();
    }

    private void Start()
    {
        StopPlacement();
        //cablesShip = new GridData();
        
        FirstStructure();
        StopPlacement();
        
        
    }

    void FirstStructure()
    {
        schemeShip = new GridData();
        //_baseNauPos= schemePlain.GetComponentInChildren<Transform>().position;
        StartPlacementGeneral(0,ConfigurationNau.Naus);
        _baseNauPos=schemePlain.GetComponent<Renderer>().bounds.center;

        
        PlaceFirstStructure(_baseNauPos);
        
       // _baseNauPos = 
    }
    

   //mètodes que es cridaran per botons
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
        StartPlacementGeneral(id, ConfigurationNau.Travelers);
    }

    public void ResetPlacement()
    {
        foreach (var placedObject in _placedObjects) Destroy(placedObject);
        _placedObjects.Clear();
        if (nauAdded != null)
        {    //   foreach (var son in nauAdded.GetComponentsInChildren<GameObject>())
            //{
              //  Destroy(son);
            //}
            nauAdded.RemovePart();
        }
        FirstStructure();
        StopPlacement();
        //WhereToAddParts();
    }

    public void SavePlacement()
    {


        //nauAdded= nauTransform.Find("Added").GameObject();
        nauAdded = player.GetComponent<Player>();
        if (nauAdded == null) {
            Debug.LogError("didn't find nau");
        
            return;
        }
        //Debug.Log(_baseNauPos +"CCCCCCCCC");

       Vector3 origin= _placedObjects[0].transform.position;
      // Debug.Log(origin +"AAAAAAAAAA");

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
            case ConfigurationNau.Travelers:
                _selectedListConfig= database.AllTravelers;
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
        _previewObject= Instantiate(_selectedListConfig[selectedObjectIndex].Prefab);
        Transform coll = _previewObject.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);
        
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
        
        GameObject partToAdd= Instantiate(_selectedListConfig[selectedObjectIndex].Prefab, _placedObjects[0].transform.Find("Added").transform);
        partToAdd.transform.position = grid.CellToWorld(gridPosition);
        
        Transform coll = partToAdd.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);
        
        _placedObjects.Add(partToAdd);
        //revisar 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip : 
            cablesShip;
        */
        //selectedData.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].Size,
        schemeShip.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].Size,
            _selectedListConfig[selectedObjectIndex].BuildSize,
            _selectedListConfig[selectedObjectIndex].ID,
            _placedObjects.Count-1);
        
    }
    
    
    private void PlaceFirstStructure(Vector3 newPosition)//Vector3Int offset, bool applyToSprite)
    {
        //StopPlacement();
        /*if (inputManager.IsPointerOverUI())
        {
            Debug.Log("Pointer over UI");
            return;
        }*/
        Vector3Int gridPosition = grid.WorldToCell(newPosition) ;
        //mouseIndicator.transform.position = inputManager.MousePosition;
        //Debug.Log("VENGA OSTIA"+selectedObjectIndex);
        
        //bool placementValidity =CheckPlacementValidity(gridPosition,selectedObjectIndex);
        //if (!placementValidity) return;
        
        GameObject partToAdd= Instantiate(_selectedListConfig[selectedObjectIndex].Prefab);
        partToAdd.transform.position = grid.CellToWorld(gridPosition);//- offset;
        
        Transform coll = partToAdd.transform.Find("Collisions");
        if (coll != null) coll.gameObject.SetActive(false);

        
        
        
        
        _placedObjects.Add(partToAdd);
        //revisar 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip : 
            cablesShip;
        */ 
        //selectedData.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].Size,
        schemeShip.AddObjectAt(gridPosition,_selectedListConfig[selectedObjectIndex].Size,
            _selectedListConfig[selectedObjectIndex].BuildSize,
            _selectedListConfig[selectedObjectIndex].ID,
            _placedObjects.Count-1);
        
        
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

        return schemeShip.CanPlaceObejctAt(gridPosition,_selectedListConfig[selectedIndex].Size);
        
    }
    
    int CheckPlacementValidityInt(Vector3Int gridPosition, int selectedIndex)
    {
        if(selectedIndex < 0 ) return 0;
        
        //això és perque al tutorial podies col·locar un terra especial i tenia id=0, com aquesta es podia construïr per sota dels mobles
        //el separaba en un altre 
        /*GridData selectedData = _selectedListConfig[selectedObjectIndex].ID == 0 ? schemeShip : 
                cablesShip;
        */

        return schemeShip.CanPlaceObejctAt2(gridPosition,_selectedListConfig[selectedIndex].Size);
        
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
}

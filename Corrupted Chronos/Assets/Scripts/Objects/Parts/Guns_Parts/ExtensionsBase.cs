using System;
using System.Collections;
using UnityEngine;



public class ExtensionsBase : GunBase
{
    
    protected override void OnEnable()
    {
        ObjectNameID="ExtensionObject";
        // = 1f;
        
    }

    void Awake()
    {
        //hauria de tenir una posició especial
        //agafar en el placement data que hi ha en aquella posició, 
        //si hi ha un gun, agafo el component i afegeixo el firepoint
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

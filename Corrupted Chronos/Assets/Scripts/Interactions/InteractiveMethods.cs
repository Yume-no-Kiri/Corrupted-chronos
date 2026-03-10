using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;



//EXPLICACIÓ:
//crida de mètodes del jugador degut a certes coses, estar en una zona concreta, etc..
//zones que també es podrien crear aquí: obrir botiga, conversa,
//està separat per poder mantenir aquests "casos especials" fora de la logica del jugador i que estigui més ordenat
//es crea al script player i s'afegeix al mateix lloc que player
// zones que també es podrien crear aquí: obrir botiga, menu?

//TODO after demo
//revisar totes les crides de player.InputManger, ja que moltes podrien tenir la instancia
//del inputManager i cridar player NOMÉS, quan sigui necesari 


//es creen per dir quin mètode es crida, i està assignat a cada objecte que afegeix un mètode interactiu al jugador
//probablemente revisar naupilotposition i pilotnauposition
public enum InteractionType
{
    NauPilot,
    PilotNau,
    NauPilotPosition,
    PilotNauPosition,
    PnINp,
    Talk,
    OpenGarage,
    CloseGarage

    
} 
public enum NameInputAction{
    Nau,
    Pilot,
    Garage
}


public class InteractiveMethods : MonoBehaviour
{
    

    //public List<String> ListMethods = new List<String>();
   // public List<MethodPlayer> ListMethodDelegates = new List<MethodPlayer>();
    
   //string estatAnterior="";
   private InteractionType estatAnterior;
    private void Start()
    {
        //ListMethods.Add("Nau_Pilot");
        //ListMethods.Add("Pilot_Nau");
     //   ListMethodDelegates.Add(Nau_Pilot);
     //   ListMethodDelegates.Add(Pilot_Nau);
    }

    //fer les interaccions del jugador, segons la llista d'ionteraccions que tenim
    public void DoInteractions(List<InteractionType> interactions, Player player)
    {
        Debug.Log("do itneractions %i:"+interactions.Count);

        //revisar funcionament, un cop surtis d'un dels menus hauries d'esperar a que aquest acabi per poder invocar el següent
        for (int i = 0; i < interactions.Count; i++)
        {
            DoInteraction(interactions[i], player);
        }
      
    }

    //fer interacció sola, relació entre interactionType i cada mètode
    private void DoInteraction(InteractionType interaction, Player player)
    {
        switch (interaction)
        {
            case InteractionType.NauPilot:
                Nau_Pilot(player);
                break;
            case InteractionType.PilotNau:
                Pilot_Nau(player);
                break;
            case InteractionType.PilotNauPosition:
                Pilot_Nau_Position(player);
                break;
            case InteractionType.NauPilotPosition:
                Nau_Pilot_Position(player);
                break;
            case InteractionType.PnINp:
                PnINp(player);
                break;

            case InteractionType.Talk:
                Talk(player);
                break;
            case InteractionType.OpenGarage:
                OpenGarage(player);

                break;
            case InteractionType.CloseGarage:
                CloseGarage(player);
                break;
            default:
                Debug.LogError("Interaction method no existeix, InteractiveMethods.cs");
                break;
        }
    }

    

    
    //canvia entre el mode nau a pilot, desactivant els inputs necessaris

    public void Nau_Pilot_Position(Player jugador)
    {
        //v1: si HangleChangeInputMap es al update
       /*  jugador.inputManager.playerInputActions.Nau.Disable();
        jugador.inputManager.playerInputActions.Pilot.Enable(); */

        //v2: HangleChangeInputMap no es al update
        jugador.AccesChangeInputMap(NameInputAction.Nau, false);
        jugador.AccesChangeInputMap(NameInputAction.Pilot, true);

        


        Debug.Log("AAAAAAAAAAAAAAAAAAAA");



        var vector3 = jugador.transform.position;
        vector3.z = vector3.z + 10f;
        jugador.transform.position = vector3;
        
        

        //jugador.transform.eulerAngles = Vector3.zero;

        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques
        InteractionType i= InteractionType.NauPilotPosition;
        jugador.SubInteraction(i);
    }
    
    
    public void Nau_Pilot(Player jugador)
    {
       /*  jugador.inputManager.playerInputActions.Nau.Disable();
        jugador.inputManager.playerInputActions.Pilot.Enable();
        */ 
        jugador.AccesChangeInputMap(NameInputAction.Nau, false);
        jugador.AccesChangeInputMap(NameInputAction.Pilot, true);


        jugador.transform.eulerAngles = Vector3.zero;

        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques
        InteractionType i= InteractionType.NauPilot;
        jugador.SubInteraction(i);
    }
    
    public void Pilot_Nau_Position(Player jugador)
    {
        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques
        /* jugador.inputManager.playerInputActions.Pilot.Disable();
        jugador.inputManager.playerInputActions.Nau.Enable();
 */
        jugador.AccesChangeInputMap(NameInputAction.Pilot, false);
        jugador.AccesChangeInputMap(NameInputAction.Nau, true);



        var vector3 = jugador.transform.position;
        vector3.z = vector3.z - 10;
        jugador.transform.position = vector3;


        // jugador.ClearDetectorsCapa();
        
        InteractionType i= InteractionType.PilotNauPosition;
        jugador.SubInteraction(i);

    }
    
    
    //canvia de pilot a nau, desactivant els inputs necessaris 
    public void Pilot_Nau(Player jugador)
    {
        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques

        /* jugador.inputManager.playerInputActions.Nau.Enable();
        jugador.inputManager.playerInputActions.Pilot.Disable(); */

        jugador.AccesChangeInputMap(NameInputAction.Nau, true);
        jugador.AccesChangeInputMap(NameInputAction.Pilot, false);


        InteractionType i= InteractionType.PilotNau;
        jugador.SubInteraction(i);
    }
    

    //canvia de pilot a nau i viceversa segons quin mode estem
    public void PnINp(Player jugador)
    {
        //Debug.Log("enter pninp");
        if (jugador.inputManager.playerInputActions.Pilot.enabled)
        {
            Pilot_Nau(jugador);
        }else if (jugador.inputManager.playerInputActions.Nau.enabled)
        {
            Nau_Pilot(jugador);
        }
        
        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques
        InteractionType i= InteractionType.PnINp;
        jugador.SubInteraction(i);

    }



    //sugerencia per col·locar els dialegs:
    /*
     Crea un mètode dialeg, que segons el player i alguna variable, cridi la teva logica de player,
     una mena d'interficie per la teva logica, que li donaria permis a fucnionar. Osigui un únic mètode de dialeg, 
     per tots els dialegs, que tocaries en un altre script.
     
     Com passar informació del mètode? 
     bé, aquí pots tocar-ho de diferentes maneres, per exemple, tindre un script a cada npc amb informació de la teva logica dels dialegs 
     que han de tenir, i que guardes a player quan afegeixes la interacció, amb logica similar al ontrigger enter i ontrigger exit de
     change nau pilot 
     */
     
    public void OpenGarage(Player jugador)
    {
        
        if (jugador.inputManager.playerInputActions.Pilot.enabled)
        {
            estatAnterior = InteractionType.NauPilot;
            // jugador.inputManager.playerInputActions.Pilot.Disable();

            jugador.AccesChangeInputMap(NameInputAction.Pilot, false);
            
        }else if (jugador.inputManager.playerInputActions.Nau.enabled)
        {
            estatAnterior = InteractionType.PilotNau;
            // jugador.inputManager.playerInputActions.Nau.Disable();
            jugador.AccesChangeInputMap(NameInputAction.Nau, false);

        }

        // jugador.inputManager.playerInputActions.Garage.Enable();
        jugador.AccesChangeInputMap(NameInputAction.Garage, true);

        InteractionType i= InteractionType.OpenGarage;
        jugador.SubInteraction(i);
        
        //sempre que s'obri el garatge s'afegeix per poder tancar-no, no trobo no tindria sentit fer-ho així
        //pero si es vulgues fer, seria afegir un if aquí sota
        i= InteractionType.CloseGarage;
        jugador.AddInteraction(i);
    }
    private void CloseGarage(Player jugador)
    {
        // jugador.inputManager.playerInputActions.Garage.Disable();
        jugador.AccesChangeInputMap(NameInputAction.Garage, false);
        
        //basicament, passem a nau o pilot.
        DoInteraction(estatAnterior,jugador);
        jugador.CloseGarage();
        
        InteractionType i= InteractionType.CloseGarage;
        jugador.SubInteraction(i);
    }
    

    

    public void Talk(Player jugador)
    {
        InteractionType i = InteractionType.Talk;

        //desactiva controls pilot
        //jugador.inputManager.playerInputActions.Pilot.Disable();

        
        print(jugador.branca);
        GameEventsManager.instance.dialogue_events.EnterDialogue(jugador.branca, jugador.mode);
        jugador.SubInteraction(i);
    }


}


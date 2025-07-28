using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;



//EXPLICACIÓ:
//crida de mètodes del jugador degut a certes coses, estar en una zona concreta, etc..
//zones que també es podrien crear aquí: obrir botiga, conversa,
//està separat per poder mantenir aquests "casos especials" fora de la logica del jugador i que estigui més ordenat
//es crea al script player i s'afegeix al mateix lloc que player

//es creen per dir quin mètode es crida, i està assignat a cada objecte que afegeix un mètode interactiu al jugador
public enum InteractionType
{
    NauPilot,
    PilotNau,
    PnINp,
    Talk
    OpenGarage

    
} 

public class InteractiveMethods : MonoBehaviour
{
    //public List<String> ListMethods = new List<String>();
   // public List<MethodPlayer> ListMethodDelegates = new List<MethodPlayer>();
    

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
            case InteractionType.PnINp:
                PnINp(player);
                break;

            case InteractionType.Talk:
                Talk(player);

            case InteractionType.OpenGarage:
                OpenGarage(player);

                break;
        }
    }
    
    
    //canvia entre el mode nau a pilot, desactivant els inputs necessaris
    public void Nau_Pilot(Player jugador)
    {
        jugador.playerInputActions.Nau.Disable();
        jugador.playerInputActions.Pilot.Enable();
        
        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques
        InteractionType i= InteractionType.NauPilot;
        jugador.SubInteraction(i);
    }
    
    //canvia de pilot a nau, desactivant els inputs necessaris 
    public void Pilot_Nau(Player jugador)
    {
        jugador.playerInputActions.Nau.Enable();
        jugador.playerInputActions.Pilot.Disable();
        
        //degut a que no sempre ontrigger exit s'activa, i la majoria de casos, només volen cridar-ho una vegada, 
        //quan s'activa la interacció l'eliminem 
        //per si les mosques
        InteractionType i= InteractionType.PilotNau;
        jugador.SubInteraction(i);
    }

    //canvia de pilot a nau i viceversa segons quin mode estem
    public void PnINp(Player jugador)
    {
        //Debug.Log("enter pninp");
        if (jugador.playerInputActions.Pilot.enabled)
        {
            Pilot_Nau(jugador);
        }else if (jugador.playerInputActions.Nau.enabled)
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
        string estatAbans;
        
        if (jugador.playerInputActions.Pilot.enabled)
        {
            estatAbans = "pilot";
        }else if (jugador.playerInputActions.Nau.enabled)
        {
            estatAbans = "nau";
        }
        jugador.playerInputActions.Garage.Enable();
         
    }
    

    

    public void Talk(Player jugador)
    {
        InteractionType i = InteractionType.Talk;

        print(jugador.branca);
        GameEventsManager.instance.dialogue_events.EnterDialogue(jugador.branca, jugador.mode);
        jugador.SubInteraction(i);
    }


}


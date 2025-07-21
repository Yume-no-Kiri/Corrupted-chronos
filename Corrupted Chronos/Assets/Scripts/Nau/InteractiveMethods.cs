using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;



//EXPLICACIÓ:
//crida de mètodes del jugador degut a certes coses, estar en una zona concreta, etc..
// zones que també es podrien crear aquí: obrir botiga, menu?


public enum InteractionType
{
    NauPilot,
    PilotNau,
    PnINp,
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

    public void DoInteractions(List<InteractionType> interactions, Player player)
    {
        Debug.Log("do itneractions %i:"+interactions.Count);

        //revisar funcionament, un cop surtis d'un dels menus hauries d'esperar a que aquest acabi per poder invocar el següent
        for (int i = 0; i < interactions.Count; i++)
        {
            DoInteraction(interactions[i], player);
        }
      
    }

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
            case InteractionType.OpenGarage:
                OpenGarage(player);
                break;
        }
    }
    
    
    public void Nau_Pilot(Player jugador)
    {
        jugador.playerInputActions.Nau.Disable();
        jugador.playerInputActions.Pilot.Enable();
        
        //no hi ha problema amb això, perque subInteraction mira si existeix
        InteractionType i= InteractionType.NauPilot;
        jugador.SubInteraction(i);
    }
    
    public void Pilot_Nau(Player jugador)
    {
        jugador.playerInputActions.Nau.Enable();
        jugador.playerInputActions.Pilot.Disable();
        InteractionType i= InteractionType.PilotNau;
        jugador.SubInteraction(i);
    }

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
        InteractionType i= InteractionType.PnINp;
        jugador.SubInteraction(i);

    }

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
    
    
    
    
}


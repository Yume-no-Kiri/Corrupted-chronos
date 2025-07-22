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
    OpenGarage,
    CloseGarage
    
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
            case InteractionType.CloseGarage:
                CloseGarage(player);
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
        
        if (jugador.playerInputActions.Pilot.enabled)
        {
            estatAnterior = InteractionType.NauPilot;
            jugador.playerInputActions.Pilot.Disable();

            
        }else if (jugador.playerInputActions.Nau.enabled)
        {
            estatAnterior = InteractionType.PilotNau;
            jugador.playerInputActions.Nau.Disable();

        }
        jugador.playerInputActions.Garage.Enable();
        
        InteractionType i= InteractionType.OpenGarage;
        jugador.SubInteraction(i);
        
        //sempre que s'obri el garatge s'afegeix per poder tancar-no, no trobo no tindria sentit fer-ho així
        //pero si es vulgues fer, seria afegir un if aquí sota
        i= InteractionType.CloseGarage;
        jugador.AddInteraction(i);
    }
    private void CloseGarage(Player jugador)
    {
        jugador.playerInputActions.Garage.Disable();
        
        //basicament, passem a nau o pilot.
        DoInteraction(estatAnterior,jugador);
        jugador.CloseGarage();
        
        InteractionType i= InteractionType.CloseGarage;
        jugador.SubInteraction(i);
    }

    
    
    
}


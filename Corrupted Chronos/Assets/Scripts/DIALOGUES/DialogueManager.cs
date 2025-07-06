using System;
using UnityEngine;
using Ink.Runtime;
public class DialogueManager : MonoBehaviour
{
    [Header("Ink file")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    private bool dialogue_playing = false;
    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        story = new Story(inkJson.text);
        playerInputActions = new PlayerInputActions();
        playerInputActions.UI.submit.performed -= ctx => ContinueOrExitStory(); //REVISAR
    }
  
    private void OnEnable()
    {
        GameEventsManager.instance.dialogue_events.onEnterDialogue += EnterDialogue;
        playerInputActions.UI.Enable();
    }

    // Update is called once per frame
    private void onDisable()
    {
        GameEventsManager.instance.dialogue_events.onEnterDialogue -= EnterDialogue;
        playerInputActions.UI.Disable();
      

    }

    private void EnterDialogue(string branca, int mode)
    {
        if (dialogue_playing)
        {
            return;
        }

        dialogue_playing=true;


        GameEventsManager.instance.dialogue_events.DialogueStarted();


        //saltar a on toca
        if (!branca.Equals("")) 
        {
            story.ChoosePathString(branca);
        }
        else
        {

        }

        //mirar story
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if (story.canContinue)  //Haurem de gestionar els tags de darrere la frase # npc:XXXXXXXX # emocio:XXXXXX, s'haurà de crear una funció que ho gestioni
        {
            string dialogue_line = story.Continue();

            //de momemnt imprimim en consola 
            // OBVIAMENT CANVIAR A PASSAR PER LA UI
            GameEventsManager.instance.dialogue_events.DisplayDialogue(dialogue_line);
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        Debug.Log("Sortint diàleg");
        GameEventsManager.instance.dialogue_events.DialogueFinished();

        dialogue_playing = false;
        //reset story
        story.ResetState();
        onDisable();
    }

}

using System;
using UnityEngine;
using Ink.Runtime;
public class DialogueManager : MonoBehaviour
{
    [Header("Ink file")]
    [SerializeField] private TextAsset inkJson;

    private Story story;

    private bool dialogue_playing = false;

    private void Awake()
    {
        story = new Story(inkJson.text);
    }
    public void Update()  //S'HA DE FER QUE FUNCIONI BÉ
    {
        if (dialogue_playing)
        {
            if (Input.GetButton("space")) ContinueOrExitStory();
        }
    }

    private void OnEnable()
    {
        GameEventsManager.instance.dialogue_events.onEnterDialogue += EnterDialogue;
    }

    // Update is called once per frame
    private void onDisable()
    {
        GameEventsManager.instance.dialogue_events.onEnterDialogue -= EnterDialogue;

    }






    private void EnterDialogue(string npc, string emotion, int mode)
    {
        if (dialogue_playing)
        {
            return;
        }

        dialogue_playing=true;

        //saltar a on toca
        if (!npc.Equals("")) 
        {
            story.ChoosePathString(npc);
        }
        else
        {

        }

        //mirar story
        ContinueOrExitStory();
    }

    private void ContinueOrExitStory()
    {
        if (story.canContinue)
        {
            string dialogue_line = story.Continue();

            //de momemnt imprimim en consola 
            // OBVIAMENT CANVIAR A PASSAR PER LA UI
            Debug.Log(dialogue_line);
        }
        else
        {
            ExitDialogue();
        }
    }

    private void ExitDialogue()
    {
        Debug.Log("Sortint diàleg");

        dialogue_playing=false;
        //reset story
        story.ResetState();
    }

}

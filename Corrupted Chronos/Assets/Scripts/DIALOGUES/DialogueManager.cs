using System;
using UnityEngine;
using Ink.Runtime;
using Ink.Parsed;
public class DialogueManager : MonoBehaviour
{
    [Header("Ink file")]
    [SerializeField] private TextAsset inkJson;

    public BossSpawner boss_spawner;
    private Ink.Runtime.Story story;
    private int current_choice = -1;

    private bool dialogue_playing = false;
    private PlayerInputActions playerInputActions;
    bool shouldChangeScene = false;
    string pendingSceneName;
    string image;
    public int pillars=0;

    private void Awake()
    {
        story = new Ink.Runtime.Story(inkJson.text);
        playerInputActions = new PlayerInputActions();
        story.variablesState["pillars_broken"]=pillars;
        playerInputActions.UI.submit.performed -= ctx => ContinueOrExitStory(); //REVISAR
        playerInputActions.UI.submit.performed += ctx => ContinueOrExitStory(); //REVISAR
    }
    private void OnEnable()
    {
        GameEventsManager.instance.dialogue_events.onEnterDialogue += EnterDialogue;
        GameEventsManager.instance.dialogue_events.onUpdateChoice += UpdateChoiceIndex;
        playerInputActions.UI.Enable();
    }

    // Update is called once per frame
    private void onDisable()
    {
        GameEventsManager.instance.dialogue_events.onEnterDialogue -= EnterDialogue;
        GameEventsManager.instance.dialogue_events.onUpdateChoice -= UpdateChoiceIndex;

        playerInputActions.UI.Disable();
    }

    private void UpdateChoiceIndex(int choice_index)
    {
        current_choice = choice_index;
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
        if (story.currentChoices.Count > 0 && current_choice != -1)
        {
            story.ChooseChoiceIndex(current_choice);
            current_choice=-1;
        }

        if (story.canContinue)  //Haurem de gestionar els tags de darrere la frase # npc:XXXXXXXX # emocio:XXXXXX, s'haur� de crear una funci� que ho gestioni
        {
            string dialogue_line = story.Continue();
            CheckTags();
            //Enviar el text a la UI
            GameEventsManager.instance.dialogue_events.DisplayDialogue(dialogue_line, story.currentChoices,image);
        }
        else if (story.currentChoices.Count == 0)
        {
            if (shouldChangeScene)
            {
                ExitDialogue();
                ExecuteSceneChange();
            }
            else
            {
                ExitDialogue();
            }
        }
    }

    private void ExitDialogue()
    {
        if (!dialogue_playing) return;
        Debug.Log("Sortint di�leg");
        GameEventsManager.instance.dialogue_events.DialogueFinished();
        
        
        dialogue_playing = false;
        //reset story
        //story.ResetState();
        if(GameManager.Instance!=null)  GameManager.Instance.playerInstance.GetComponent<Player>().ChangeToTalk(false);

    }

    private void CheckTags()
    {
        foreach (string tag in story.currentTags)
        {
            string trimmedTag = tag.Trim();

            if (trimmedTag.StartsWith("changeScene:"))
            {
                shouldChangeScene = true;
                //Agafa la segona part del tag
                pendingSceneName = trimmedTag.Split(':')[1];
            }
            if (trimmedTag.StartsWith("scene:"))
            {
                image = trimmedTag.Split(':')[1];
            }
            if (trimmedTag.StartsWith("BOSS"))
            {
                pillars = 4;
                story.variablesState["pillars_broken"] = pillars;
                boss_spawner.spawnBoss();
            }
            // Pel futur: # npc:Fisherman, # emotion:Angry
        }
    }
    private void ExecuteSceneChange()
    {
        Debug.Log("CHANGING SCENE");
        shouldChangeScene = false; 
        UnityEngine.SceneManagement.SceneManager.LoadScene(pendingSceneName);
    }



    //Funcions per actualitzar variables de INK
    public void UpdatePillars()
    {
        pillars = pillars+1;
        story.variablesState["pillars_broken"] = pillars;
    }

}

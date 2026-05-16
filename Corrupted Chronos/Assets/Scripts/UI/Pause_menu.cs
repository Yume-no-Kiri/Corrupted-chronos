using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class pause_menu : MonoBehaviour
{

    public static bool GameIsPaused = false;
    [SerializeField] public Button config_button;
    [SerializeField] public Button continue_button;
    [SerializeField] public Button exit_button;
    [SerializeField] private GameObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        // inventory_button.onClick.AddListener(Inv);
        exit_button.onClick.AddListener(ExitGame);
        continue_button.onClick.AddListener(ContinueGame);
    }

    public void ContinueGame()
    {
        Resume();
    }
    /* private void Inv()
     {
         Resume();
         inventory_menu.Pause();
     }*/

    //private void EnterConfig()
    //{
    //    Resume();
    //    options.Pause();
    //}

    public void ExitGame()
    {
        SceneManager.LoadScene("Main_menu");
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
        GameIsPaused = false;

    }

    public void Pause()
    {;
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Resume();
        }
    }
}
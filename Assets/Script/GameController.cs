using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public int playerNumber=0;
    public CameraController cc;
    public GameObject pauseMenu;
    public GameObject ui;
    public GameObject j1;
    public GameObject j2;
    public Transform spawn;
    public int currentLevel;
    private bool active;
    void Start()
    {
        active = true;
        GetComponent<PlayerInputManager>().playerPrefab = j1;
        cc = FindAnyObjectByType<CameraController>();
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!active) return;
        if (EventSystem.current.currentSelectedGameObject == null && pauseMenu.activeSelf)
        {
            EventSystem.current.SetSelectedGameObject(ui);
        }
    }

    public void pause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(ui);
    }

    public void closePause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);

    }


    public void endGame()
    {
        playerNumber = 0;
        active = false;
        DontDestroyOnLoad(gameObject);
        StartCoroutine(RestartCoroutine());

    }
    public void OnPlayerJoined()
    {
        GetComponent<PlayerInputManager>().playerPrefab = j2;
    }


    System.Collections.IEnumerator RestartCoroutine()
    {

        gameObject.name = "abouttodestroy";
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        yield return null;

        cc = FindAnyObjectByType<CameraController>();

        while (cc == null)
        {
            yield return null;
            cc = FindAnyObjectByType<CameraController>();
        }

        while (cc.players.Count == 0)
        {
            yield return null;
        }

        if (currentLevel == 2)
        {
            cc.players[1].transform.position =
                GameObject.Find("Fin niveau 1").transform.position;
        }
        else if (currentLevel == 3)
        {
            cc.players[1].transform.position =
                GameObject.Find("Fin niveau 2").transform.position;
        }

        Destroy(gameObject);
    }

}

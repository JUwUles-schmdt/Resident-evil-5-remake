using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{

    public int playerNumber=0;
    public CameraController cc;
    public GameObject pauseMenu;
    public GameObject ui;
    public GameObject j1;
    public GameObject j2;
    public Transform spawn;
    void Start()
    {
        GetComponent<PlayerInputManager>().playerPrefab = j1;
        cc = FindAnyObjectByType<CameraController>();
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
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

    public void OnPlayerJoined()
    {
        GetComponent<PlayerInputManager>().playerPrefab = j2;
    }

}

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] Vector2Int aspectRatio = new(16, 9);
    public bool fullScreen = true;
    public GameObject levels;


    public GameObject firstSelected;
    public GameObject firstlevel;

    void Start()
    {
        Vector2Int size = default;
        float currentRatio = (float)Screen.width / Screen.height;
        float ratioGoal = (float)aspectRatio.x / aspectRatio.y;
        if (currentRatio > ratioGoal)
        {
            size.y = Screen.height;
            size.x = (int)(Screen.height * ratioGoal);
        }
        else
        {
            size.y = (int)(Screen.width / ratioGoal);
            size.x = Screen.width;
        }
        Screen.SetResolution(size.x, size.y, fullScreen);
        EventSystem.current.SetSelectedGameObject(firstSelected);
        levels.SetActive(false);
    }
    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == null)
        {
            if (!levels.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(firstlevel);
            }
        }
        if (Gamepad.current.buttonEast.wasPressedThisFrame)
        {
        closeLevels();
        }
    }


    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void openLevels()
    {
        levels.SetActive(true);
        EventSystem.current.SetSelectedGameObject(firstlevel);
    }

    public void closeLevels()
    {
        levels.SetActive(false);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }
}

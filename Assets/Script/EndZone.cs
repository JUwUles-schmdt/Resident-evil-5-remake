using UnityEngine;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public CameraController cc;
    public int numberofplayer;
    public string scene;

    void Update()
    {
        if (cc.players.Count > 0 && numberofplayer >= cc.players.Count)
        {
            changeScene(scene);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            numberofplayer++;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            numberofplayer--;
    }

    private void changeScene(string sc)
    {
        for (int i = 0; i < cc.players.Count; i++) 
        {
            cc.players[i].gameObject.GetComponent<PlayerController>().stock();
        }
        SceneManager.LoadScene(sc);
    }
}

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public CameraController cc;
    public int numberofplayer;
    public static int level=1;
    public Transform nextLevel;

    private void Start()
    {
        level = 1;
    }

    void Update()
    {
        if (cc.players.Count > 0 && numberofplayer >= cc.players.Count)
        {
            level += 1;   
            if (cc.players.Count > 0 && cc.players[0] != null)
            {
                cc.players[0].transform.position = nextLevel.position;

                if (level == 2)
                    cc.players[0].GetComponent<PlayerController>().toggleLight(true);
                else
                    cc.players[0].GetComponent<PlayerController>().toggleLight(false);
            }

            if (cc.players.Count > 1 && cc.players[1] != null)
            {
                cc.players[1].transform.position = nextLevel.position;

                if (level == 2)
                    cc.players[1].GetComponent<PlayerController>().toggleLight(true);
                else
                    cc.players[1].GetComponent<PlayerController>().toggleLight(false);
            }
            cc.transform.position = nextLevel.position;
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
}

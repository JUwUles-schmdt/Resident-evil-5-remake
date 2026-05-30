using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class EndZone : MonoBehaviour
{
    public CameraController cc;
    public int numberofplayer;
    public int nextLevelNumber;
    public Transform nextLevel;

    private void Start()
    {
    }

    void Update()
    {
        if (cc.players.Count > 0 && numberofplayer >= cc.players.Count)
        {
            FindObjectOfType<GameController>().currentLevel += 1;
            if (cc.players.Count > 0 && cc.players[0] != null)
            {
                cc.players[0].transform.position = nextLevel.position;

                if (nextLevelNumber == 2)
                    cc.players[0].GetComponent<PlayerController>().toggleLight(true);
                else
                    cc.players[0].GetComponent<PlayerController>().toggleLight(false);
            }

            if (cc.players.Count > 1 && cc.players[1] != null)
            {
                cc.players[1].transform.position = nextLevel.position;

                if (nextLevelNumber == 2)
                    cc.players[1].GetComponent<PlayerController>().toggleLight(true);
                else
                    cc.players[1].GetComponent<PlayerController>().toggleLight(false);
            }
            cc.transform.position = nextLevel.position;


            if (nextLevelNumber == 3)
            {
                GameObject.Find("Water").GetComponent<WaveManager>().isActive=true;
            }
            if (nextLevelNumber == 3)
            {
                GameObject.Find("Water").GetComponent<WaveManager>().isActive = false;
                foreach (Transform child in GameObject.Find("Water").transform)
                {
                    Destroy(child.gameObject);
                }
            }
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

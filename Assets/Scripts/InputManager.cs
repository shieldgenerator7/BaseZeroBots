using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public BotEditPanel botEditPanel;
    public GameObject botCursor;

    private BotController mostRecentBot = null;
    public BotController CurrentBot
    {
        get { return mostRecentBot; }
        set
        {
            mostRecentBot = value;
            botCursor.transform.position = mostRecentBot.transform.position;
        }
    }

    private void Start()
    {
        FindObjectOfType<LevelManager>().onSceneLoaded += sceneLoaded;
    }

    private void sceneLoaded()
    {
        CurrentBot = FindObjectOfType<BotController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.x = Mathf.Round(mousePos.x);
            mousePos.y = Mathf.Round(mousePos.y);
            if (botEditPanel.Open)
            {
                if (botEditPanel.processClick(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    return;
                }
            }
            botEditPanel.changeTarget(null);
            foreach (BotController bc in FindObjectsOfType<BotController>())
            {
                if ((Vector2)bc.transform.position == mousePos)
                {
                    CurrentBot = bc;
                    botEditPanel.changeTarget(bc);
                    break;
                }
            }
        }
        //Escape the instruction panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            botEditPanel.changeTarget(null);
        }
        //Open instruction panel of most recent bot
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!botEditPanel.Open)
            {
                botEditPanel.changeTarget(CurrentBot);
            }
            else
            {
                botEditPanel.changeTarget(null);
            }
        }
        if (!botEditPanel.Open)
        {
            //Select other bot
            bool up = Input.GetKeyDown(KeyCode.UpArrow);
            bool down = Input.GetKeyDown(KeyCode.DownArrow);
            bool left = Input.GetKeyDown(KeyCode.LeftArrow);
            bool right = Input.GetKeyDown(KeyCode.RightArrow);
            if (up || down || left || right)
            {
                int direction = (up || right) ? 1 : -1;
                List<BotController> bots = new List<BotController>(FindObjectsOfType<BotController>());
                int index = bots.IndexOf(CurrentBot);
                index += direction;
                index = (int)Mathf.Repeat(index, bots.Count);
                CurrentBot = bots[index];
            }

            //Reset Game
            if (Input.GetKeyDown(KeyCode.R))
            {
                LevelManager lm = FindObjectOfType<LevelManager>();
                lm.LoadLevel(lm.LevelIndex);
            }

            //Next Level
            if (Input.GetKeyDown(KeyCode.RightBracket))
            {
                LevelManager lm = FindObjectOfType<LevelManager>();
                lm.LevelIndex++;
            }

            //Previous Level
            if (Input.GetKeyDown(KeyCode.LeftBracket))
            {
                LevelManager lm = FindObjectOfType<LevelManager>();
                lm.LevelIndex--;
            }

            //Leave Game
            if (Input.GetKeyDown(KeyCode.W)
                && Input.GetKey(KeyCode.LeftControl))
            {
                Application.Quit();
            }
        }
    }

    private void LateUpdate()
    {
        if (mostRecentBot)
        {
            botCursor.SetActive(true);
            botCursor.transform.position = mostRecentBot.transform.position;
        }
        else
        {
            botCursor.SetActive(false);
        }
    }
}

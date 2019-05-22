using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InstructionPanel instructionPanel;
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
            if (instructionPanel.target != null)
            {
                if (instructionPanel.processClick(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    return;
                }
            }
            instructionPanel.changeTarget(null);
            foreach (BotController bc in FindObjectsOfType<BotController>())
            {
                if ((Vector2)bc.transform.position == mousePos)
                {
                    CurrentBot = bc;
                    instructionPanel.changeTarget(bc);
                    break;
                }
            }
        }
        //Escape the instruction panel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            instructionPanel.changeTarget(null);
        }
        //Open instruction panel of most recent bot
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (instructionPanel.target == null)
            {
                instructionPanel.changeTarget(CurrentBot);
            }
            else
            {
                instructionPanel.changeTarget(null);
            }
        }
        //Select other bot
        if (instructionPanel.target == null)
        {
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InstructionPanel instructionPanel;

    private BotController mostRecentBot = null;

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
                    mostRecentBot = bc;
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
                instructionPanel.changeTarget(mostRecentBot);
            }
            else
            {
                instructionPanel.changeTarget(null);
            }
        }
        }
}

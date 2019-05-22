using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public InstructionPanel instructionPanel;

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
                    instructionPanel.changeTarget(bc);
                    break;
                }
            }
            FindObjectOfType<TurnManager>().Paused = instructionPanel.target != null;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotEditPanel : MonoBehaviour
{
    private BotController target;
    public List<InstructionPanel> panels;

    public bool Open
    {
        get { return target != null; }
    }

    private int currentPanelIndex = 0;
    public int PanelIndex
    {
        get { return currentPanelIndex; }
        set
        {
            panels[currentPanelIndex].changeTarget(null);
            currentPanelIndex = (int)Mathf.Repeat(value, panels.Count);
            panels[currentPanelIndex].changeTarget(target);
        }
    }
    public InstructionPanel Panel
    {
        get { return panels[currentPanelIndex]; }
        set { PanelIndex = panels.IndexOf(value); }
    }
    
    // Update is called once per frame
    void Update()
    {
        //Check for changing tabs
        if (Input.GetKeyDown(KeyCode.Tab)
            && Input.GetKey(KeyCode.LeftControl))
        {
            PanelIndex++;
        }
    }

    public void changeTarget(BotController bc)
    {
        target = bc;
        FindObjectOfType<TurnManager>().Paused = target != null;
        if (target == null)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
            PanelIndex = 0;
            Panel.changeTarget(target);
        }
    }

    public bool processClick(Vector2 mousePos)
    {
        return Panel.processClick(mousePos);
    }
}

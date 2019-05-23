using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessContext
{
    public BotController botController;
    private int currentIndex;
    public int Index
    {
        get { return currentIndex; }
    }
    public ProcessContext caller;

    public ProcessContext(BotController botController, int currentIndex)
    {
        this.botController = botController;
        this.currentIndex = currentIndex;
        this.caller = null;
    }

    public ProcessContext(ProcessContext processContext, int currentIndex)
        : this(processContext.botController, currentIndex)
    {
        this.caller = processContext;
    }

    public Instruction instruction(int index = -1)
    {
        if (index == -1)
        {
            index = currentIndex;
        }
        if (index < 0 || index >= botController.instructions.Count)
        {
            throw new System.ArgumentOutOfRangeException(
                "Index (" + index + ") is out of bounds! instructions.count: "
                + botController.instructions.Count);
        }
        return botController.instructions[index];
    }

    public int next(int startIndex = -1, bool loop = true)
    {
        if (startIndex < 0)
        {
            startIndex = currentIndex;
        }
        int index = startIndex;
        do
        {
            index++;
            if (loop)
            {
                index = (int)Mathf.Repeat(
                    index,
                    botController.instructions.Count
                    );
            }
        }
        //keep searching until you find one that's not null
        while (
            index >= 0 && index < botController.instructions.Count
            && botController.instructions[index] == null
            && index != startIndex
        );
        return index;
    }

    public int prev(int startIndex = -1, bool loop = true)
    {
        if (startIndex < 0)
        {
            startIndex = currentIndex;
        }
        int index = startIndex;
        do
        {
            index--;
            if (loop)
            {
                index = (int)Mathf.Repeat(
                    index,
                    botController.instructions.Count
                    );
            }
        }
        //keep searching until you find one that's not null
        while (
            index >= 0 && index < botController.instructions.Count
            && botController.instructions[index] == null
            && index != startIndex
        );
        return index;
    }

    public int Count
    {
        get { return botController.instructions.Count; }
    }

    public ProcessContext context(int index)
    {
        return new ProcessContext(this, index);
    }

    public int getLastParameterIndex(int paramIndex)
    {
        return instruction(paramIndex).getLastParameterIndex(context(paramIndex));
    }
}

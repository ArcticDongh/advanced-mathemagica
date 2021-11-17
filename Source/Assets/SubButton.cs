using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubButton : UnityEngine.UI.Button
{
    int numMark;
    public delegate void Action(int numMark);
    public event Action EventClick;

    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        if (state == SelectionState.Pressed)
            EventClick?.Invoke(numMark);
    }

    public void MarkAndBind(int numMark, Action action)
    {
        this.numMark = numMark;
        EventClick += action;
    }
}

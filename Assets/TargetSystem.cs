using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public abstract class Target
{
    public delegate void Action();
    public event Action EventTargetComplete;
    TargetData targetData;
    public TargetData TargetData { set { targetData = value; } get { return targetData; } }
    UnityEngine.UI.Text text;
    public UnityEngine.UI.Text Text { get { return text; } set { text = value; } }
    public bool IsCompleted { get { return targetData.IsCompleted; }set { bool t = targetData.IsCompleted; targetData.IsCompleted = value; if (value && !t) EventTargetComplete?.Invoke(); } }
    public abstract void Update();
    public virtual void UpdateText()
    {
        text.text = targetData.description + (targetData.IsCompleted ? "（完成）" : "");
    }
}

[System.Serializable]
public class TestTarget : Target
{
    public TestTarget()
    {
        EventTargetComplete += base.UpdateText;
    }
    public override void Update()
    {
        IsCompleted = true;
        //Debug.Log("targetUpdated");
    }
}
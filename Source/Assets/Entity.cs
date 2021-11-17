using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Flag { test = 0, destroy, protect}
    //public delegate void Check(Entity entity);
    public delegate void Check();
    public event Check EventOnClick;
    public event Check EventOnDestroy;
    public List<Flag> flags;

    public bool HaveFlagInFlags(List<Flag> flags)
    {
        if(this.flags != null && flags != null)
        {
            foreach(var flag in this.flags)
            {
                if (flags.Contains(flag))
                    return true;
            }
        }
        return false;
    }

    public virtual void OnClick()
    {
        EventOnClick?.Invoke();
    }

    private void OnDestroy()
    {
        EventOnDestroy?.Invoke();
    }
}

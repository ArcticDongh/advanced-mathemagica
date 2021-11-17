using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TargetData : ScriptableObject
{
    public enum TargetType { test = 0, destroy }
    public TargetType type;
    public List<Entity.Flag> targetFlags;
    public string description;

    [SerializeField, ReadOnly]
    bool isCompleted;
    public bool IsCompleted { get; set; }
}
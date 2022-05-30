using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UpgradeModuleBehaviour : ScriptableObject
{
    public abstract void Apply(int _newStage);
}


using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:Component
{
    public static T instance;
    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
        }
            
        else
            DestroyImmediate(instance);
    }
}

public class StageData
{
    public bool isLocked;
    public int index,rate;

    public StageData(int index, bool isLocked, int rate)
    {
        this.index = index;
        this.isLocked= isLocked;
        this.rate = rate;
    }
}


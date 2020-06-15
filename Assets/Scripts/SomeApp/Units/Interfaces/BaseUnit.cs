using System.Collections;
using UnityEngine;

public abstract class BaseUnit : MonoBehaviour, IEnumerable
{
    public static BaseUnit FirstCreated { get; protected set; }
    public static BaseUnit LastCreated { get; protected set; }

    public BaseUnit NextUnit { get; protected set; }
    public BaseUnit PrevUnit { get; protected set; }

    public IEnumerator GetEnumerator()
    {
        return new UnitEnumerator();
    }

    protected void InitUnit()
    {
        if (FirstCreated == null)
            FirstCreated = this;

        if (LastCreated != null)
        {
            LastCreated.NextUnit = this;
            PrevUnit = LastCreated;
        }

        LastCreated = this;
    }

    protected void UnInitUnit()
    {
        if (PrevUnit != null)
            PrevUnit.NextUnit = NextUnit;

        if (NextUnit != null)
            NextUnit.PrevUnit = PrevUnit;
    }

    protected virtual void Awake()
    {
        InitUnit();
    }

    protected virtual void OnDestroy()
    {
        UnInitUnit();
    }
}

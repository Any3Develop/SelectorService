using System.Collections;

public class UnitEnumerator : IEnumerator
{
    private BaseUnit _currentObj;

    public object Current
    {
        get { return _currentObj; }
    }

    public bool MoveNext()
    {
        _currentObj = (_currentObj == null) ? BaseUnit.FirstCreated : _currentObj.NextUnit;

        return _currentObj != null;
    }

    public void Reset()
    {
        _currentObj = null;
    }
}
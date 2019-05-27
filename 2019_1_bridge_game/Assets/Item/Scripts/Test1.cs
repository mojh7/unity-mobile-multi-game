using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Test1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public interface ITest1
{
    void F1(C1 c);
    void F2(C1 c);
}

public class C1
{
    private ITest1 state;
    private ITest1 state2;

    public C1()
    {
        state = new D1();
}

    public void F1()
    {
        state.F1(this);
    }

    public void F2()
    {
        state.F2(this);
    }

    public void F3()
    {
        state2.F1(this);
    }
}

public class D1 : MonoBehaviour, ITest1
{
    public void F1(C1 c)
    {

    }
    public void F2(C1 c)
    {

    }
}
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
        state2 = new D2();
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

public class D2 : UBZ.Owner.Character, ITest1
{
    public void F1(C1 c)
    {
        throw new System.NotImplementedException();
    }

    public void F2(C1 c)
    {
        throw new System.NotImplementedException();
    }

    protected override void AddRetrictsBehaviorCount()
    {
        throw new System.NotImplementedException();
    }

    protected override void AddRetrictsMovingCount()
    {
        throw new System.NotImplementedException();
    }

    protected override bool IsControlTypeAbnormal()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator StunCoroutine(float effectiveTime)
    {
        throw new System.NotImplementedException();
    }

    protected override void SubRetrictsBehaviorCount()
    {
        throw new System.NotImplementedException();
    }

    protected override void SubRetrictsMovingCount()
    {
        throw new System.NotImplementedException();
    }
}
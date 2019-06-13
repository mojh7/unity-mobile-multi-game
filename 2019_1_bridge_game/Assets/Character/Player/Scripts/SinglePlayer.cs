using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UBZ.Owner
{
    // TODO : 멀티 인 게임 외의 only 싱글 로직인 씬에서 사용 할 Player Class
    public class SinglePlayer : Character
    {
        protected override void AddRetrictsBehaviorCount()
        {
            throw new System.NotImplementedException();
        }

        protected override void AddRetrictsMovingCount()
        {
            throw new System.NotImplementedException();
        }

        protected override bool IsControlTypeAbnormal(ControlTypeAbnormalStatus abnormalStatusType)
        {
            throw new System.NotImplementedException();
        }

        protected override IEnumerator SlidingCoroutine()
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
}


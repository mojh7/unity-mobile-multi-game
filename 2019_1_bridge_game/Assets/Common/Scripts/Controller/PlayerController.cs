using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController
{
    #region components
    private MovingJoystick movingJoyStick;
    #endregion

    public PlayerController(MovingJoystick movingJoyStick)
    {
        this.movingJoyStick = movingJoyStick;
    }

    #region move
    /// <summary>
    /// 조이스틱이 현재 바라보는 방향의 벡터  
    /// </summary> 
    public Vector3 GetMovingInputVector()
    {
        float h = movingJoyStick.GetHorizontalValue();
        float v = movingJoyStick.GetVerticalValue();

        return new Vector3(h, v, 0).normalized;
    }

    public float GetMovingInputDegree()
    {
        return new Vector3(movingJoyStick.GetHorizontalValue(), movingJoyStick.GetVerticalValue(), 0).GetDegFromVector();
    }
    /// <summary>
    /// 입력한 조이스틱의 가장 최근 Input vector의 normal vector 반환 
    /// </summary>
    public Vector3 GetMoveRecentNormalInputVector()
    {
        return movingJoyStick.GetRecentNormalInputVector();
    }
    #endregion
}

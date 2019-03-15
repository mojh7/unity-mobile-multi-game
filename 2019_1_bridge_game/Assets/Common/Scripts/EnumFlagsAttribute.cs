using UnityEngine;

/* 예시
 * 다중 enum inspector 표시 해줌.
 * 
 * enum 작성할 때
 * public enum TestEnum
   {
       A = 0x00000001,
       B = 0x00000002,
       C = 0x00000004
   }

    인스펙터 표기상 nothing = 0, everything = -1로 알고 있어서 이 숫자는 비워놓고 각 항목들 2의 배수로 작성 해야됨.

    사용 하는 곳
    [EnumFlags]
    public TestEnum te;
 * 
 */

public class EnumFlagsAttribute : PropertyAttribute
{
    public EnumFlagsAttribute() { }
}
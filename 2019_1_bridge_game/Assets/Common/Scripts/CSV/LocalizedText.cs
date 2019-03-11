using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class LocalizedText : MonoBehaviour
{
    // Manager 이름에 맞게 수정후 사용
    //public string key;

    // 사용법 :
    // LocalizeUtil Dic에 생성된 value 값 이용.

	//void Start ()
 //   {
 //       if (!GameManager.instance.uiManager.GetLocalize(key))
 //       {
 //           Debug.Log("Error : localize key is null " + key); return;
 //       }

 //       Text text = GetComponent<Text>();
 //       text.text = GameManager.instance.uiManager.GetLocalizeText(key);
 //   }
}
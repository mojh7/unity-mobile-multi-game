using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO : 임시로 만든 화면 터치 이펙트 클래스, 추후 수정 해야됨.

public class ScreenTouchEffect : MonoBehaviour
{
    // 오브젝트 풀 만들어서 꺼내 쓰게끔 해야 되는데 프로토타입 용으로 임시로 바로 instantiate로 복사해서 사용
    [SerializeField] private GameObject[] touchEffectList; 

    private int pianoIndex = 0;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 touchPos = new Vector2(wp.x, wp.y);

            transform.position = touchPos;

            // 생성 
            GameObject touchEffect = Instantiate(touchEffectList[Random.Range(0, touchEffectList.Length)], transform.position, Quaternion.identity) as GameObject;
            AudioManager.Instance.PlaySound(pianoIndex, SFXType.PIANO);
            pianoIndex = (pianoIndex + 1) % 7;
            //pianoIndex = Random.Range(0, 7);
            // 파괴 
            Destroy(touchEffect.gameObject, 1f);
        }
    }
}

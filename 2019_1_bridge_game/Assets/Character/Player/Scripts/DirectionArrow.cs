using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private GameObject sprite;

    private Transform baseTown;
    private float speed = 3.0f;
    private bool canFollow = false;
    private bool isInBase = true;

    public void RemoveDirectionArrow()
    {
        canFollow = false;
        arrow.gameObject.SetActive(false);
    }

    public void SetBaseTown(Transform baseTf)
    {
        Debug.Log(baseTf);
        canFollow = true;
        this.baseTown = baseTf;
    }

    // TODO : tag 체크 비싼 걸로 알아서 layer 체크로 대체
    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (!canFollow)
            return;
        if (coll.transform.CompareTag("BaseZone"))
        {
            isInBase = true;
            sprite.SetActive(false);
            StopCoroutine(DirectionFollowBaseTown());
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (!canFollow)
            return;
        if (coll.transform.CompareTag("BaseZone"))
        {
            isInBase = false;
            sprite.SetActive(true);
            StartCoroutine(DirectionFollowBaseTown());
        }
    }

    private IEnumerator DirectionFollowBaseTown()
    {
        while (true)
        {
            if (isInBase) yield break;

            Vector3 vectorToTarget = baseTown.position - arrow.position;
            vectorToTarget.Normalize();

            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;

            arrow.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

            yield return null;
        }
    }
}

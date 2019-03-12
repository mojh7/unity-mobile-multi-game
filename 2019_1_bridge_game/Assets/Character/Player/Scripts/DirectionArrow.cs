using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionArrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    [SerializeField] private GameObject sprite;

    private Transform baseTown;
    private float speed = 3.0f;
    private bool isInBase = true;

    public void SetBaseTown(Transform baseTf)
    {
        this.baseTown = baseTf;
    }

    public void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.name.Equals("임시 - Base Town"))
        {
            isInBase = true;
            sprite.SetActive(false);
            StopCoroutine(DirectionFollowBaseTown());
        }
    }

    public void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.name.Equals("임시 - Base Town"))
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

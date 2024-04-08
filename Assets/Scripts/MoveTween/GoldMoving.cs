using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMoving : MonoBehaviour
{
    public bool chest;
    public Transform targetObject;
    public float moveDuration = 2.0f;
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo;
    public int gold;
    public int YoffSet;

    public float arcHeight = 5.0f;
    public float bulletSpeed = 10.0f;

    private Vector3 startPosition;
    private Vector3 endPosition;
    private float totalTime;
    private bool can;

    private void Start()
    {
        if(!chest)
        {
            targetObject = GameManager.Instance.goldPos;
        }
        else
        {
            targetObject = GameManager.Instance.chestPos;
        }
        startPosition = transform.position;
        int i = Random.Range(0, 2);
        if (i == 0)
        {
            endPosition = transform.position + new Vector3(Random.Range(-1f, -0.4f), YoffSet, 0f);
        }
        else if (i == 1)
        {
            endPosition = transform.position + new Vector3(Random.Range(0.4f, 1f), YoffSet, 0f);
        }

        float distance = Vector3.Distance(startPosition, endPosition);
        float peakHeight = arcHeight + Mathf.Max(0, distance / 2f);
        float gravity = Mathf.Abs(Physics.gravity.y);

        totalTime = Mathf.Sqrt(2f * peakHeight / gravity) + Mathf.Sqrt(2f * (peakHeight - Mathf.Abs(endPosition.y - startPosition.y)) / gravity);
        StartCoroutine(MoveAlongArc());
    }

    private IEnumerator MoveAlongArc()
    {
        float elapsedTime = 0f;

        while (elapsedTime < totalTime)
        {
            float t = elapsedTime / totalTime;
            Vector3 parabolicPosition = Vector3.Lerp(startPosition, endPosition, t) + Vector3.up * Mathf.Sin(Mathf.PI * t) * arcHeight;

            transform.position = parabolicPosition;

            elapsedTime += Time.unscaledDeltaTime * bulletSpeed;
            yield return null;
        }

        can = true;
    }

    private void Update()
    {
        if (targetObject != null && can)
        {
            MoveToTarget();
        }
    }

    private void MoveToTarget()
    {
        Hashtable moveParams = new Hashtable();
        moveParams.Add("position", targetObject.position);
        moveParams.Add("time", moveDuration);
        moveParams.Add("easetype", easeType);
        moveParams.Add("oncomplete", "OnMoveComplete");
        moveParams.Add("oncompletetarget", gameObject);
        moveParams.Add("ignoretimescale", true);

        iTween.MoveTo(gameObject, moveParams);
    }

    private void OnMoveComplete()
    {
        Debug.Log("Gold reached target!");
        if(!chest)
        {
            GameManager.Instance.AddMoney(gold);
        }
        else
        {
            GameManager.Instance.ChestClaim();
        }
        Destroy(gameObject);
    }
}
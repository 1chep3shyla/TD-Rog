using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldMoving : MonoBehaviour
{
    public Transform targetObject; // ������� ������, � �������� ����� �������������
    public float moveDuration = 2.0f; // ������������ �����������
    public iTween.EaseType easeType = iTween.EaseType.easeOutExpo; // ��� ������
    public int gold;
    public int YoffSet;

    public float arcHeight = 5.0f;
    public float bulletSpeed = 10.0f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float horizontalDistance;
    private float totalTime;

    private bool isMoving = false; // ����, �����������, ��������� �� ������
    private bool can;


    private void Update()
    {
        if (targetObject != null && can)
        {
            MoveToTarget();
        }

    }

    private void MoveToTarget()
    {
        isMoving = true;

        // ���������� ��������� ��� ����������� iTween
        Hashtable moveParams = new Hashtable();
        moveParams.Add("position", targetObject.position);
        moveParams.Add("time", moveDuration);
        moveParams.Add("easetype", easeType);
        moveParams.Add("oncomplete", "OnMoveComplete");
        moveParams.Add("oncompletetarget", gameObject);

        // ��������� iTween
        iTween.MoveTo(gameObject, moveParams);
    }

    private void OnMoveComplete()
    {
        isMoving = false;
        Debug.Log("������ � ������� �������!");
        GameManager.Instance.AddMoney(gold);
        Destroy(gameObject);
    }//

    private void Start()
    {
        targetObject = GameManager.Instance.goldPos;
        startPosition = transform.position;
        int i = Random.Range(0,2);
        if ( i == 0)
        {
            endPosition = transform.position + new Vector3(Random.Range(-1f,-0.4f) , YoffSet,0f);
        }
        else if ( i == 1)
        {
             endPosition = transform.position + new Vector3(Random.Range(0.4f,1f) , YoffSet,0f);
        }

    

        horizontalDistance = Vector3.Distance(startPosition, endPosition);

        float yOffset = endPosition.y - startPosition.y;
        float timeToTop = Mathf.Sqrt((2 * arcHeight) / Mathf.Abs(Physics2D.gravity.y));
        float timeToTarget = Mathf.Sqrt((2 * (arcHeight - yOffset)) / Mathf.Abs(Physics2D.gravity.y));
        totalTime = timeToTop + timeToTarget;

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

            elapsedTime += Time.deltaTime * bulletSpeed;
            yield return null;
        }
       yield return new WaitForSeconds(0.25f);
        can = true;
        
    }
}
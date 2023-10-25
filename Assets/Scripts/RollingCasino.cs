using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;


public class RollingCasino : MonoBehaviour
{
    public Transform[] slotReels;  // ������ � ����������
    public float[] spinSpeed; // �������� �������� ���������
    public float[] spinTimeRow;
    public float spinDuration = 3.0f;  // ������������ ��������
    public AnimationCurve spinCurve;  // ������ �������� ��� ��������� ��������
    public float Pos;
    public float controlPos;
    private bool isSpinning = false;

    // ����������� �������� ��� ������� ��������
    private List<Image> symbolsReel1 = new List<Image>();
    private List<Image> symbolsReel2 = new List<Image>();
    private List<Image> symbolsReel3 = new List<Image>();
    public Transform[] allRow;

    public Sprite[] allSymbols;

    void Start()
    {
        for(int i = 0; i < allRow.Length; i++)
        {
            FindAllChildImages(allRow[i]);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isSpinning)
        {
            StartCoroutine(SpinReels());
        }
    }
    IEnumerator SpinReels()
    {
        // ����� �������� ��� ������� �������� (����� ���������)

        for (int j = 0; j < slotReels.Length; j++)
        {
            slotReels[j].localPosition = new Vector3(slotReels[j].localPosition.x, Pos, slotReels[j].localPosition.z);
        }

        isSpinning = true;

        List<Coroutine> spinCoroutines = new List<Coroutine>();

        for (int i = 0; i < slotReels.Length; i++)
        {
            float spinTime = spinTimeRow[i];
            spinCoroutines.Add(StartCoroutine(SpinReels(spinTime, i)));
        }

        // ��������� ���������� ���� ������� ��������
        foreach (var coroutine in spinCoroutines)
        {
            yield return coroutine;
        }

        // ��������� ��������
        isSpinning = false;
        UpdateSymbols(); // ��������� ������� �� ���������
    }

    IEnumerator SpinReels(float spinTime, int i)
    {
        float startTime = Time.time;

        while (Time.time - startTime < spinTime)
        {
            float t = (Time.time - startTime) / spinTime;
            float moveAmount = spinCurve.Evaluate(t) * spinSpeed[i] * Time.deltaTime;

            // ���������� ������� ����� �� ��� Y (��� ������, ���� ����������)
            slotReels[i].Translate(new Vector3(0, moveAmount, 0));
            yield return null;
        }

        // ����������� ������� �������� �� ������������� ������ (Pos)
        slotReels[i].localPosition = new Vector3(slotReels[i].localPosition.x, controlPos, slotReels[i].localPosition.z);
    }
    private void FindAllChildImages(Transform parent)
    {
        // ���������� ��� �������� �������
        foreach (Transform child in parent)
        {
            // �������� ��������� Image, ���� �� ����
            Image image = child.GetComponent<Image>();
            if (image != null)
            {
                // ���� ��������� Image ������, ��������� ��� � ������
                symbolsReel1.Add(image);
            }

            // ���������� �������� ������� ��� ������ ���������� �������� ��������
            FindAllChildImages(child);
        }
    }
    void UpdateSymbols()
    {
        // ����� �� ������ ����������� ������ ���������� �������� �� ���������.
        // �������� ��������� ������� �� �������� symbolsReel1, symbolsReel2 � symbolsReel3
        // � ���������� �� �� ��������������� ���������.
    }
}
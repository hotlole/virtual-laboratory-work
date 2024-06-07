using UnityEngine;
using TMPro;
using System.Collections;

public class TextChanger : MonoBehaviour
{
    private TMP_Text textComponent;
    public float changeDuration = 30f; // ������������ ��������� ������ � ��������
    public Camera playerCamera; // ������ ������
    public GameObject targetObject; // ������, �� ������� �� ���������� ������
    public GameObject fire; // ������ �� ������ ����
    public LayerMask raycastLayerMask; // ���� ��� Raycast

    private Coroutine changeTextCoroutine; // ������� ��� ��������� ������

    void Start()
    {
        // �������� ��������� TMP_Text �� �������� �������
        textComponent = GetComponent<TMP_Text>();

        if (textComponent == null)
        {
            Debug.LogError("TMP_Text component not found on this GameObject.");
        }

        if (fire == null)
        {
            Debug.LogError("Fire GameObject not found. Please assign it in the inspector.");
        }

        if (playerCamera == null)
        {
            Debug.LogError("Player Camera not found. Please assign it in the inspector.");
        }

        if (targetObject == null)
        {
            Debug.LogError("Target Object not found. Please assign it in the inspector.");
        }
    }

    void Update()
    {
        if (textComponent != null && fire != null && playerCamera != null && targetObject != null)
        {
            bool isLookingAtTarget = IsLookingAtTarget();
            Debug.Log($"Fire active: {fire.activeSelf}, Looking at target: {isLookingAtTarget}");

            if (fire.activeSelf && isLookingAtTarget)
            {
                // ��������� �������� ��� �������� ��������� ������, ���� ��� �� ��������
                if (changeTextCoroutine == null)
                {
                    changeTextCoroutine = StartCoroutine(ChangeTextOverTime());
                }
            }
            else
            {
                // ������������� ��������, ���� ������� ������ �� �����������
                if (changeTextCoroutine != null)
                {
                    StopCoroutine(changeTextCoroutine);
                    changeTextCoroutine = null;
                }
            }
        }
    }

    private bool IsLookingAtTarget()
    {
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        float maxDistance = 100f; // ���������� ������������ ��������� ����

        if (Physics.Raycast(ray, out hit, maxDistance, raycastLayerMask))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
            if (hit.collider != null && hit.collider.gameObject == targetObject)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator ChangeTextOverTime()
    {
        float elapsedTime = 0f;
        float startValue = 0.000f;
        float endValue = 10.000f;

        while (elapsedTime < changeDuration)
        {
            elapsedTime += Time.deltaTime;
            float newValue = Mathf.Lerp(startValue, endValue, elapsedTime / changeDuration);
            textComponent.text = newValue.ToString("F2");
            yield return null;
        }

        textComponent.text = endValue.ToString("F2"); // ���������, ��� ����� � ����� ����� ����� endValue
    }

    public void ChangeText(string newText)
    {
        if (textComponent != null)
        {
            textComponent.text = newText;
        }
        else
        {
            Debug.LogError("TMP_Text component not found on this GameObject.");
        }
    }
}

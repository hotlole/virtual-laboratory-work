using UnityEngine;
using TMPro;
using System.Collections;

public class TextChanger : MonoBehaviour
{
    private TMP_Text textComponent;
    public float changeDuration = 30f; // Длительность изменения текста в секундах
    public Camera playerCamera; // Камера игрока
    public GameObject targetObject; // Объект, на который мы направляем взгляд
    public GameObject fire; // Ссылка на объект огня
    public LayerMask raycastLayerMask; // Слой для Raycast

    private Coroutine changeTextCoroutine; // Корутин для изменения текста

    void Start()
    {
        // Получаем компонент TMP_Text из текущего объекта
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
                // Запускаем корутину для плавного изменения текста, если еще не запущена
                if (changeTextCoroutine == null)
                {
                    changeTextCoroutine = StartCoroutine(ChangeTextOverTime());
                }
            }
            else
            {
                // Останавливаем корутину, если условия больше не выполняются
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
        float maxDistance = 100f; // Установите максимальную дальность луча

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

        textComponent.text = endValue.ToString("F2"); // Убедиться, что текст в конце точно равен endValue
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

using TMPro;
using UnityEngine;

public class OptionListener : MonoBehaviour
{
    [SerializeField] Color mouseOverBoxColor;
    [SerializeField] Color mouseOverTextColor;
    private Color originalColor;
    private Color originalTextColor;
    private string SelectedOptionText { get; set; }

    private void Start()
    {
        originalColor = GetComponent<MeshRenderer>().material.color;
        originalTextColor = GetComponentInChildren<TextMeshProUGUI>().color;
    }

    private void OnMouseOver()
    {
        GetComponent<MeshRenderer>().material.color = mouseOverBoxColor;
        GetComponentInChildren<TextMeshProUGUI>().color = mouseOverTextColor;
    }

    private void OnMouseExit()
    {
        GetComponent<MeshRenderer>().material.color = originalColor;
        GetComponentInChildren<TextMeshProUGUI>().color = originalTextColor;
    }

    private void OnMouseDown()
    {
        SelectedOptionText = GetComponentInChildren<TextMeshProUGUI>().text;
    }

    public string GetSelectedOptionText() => SelectedOptionText;
}

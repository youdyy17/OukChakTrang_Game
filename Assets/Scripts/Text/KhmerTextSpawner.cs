using UnityEngine;
using TMPro;

public class KhmerTextSpawner : MonoBehaviour
{
    public GameObject khmerTextPrefab;
    public Transform parentCanvas;

    void Start()
    {
        GameObject textObj = Instantiate(khmerTextPrefab, parentCanvas);

        TextMeshProUGUI tmp = textObj.GetComponent<TextMeshProUGUI>();
        tmp.text = "សួស្តី​ពិភពលោក"; // Hello World in Khmer
        tmp.fontSize = 48;
        tmp.alignment = TextAlignmentOptions.Center;
    }
}
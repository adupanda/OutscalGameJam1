using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionTextObject : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textMeshProUGUI;
    // Start is called before the first frame update
    void Start()
    {
        GridManager.Instance.UpdateInstructionText.AddListener(UpdateText);
    }

    void UpdateText()
    {
        textMeshProUGUI.SetText(GridManager.Instance.GetTurnTrainName());
        textMeshProUGUI.color = GridManager.Instance.GetTurnTrainColor();
    }
}

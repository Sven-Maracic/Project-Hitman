using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class SettingsMenu : MonoBehaviour
{
    [SerializeField] Color32 highlightedTextColor;
    [SerializeField] Color32 unselectedTextColor;


    [SerializeField] Button generalSettingsButton;
    [SerializeField] Button audioSettingsButton;

    [SerializeField] GameObject generalSettings;
    [SerializeField] GameObject audioSettings;



    private TextMeshProUGUI lastSelectedButton;
    private GameObject lastSelected;


    void Start()
    {
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(generalSettingsButton.gameObject);
        lastSelectedButton = generalSettingsButton.GetComponentInChildren<TextMeshProUGUI>();
        lastSelectedButton.color = highlightedTextColor;
        lastSelected = generalSettings;
    }


    public void OnGeneralPressed()
    {
        lastSelected.SetActive(false);
        lastSelected = generalSettings;
        lastSelected.SetActive(true);

        lastSelectedButton.color = unselectedTextColor;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(generalSettingsButton.gameObject);
        lastSelectedButton = generalSettingsButton.GetComponentInChildren<TextMeshProUGUI>();
        lastSelectedButton.color = highlightedTextColor;
    }

    public void OnAudioPressed()
    {
        lastSelected.SetActive(false);
        lastSelected = audioSettings;
        lastSelected.SetActive(true);

        lastSelectedButton.color = unselectedTextColor;
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(audioSettingsButton.gameObject);
        lastSelectedButton = audioSettingsButton.GetComponentInChildren<TextMeshProUGUI>();
        lastSelectedButton.color = highlightedTextColor;
    }

}

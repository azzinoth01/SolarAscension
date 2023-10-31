using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OnSelectedChanges : MonoBehaviour {
    public string SelectedText;
    private string _originalText;
    
    private Button _button;
    private TMP_Text _text;
    
    void Start() {
        _button = GetComponent<Button>();
        _text = _button.transform.GetChild(0).GetComponent<TMP_Text>();
        _originalText = _text.text;


    }
    
    void Update() {
        if (EventSystem.current.currentSelectedGameObject == gameObject) {
            _text.SetText(SelectedText);
        }
        else {
            _text.SetText(_originalText);
        }
    }
}

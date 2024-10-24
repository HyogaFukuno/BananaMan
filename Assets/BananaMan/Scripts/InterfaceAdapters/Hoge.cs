using UnityEngine;
using UnityEngine.UIElements;

public class Hoge : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var rootDocument = GetComponent<UIDocument>();
        rootDocument?.rootVisualElement.RegisterCallback<NavigationSubmitEvent>(OnSubmit);
        rootDocument?.rootVisualElement.RegisterCallback<NavigationCancelEvent>(OnCancel);
    }

    void OnSubmit(NavigationSubmitEvent e)
    {
        Debug.Log($"OnSubmit: {e}");
    }

    void OnCancel(NavigationCancelEvent e)
    {
        Debug.Log($"OnCancel: {e}");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabToNextField : MonoBehaviour, IUpdateSelectedHandler
{
    public Selectable nextField;

    public void OnUpdateSelected(BaseEventData data)
    {
        Debug.Log("Inside tabnextcontroller");
        if (Input.GetKeyDown(KeyCode.Tab))
            nextField.Select();
    }
}

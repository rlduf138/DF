using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AreaController : MonoBehaviour
{
      public Transform areaTransform;

      EventTrigger eventTrigger;

      void Start()
      {
            eventTrigger = GetComponent<EventTrigger>();
            InitEvent();
      }

      private void InitEvent()
      {
            EventTrigger.Entry click_entry = new EventTrigger.Entry();
            click_entry.eventID = EventTriggerType.PointerUp;
            click_entry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            eventTrigger.triggers.Add(click_entry);
      }

      public void OnPointerUp(PointerEventData eventData)
      {
            if(eventData.pointerEnter == gameObject)
            {
                  Debug.Log("Area OnPointerUp");
                  GameController.Instance.group.transform.position = areaTransform.position;
            }
      }
    
}

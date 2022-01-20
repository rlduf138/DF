using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardTouch : MonoBehaviour
{
      public Card card;

      EventTrigger eventTrigger;

      void Start()
      {
            eventTrigger = GetComponent<EventTrigger>();

            InitEvent();
      }
      public void InitEvent()
      {
            EventTrigger.Entry begin_entry = new EventTrigger.Entry();
            begin_entry.eventID = EventTriggerType.BeginDrag;
            begin_entry.callback.AddListener((data) => { OnBeginDrag((PointerEventData)data); });
            eventTrigger.triggers.Add(begin_entry);

            EventTrigger.Entry drag_entry = new EventTrigger.Entry();
            drag_entry.eventID = EventTriggerType.Drag;
            drag_entry.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
            eventTrigger.triggers.Add(drag_entry);

            EventTrigger.Entry end_entry = new EventTrigger.Entry();
            end_entry.eventID = EventTriggerType.EndDrag;
            end_entry.callback.AddListener((data) => { OnEndDrag((PointerEventData)data); });
            eventTrigger.triggers.Add(end_entry);

            EventTrigger.Entry down_entry = new EventTrigger.Entry();
            down_entry.eventID = EventTriggerType.PointerDown;
            down_entry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            eventTrigger.triggers.Add(down_entry);

            EventTrigger.Entry up_entry = new EventTrigger.Entry();
            up_entry.eventID = EventTriggerType.PointerUp;
            up_entry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            eventTrigger.triggers.Add(up_entry);
      }

      public void OnPointerDown(PointerEventData eventData)
      {
            if (card != null)
                  card.PointerDown();
      }
      public void OnPointerUp(PointerEventData eventData)
      {
            if (card != null)
                  card.PointerUp();
      }
      public void OnBeginDrag(PointerEventData eventData)
      {
            if (card != null)
                  card.OnBeginDrag(eventData);
      }

      public void OnDrag(PointerEventData eventData)
      {
            if (card != null)
                  card.OnDrag(eventData);
      }

      public void OnEndDrag(PointerEventData eventData)
      {
            if (card != null)
                  card.OnEndDrag(eventData);
      }
}

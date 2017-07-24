using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EventManagement;

public class ButtonEventManager : MonoBehaviour {

	public void GenerateEvent(string eventName) {
        
        EventManager.Instance.triggerEvent(eventName, new EventArgs());
    }
}

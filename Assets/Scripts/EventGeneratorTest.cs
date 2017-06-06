using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventGeneratorTest : MonoBehaviour {
    
	void Start () {

        EventManagement.EventManager.Instance.registerCallbackForEvent("ReceiveMessageFromServer",
            (System.EventArgs e) => {
                NetworkWrapper.ReceiveMessageFromServerArgs args = e as NetworkWrapper.ReceiveMessageFromServerArgs;
                Debug.Log(args.messageData);
            });

    }
    
    void Update() {

        if (Input.GetKeyDown(KeyCode.C)) {
            EventManagement.EventManager.Instance.triggerEvent("StartConnectionToServer", new NetworkWrapper.StartConnectionToServerArgs() { url = "wss://exploding-kittens-backend.herokuapp.com/" });
        }

        if (Input.GetKeyDown(KeyCode.D)) {
            EventManagement.EventManager.Instance.triggerEvent("EndConnectionToServer", null);
        }
    }
}

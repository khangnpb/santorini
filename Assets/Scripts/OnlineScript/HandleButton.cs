using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;

namespace HandleButtonNamespace
{
    public class HandleButton : NetworkBehaviour
    {
        [SerializeField]
        public string Name;
        [SerializeField]
        public bool state = false;
        public void OnButtonClick()
        {
            state = !state;
            Debug.Log("Button " + Name + " is clicked");
            if (NetworkServer.active)
            {
                NetworkServer.SendToAll(new MyMessage {content = Name});
                Debug.Log("Send message from Server");
            }
            else if (NetworkClient.active)
            {
                NetworkClient.Send(new MyMessage { content = Name});
                Debug.Log("Send message from Client");
            }
            else Debug.Log("No connection");
        }
        void Start()
        {
            NetworkServer.ReplaceHandler<MyMessage>(OnMessageOfServer);
            NetworkClient.ReplaceHandler<MyMessage>(OnMessage);
        }
        void OnMessage(MyMessage msg)
        {
            Debug.Log("Received message from Server: " + msg.content);
            //Do some thing
            GameObject buttonObject = GameObject.Find(msg.content);

            if (buttonObject != null)
            {
                //Debug.Log("Đã tìm thấy");
                // Lấy component Button của đối tượng
                UnityEngine.UI.Button button = buttonObject.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    //Debug.Log("Đã tìm thấy 2");
                    //if !isLocalhost -> button.onClick.Invoke();
                    if (!NetworkServer.active)
                    {
                        button.onClick.Invoke();
                    }                    
                }
                else
                    Debug.Log("Không tìm thấy component Button");
            }
            else
            {
                Debug.Log("Không tìm thấy đối tượng");
            }
        }

        void OnMessageOfServer(NetworkConnection conn, MyMessage msg)
        {
            Debug.Log("Received message from Client: " + msg.content);//
            GameObject buttonObject = GameObject.Find(msg.content);

            if (buttonObject != null)
            {
                //Debug.Log("Đã tìm thấy");
                // Lấy component Button của đối tượng
                UnityEngine.UI.Button button = buttonObject.GetComponent<UnityEngine.UI.Button>();
                if (button != null)
                {
                    //Debug.Log("Đã tìm thấy 2");
                    //if !isLocalhost -> button.onClick.Invoke();
                    if (NetworkServer.active)
                    {
                        button.onClick.Invoke();
                        Debug.Log("Invoke button");
                    }
                }
                else
                    Debug.Log("Không tìm thấy component Button");
            }
            else
            {
                Debug.Log("Không tìm thấy đối tượng");
            }
        }
        
    }

    public struct MyMessage : NetworkMessage
    {
        // optional message data here
        [SerializeField]
        public string content;
    }
}
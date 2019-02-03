using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkingController : MonoBehaviour {

    Server server;
    Client client;

    public void HostServer() {
        server = new Server(this);
        client = new Client(this);
        server.startServer();
        client.connectToLocal();
        
    }

    public void JoinServer() {
        client = new Client(this);
        client.connectToLocal();
    }

    public void JoinCallback() {
        Invoke("goToTest", 0);
    }

    void goToTest() {
        SceneManager.LoadScene("Scenes/Test");
    }

    public void sendToServer() {
        client.sendNewState();
    }

}
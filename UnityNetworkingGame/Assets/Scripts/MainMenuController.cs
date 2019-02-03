using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

    public GameObject networkManager = null;
    private NetworkManager mgr = null;

    static int maxAllowedClients = 32;
    static string hostAddress = "127.0.0.1";
    static ushort port = 15937;

    const int groupID = MessageGroupIds.START_OF_GENERIC_IDS + 1;

    public void hostServer() {
        TCPServer server = new TCPServer(maxAllowedClients);
        server.Connect();
        Connected(server);
    }

    public void joinServer() {
        TCPClient client = new TCPClient();
        client.Connect(hostAddress, port);
        Connected(client);
    }

    public void Connected(NetWorker networker) {
        if (!networker.IsBound) {
            Debug.LogError("NetWorker failed to bind");
            return;
        }

        if (mgr == null && networkManager == null) {
            Debug.LogWarning("A network manager was not provided, generating a new one instead");
            networkManager = new GameObject("Network Manager");
            mgr = networkManager.AddComponent<NetworkManager>();
        }
        else if (mgr == null)
            mgr = Instantiate(networkManager).GetComponent<NetworkManager>();

        mgr.Initialize(networker);

        if (networker is IServer) {
            SceneManager.LoadScene("Test");
        }
    }
}

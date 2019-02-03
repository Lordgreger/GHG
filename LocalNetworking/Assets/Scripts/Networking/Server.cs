using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

public class Server {
    string serverState;

    NetworkingController nc;
    byte[] buffer;
    Socket serverSocket;
    Socket clientSocket;
    
    public Server(NetworkingController inc) {
        nc = inc;
    }

    public void startServer() {
        MonoBehaviour.print("Starting server!");
        serverState = "starting state";
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        serverSocket.Bind(new IPEndPoint(IPAddress.Any, 3333));
        serverSocket.Listen(0);
        serverSocket.BeginAccept(AcceptCallback, null);
    }

    void AcceptCallback(IAsyncResult ar) {
        clientSocket = serverSocket.EndAccept(ar);
        buffer = new byte[clientSocket.ReceiveBufferSize];

        //byte[] sendData = Encoding.ASCII.GetBytes("Hello");
        //clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);

        MonoBehaviour.print("New connection!");
        serverSocket.BeginAccept(AcceptCallback, null);
    }

    void SendCallback(IAsyncResult ar) {
        byte[] sendData = Encoding.ASCII.GetBytes("Hello");
        clientSocket.EndSend(ar);
        clientSocket.BeginSend(sendData, 0, sendData.Length, SocketFlags.None, SendCallback, null);
    }

    void ReceiveCallback(IAsyncResult ar) {
        int received = clientSocket.EndReceive(ar);
        if (received == 0)
            return;

        string msg = Encoding.ASCII.GetString(buffer);
        string newServerState = getDataValue(msg, "state:");

        MonoBehaviour.print("Received new state: " + newServerState);

        clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }

    static string getDataValue(string data, string index) {
        string search = index + ":";
        string value = data.Substring(data.IndexOf(search) + search.Length);
        return value;
    }
}

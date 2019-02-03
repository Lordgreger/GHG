using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using System.Text;

public class Client {
    Socket socket;
    byte[] buffer;

    NetworkingController nc;

    public Client(NetworkingController inc) {
        nc = inc;
    }

    public void connectToServer() {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3333);
        socket.BeginConnect(endPoint, ConnectCallback, null);
    }

    public void sendNewState() {
        byte[] msg = Encoding.ASCII.GetBytes("state:TEST");
        socket.BeginSend(msg, 0, msg.Length, SocketFlags.None, SendCallback, null);
    }

    void SendCallback(IAsyncResult ar) {
        socket.EndSend(ar);
    }

    public void connectToLocal() {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        var endPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3333);
        socket.BeginConnect(endPoint, ConnectCallback, null);
    }

    void ConnectCallback(IAsyncResult ar) {
        socket.EndConnect(ar);
        buffer = new byte[socket.ReceiveBufferSize];
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
        nc.JoinCallback();
    }

    void ReceiveCallback(IAsyncResult ar) {
        int received = socket.EndReceive(ar);
        if (received == 0)
            return;

        string message = Encoding.ASCII.GetString(buffer);
        MonoBehaviour.print("Received: " + message);

        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }
}

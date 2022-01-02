
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class UDPSender
{
    private static UDPSender _Instance;

    public int remotePort = 8888;  //远端端口号
    public string remoteIpStr = "127.0.0.1"; //

    UdpClient client = null;

    IPAddress remoteIP;
    IPEndPoint remotePoint;

    public static UDPSender Instance {
        get {
            if (_Instance == null)
            {
                _Instance = new UDPSender();
                _Instance.Init();
            }
            return _Instance;
        }
    }

    private void Init()
    {
        Console.WriteLine("初始化");
        client = new UdpClient();
    }

    public void Send(string msg)
    {
        byte[] data = Encoding.Default.GetBytes(msg);
        //Console.WriteLine(data.Length);
        remoteIP = IPAddress.Parse(remoteIpStr);
        remotePoint = new IPEndPoint(remoteIP, remotePort);//实例化一个远程端点 
        client.Send(data, data.Length, remotePoint);//将数据发送到远程端点 
    }

}
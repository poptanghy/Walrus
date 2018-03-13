using UnityEngine;  
using System.Collections;  
//引入库  
using System.Net;  
using System.Net.Sockets;  
using System.Text;  
using System.Threading;  
using System;

public class TestTcpClient:MonoBehaviour  
{  
	string editString="hello wolrd"; //编辑框文字  

	Socket serverSocket; //服务器端socket  
	IPAddress ip; //主机ip  
	IPEndPoint ipEnd;   
	string recvStr; //接收的字符串  
	string sendStr; //发送的字符串  
	byte[] recvData=new byte[40960]; //接收的数据，必须为字节  
	byte[] sendData=new byte[40960]; //发送的数据，必须为字节  
	int iSendData = 0;
	int recvLen; //接收的数据长度  
	Thread connectThread; //连接线程  

	//初始化  
	void InitSocket()  
	{  
		//定义服务器的IP和端口，端口与服务器对应  
		ip=IPAddress.Parse("127.0.0.1");//"183.146.209.103"); //可以是局域网或互联网ip，此处是本机  
		ipEnd=new IPEndPoint(ip,10001);  


		//开启一个线程连接，必须的，否则主线程卡死  
		connectThread=new Thread(new ThreadStart(SocketReceive));  
		connectThread.Start();  
	}  

	void SocketConnet()  
	{  
		if(serverSocket!=null)  
			serverSocket.Close();  
		//定义套接字类型,必须在子线程中定义  
		serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);  
		print("ready to connect");  
		//连接  
		serverSocket.Connect(ipEnd);  
		print("Connected " + serverSocket.Connected);

		//输出初次连接收到的字符串  
		recvLen=serverSocket.Receive(recvData);  
		recvStr=Encoding.ASCII.GetString(recvData,0,recvLen);  
		print(recvStr);  
	}  

	void SocketSend(string sendStr)  
	{  
		//BitConverter.GetBytes(40960)

		/*buf := bytes.NewBuffer(make([]byte, 0, 40960))
			length := len(b) + 8
			binary.Write(buf, binary.BigEndian, uint16(length))
			binary.Write(buf, binary.BigEndian, uint16(0xCCCC))
			binary.Write(buf, binary.BigEndian, id)
			binary.Write(buf, binary.BigEndian, uint16(0))
			binary.Write(buf, binary.BigEndian, b)
	*/
		byte[] akInfo = new byte[40960];
		akInfo = Encoding.ASCII.GetBytes (sendStr);
		ushort id = 994;
		Send_Clear(); 
		ushort iLength = (ushort)(akInfo.Length + 8);
		Send_Write (iLength);
		Send_Write ((ushort)0xCCCC);
		Send_Write (id);
		Send_Write ((ushort)0);
		Send_Write (akInfo);

		string kString = Encoding.ASCII.GetString (sendData, 0, iSendData);
		print(kString);  

		serverSocket.Send(sendData,iSendData,SocketFlags.None);  
	}

	void Send_Clear()
	{
		sendData=new byte[40960];
		iSendData = 0;
	}

	void Send_Write(ushort kVal)
	{
		Send_Write (BitConverter.GetBytes(kVal));
	}

	void Send_Write(byte[] kVal)
	{
		for (int i = 0; i < kVal.Length; i++) 
		{
			sendData [iSendData] = kVal [i];
			iSendData++;
		}
	}


	void SocketReceive()  
	{  
		SocketConnet();  
		//不断接收服务器发来的数据  
		while(true)  
		{  
			recvData=new byte[40960];  
			recvLen=serverSocket.Receive(recvData);  
			if(recvLen==0)  
			{  
				SocketConnet();  
				continue;  
			}  
			recvStr=Encoding.ASCII.GetString(recvData,0,recvLen);  
			print(recvStr);  
		}  
	}  

	void SocketQuit()  
	{  
		//关闭线程  
		if(connectThread!=null)  
		{  
			connectThread.Interrupt();  
			connectThread.Abort();  
		}  
		//最后关闭服务器  
		if(serverSocket!=null)  
			serverSocket.Close();  
		print("diconnect");  
	}  

	// Use this for initialization  
	void Start()  
	{  
		InitSocket();  
	}  

	void OnGUI()  
	{  
		editString=GUI.TextField(new Rect(10,10,100,20),editString);  
		if(GUI.Button(new Rect(10,30,60,20),"send"))  
			SocketSend(editString);  
	}  

	// Update is called once per frame  
	void Update()  
	{  

	}  

	//程序退出则关闭连接  
	void OnApplicationQuit()  
	{  
		SocketQuit();  
	}  
}  
private string getIpAddress()
{
	IPAdress[] addressList = Dns.GetHostByName(Dns.GetHostName()).AddressList;
	if (addressList.Length < 1)
	{
		return "";
	}
	return addressList[0].ToString();
}

void NetWorkConnect()  // 接收数据类型
{
	string remote_data_ip = "225.0.72.1"; // this.data_ip_msk.Text.Trim();
	int remote_data_port = 2051; // int.Parse(this.data_port_msk.Text.Trim)
	string local_ip = getIpAddress();

	try
	{
		// 套接字配置
		IPEndPoint data_ipe = new IPEndPoint(IPAddress.Parse(local_ip), remote_data_port);
		data_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
		EndPoint data_ep = (EndPoint)data_ipe;

		data_socket.SetSocketOption(SocketOptionLevel.IP, SocketOptionName.AddMembership, new MulticastOption(IPAddress.Parse(remote_data_ip), IPAddress.Parse(local_ip)));
		data_socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
		data_socket.Bind(data_ipe);

		while (true)
		{
			if (!isReceivedPar)
				continue;

			dataBytes = new byte[(DATA_COUNT + 4 + 1) * 4]; // 接受数据缓存区
			recvCnt = data_socket.ReceiveFrom(dataBytes, ref data_ep); // 接收数据
			if(recvCnt <= 0) // 未接到数据
				continue;
			for (int i = 0; i < recvDataBuff)
			{
				recvDataBuff[i] = BitConverter.ToSingle(dataBytes, i * 4); // 类型转换float
			}

			isRecievedData = true; // 已接受到数据
		}
	}
	catch (Exception ex)
	{
		MessageBox.show(ex.Message);
		return;
	}
}
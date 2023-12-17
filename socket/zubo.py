# 开放时间：2023/12/12 21:20
# 开发者：萤火、

import socket
import struct

DATA_COUNT = 20000 

class Connector:
    def __init__(self, ip, port):
        self.ip = ip
        self.port = port
        self.multicast_socket = None
        self.isRecievedData = False

    def connect(self):
        try:
            # 创建一个UDP套接字
            multicast_socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)

            # 设置组播地址
            group = socket.inet_aton(self.ip)

            # 加入组播组
            multicast_socket.setsockopt(socket.IPPROTO_IP, socket.IP_ADD_MEMBERSHIP, socket.inet_aton(self.ip) + socket.inet_aton('0.0.0.0'))

            # 绑定到组播端口
            multicast_socket.bind(('0.0.0.0', self.port))

            self.multicast_socket = multicast_socket
            print(f"加入udp {self.ip}:{self.port} 组播成功")

        except Exception as e:
            print(f"加入udp {self.ip}:{self.port} 组播失败，原因: {e}")
            return False
        return True

    def receive(self):
        try:
            while True:
                # if not isReceivedPar: # 这个变量的含义是什么？
                #     continue

                dataBytes = bytearray((DATA_COUNT + 4 + 1) * 4)

                recvCnt, addr = self.multicast_socket.recvfrom_into(dataBytes)

                if recvCnt <= 0:
                    continue

                recvDataBuff = [struct.unpack('f', dataBytes[i*4:i*4+4])[0] for i in range(recvCnt//4)]
                
                print(recvDataBuff)

                self.isRecievedData = True

        except Exception as e:
            print(f"接收udp组播数据失败，原因: {e}")

if __name__ == "__main__":
    # 测试案例
    udp_connector = Connector(ip='225.0.71.1', port=7104) # 10.182.37.27

    if udp_connector.connect():
        udp_connector.receive()




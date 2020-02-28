"""
Get IPv6 Address
"""
__author__ = "IS_F06"
__date__ = "Date: 2009/12/14"

import socket

def GetIPv6Addr ():
	if socket.has_ipv6:
		try :
			s = socket.socket(socket.AF_INET6, socket.SOCK_DGRAM)
			s.connect(('bbs6.sjtu.edu.cn', 0))
			return s.getsockname()[0]
		except socket.gaierror:
			return None
	else:
		return None

if __name__ == "__main__":
	sAdd = GetIPv6Addr()
	if sAdd :
		print "IPv6 Address: " + sAdd
	else :
		print "Fail in getting IPv6 Address"
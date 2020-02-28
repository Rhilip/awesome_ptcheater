"""PTLiar v1.0 Created by IS_F06

Usage: PTLiar [options]
Options:
    -h             [H]elp, printing this message
    -s             [S]tart "uploading"
    -i [integer]   M[i]nimum uploading speed, specified in KB/s, (default 0)
    -a [integer]   M[a]ximum uploading speed, specified in KB/s, (default 2048)
    -e             [E]nable IPv6
    -c [0-2]       To specify BT [C]lient, (default 1[uTorrent1.85])
    -t [integer]   [T]imer, specified in minutes, (default 9999)
    -v             [V]erbose mode
Client List:
     0 - uTorrent2.00B  1 - uTorrent1.85  2 - uTorrent1.83  3 - uTorrent1.61
Example: python PTLiar.py -s -i 100 -a 5120 -c 1 -e -t 35
         min speed set to 100KB/s,
         max speed set to 5MB/s,
         using client uTorrent1.83,
         using IPv6,
         disconnect at 35min
*Hit [Ctrl+C] at anytime for a quick and clean exit
*DO NOT close console window directly"""
__author__ = "IS_F06"
__version__ = "Revision 1.0$"
__licence__ = u"GPL"
__date__ = "Date: 2009/12/17"

import httplib, hashlib, urllib, os, os.path, time, getopt, sys, threading, random, StringIO, gzip
import bencode, ipv6addr

#CONSTANTS
#Torrent Path
sTorrentPath = "torrents"

#Number of Connections
iNumConnections = 4

lBTClient = [["uTorrent2.0B", "uTorrent/200B(17539)", "-UT200B-"], ["uTorrent1.85", "uTorrent/1850(17414)", "-UT1850-"], ["uTorrent1.83", "uTorrent183B(14809)", "-UT183B-"], ["uTorrent1.61", "uTorrent/1610", "-UT1610-"]]
lHex = ["0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "A", "B", "C", "D", "E", "F"]

#GLOBAL VARIABLES
dTorrentInfoDict = {}
dHeaders = {}
iMinSpeed = iMaxSpeed = None
iPort = None
iClientID = None
iTorrentNum = None

tIsIPv6 = None
tVerbose = None
sPeerID = None
sKey = None
sIPv6Addr = None
mPrintMutex = threading.Lock()

iTimer = None
iTimeElapsed = None
iTotalUploaded = None

def Usage():
	print __doc__

def PrettyFileSize(iBytes):
	if iBytes >= 1073741824 :
		return str(round (iBytes / 1024 / 1024 / 1024.0, 1)) + 'GB'
	elif iBytes >= 1048576 :
		return str(round (iBytes / 1024 / 1024.0, 1)) + 'MB'
	elif iBytes >= 1024 :
		return str(round (iBytes / 1024.0, 1)) + 'KB'
	elif iBytes < 1024 :
		return str(round (iBytes, 1)) + 'B'

def HexToByte(sHexStr):
	lBytes = []
	for i in range(0, len(sHexStr), 2):
		lBytes.append(chr(int(sHexStr[i:i+2], 16)))
	return ''.join(lBytes)

def Initializer() :
	global dHeaders, sKey, sPeerID, iPort, iTorrentNum, iTotalUploaded, iTimeElapsed

	iTorrentID = 0
	
	#reading torrents
	print "Reading torrent files..."
	for item in os.listdir(sTorrentPath):
		if os.path.isfile(os.path.join(sTorrentPath, item)) and item.split('.')[-1:][0].lower() == 'torrent' :
			print item
			file = open(os.path.join(sTorrentPath, item), "rb")
			#B decoding torrent file
			dMetainfo = bencode.bdecode(file.read())
			file.close()

			#generating Infohash
			info = dMetainfo["info"]
			sha1 = hashlib.sha1()
			sha1.update(bencode.bencode(info))
			infohash = sha1.hexdigest()
			
			#fill in info table
			dTorrentInfo = {}
			dTorrentInfo["Host"] = dMetainfo["announce"].split('/')[2]
			dTorrentInfo["Link"] = '/'.join(dMetainfo["announce"].split('/')[3:])
			dTorrentInfo["InfoHash"] = HexToByte(infohash)
			dTorrentInfo["TorrentName"] = item
			dTorrentInfo["Uploaded"] = 0
			dTorrentInfo["TimeLeft"] = 0
			dTorrentInfo["Status"] = "started"
			dTorrentInfo["Name"] = '.'.join(item.split('.')[:-1])
			dTorrentInfoDict[iTorrentID] = dTorrentInfo
			iTorrentID += 1
	
	iTorrentNum = iTorrentID
	iTotalUploaded = 0
	iTimeElapsed = -1
	
	#initializing HTTP-headers
	dHeaders["Host"] = None
	dHeaders["User-Agent"] = lBTClient[iClientID][1]
	dHeaders["Accept-Encoding"] = "gzip"

	#generating PortNumber, PeerID and Keystring
	sPeerIDHex = ''.join(random.sample(lHex+lHex+lHex, 24))
	sPeerIDChar = HexToByte(sPeerIDHex)
	sPeerID = lBTClient[iClientID][2] + sPeerIDChar
	sKey = ''.join(random.sample(lHex+lHex+lHex, 8))
	iPort = random.randint(10000, 65000)

def Controller() :
	global dTorrentInfoDict, iTimer, iTotalUploaded, iTimeElapsed

	tStopped = False
	lThreadPool = []
	while True :
		iCount = 0
		if tStopped :
			print "Stopping..."
		for iTorrentID in dTorrentInfoDict :
			dTorrentInfo = dTorrentInfoDict[iTorrentID]
			if dTorrentInfo["Status"] != "error" :
				if dTorrentInfo["TimeLeft"] == 0 :
					th = threading.Thread(target = RequestHandler, args = [iTorrentID])
					lThreadPool.append(th)
					iCount += 1
			if iCount == iNumConnections :
				for th in lThreadPool :
					th.start()
				for th in lThreadPool :
					threading.Thread.join(th)
				time.sleep(1)
				lThreadPool = []
				iCount = 0
		if iCount != 0 :
			for th in lThreadPool :
				th.start()
			for th in lThreadPool :
				threading.Thread.join(th)
			lThreadPool = []
			iCount = 0
		if tStopped :
			print "Total Uploaded: " + PrettyFileSize(iTotalUploaded) + " Time Elapsed: " + str(iTimeElapsed) + " mins"
			print "Bye~"
			sys.exit()
		if iTorrentNum < 1 :
			print "Error: No torrents available"
			sys.exit()
		#Increasing uploaded value
		iSumUploaded = random.randint(iMinSpeed * 1024 * 60, iMaxSpeed * 1024 * 60)
		
		lRandList = []
		iSumRand = 0
		iCount = 0
		while iCount < iTorrentNum :
			lRandList.append(random.randint(0,1000))
			iSumRand += lRandList[iCount]
			iCount += 1
		iCount = 0
		if iSumRand == 0 :
			iSumRand = 1
		for iTorrentID in dTorrentInfoDict :
			dTorrentInfo = dTorrentInfoDict[iTorrentID]
			if dTorrentInfo["Status"] != "error" :
				dTorrentInfoDict[iTorrentID]["Uploaded"] += int(iSumUploaded*lRandList[iCount]/iSumRand)
				iCount += 1
		iTotalUploaded += iSumUploaded
		iTimeElapsed += 1
		print "Upload: " + PrettyFileSize(iSumUploaded) + " Total: " + PrettyFileSize(iTotalUploaded) + " TimePassed: " + str(iTimeElapsed) + "min Speed: " + PrettyFileSize(iTotalUploaded/(iTimeElapsed+1)/60) + "/s"
		try :
			time.sleep(60)
			for iTorrentID in dTorrentInfoDict :
				dTorrentInfo = dTorrentInfoDict[iTorrentID]
				if dTorrentInfo["Status"] != "error" :
					dTorrentInfo["TimeLeft"] -= 1
		except KeyboardInterrupt:
			tStopped = True
			for iTorrentID in dTorrentInfoDict :
				dTorrentInfo = dTorrentInfoDict[iTorrentID]
				if dTorrentInfo["Status"] != "error" :
					dTorrentInfo["Status"] = "stopped"
					dTorrentInfo["TimeLeft"] = 0
			print "Please wait..."
			pass
		iTimer -= 1
		if iTimer == -1 :
			tStopped = True
			for iTorrentID in dTorrentInfoDict :
				dTorrentInfo = dTorrentInfoDict[iTorrentID]
				if dTorrentInfo["Status"] != "error" :
					dTorrentInfo["Status"] = "stopped"
					dTorrentInfo["TimeLeft"] = 0

def RequestHandler (iTorrentID) :
	global dHeaders, dTorrentInfoDict, iPort, sKey, sPeerID, iTorrentNum, tIsIPv6

	dTorrentInfo = dTorrentInfoDict[iTorrentID]
	dHeaders["Host"] = dTorrentInfo["Host"]
	sEvent = dTorrentInfo["Status"]
	if tIsIPv6 and sEvent != "stopped" :
		sIPv6 = "&ipv6=" + urllib.quote(sIPv6Addr)
	else :
		sIPv6 = ""
	if sEvent == "stopped" :
		iNumwant = 0
	else :
		iNumwant = 200
	lString = ['/', dTorrentInfo["Link"], "&info_hash=", urllib.quote(dTorrentInfo["InfoHash"]), "&peer_id=", urllib.quote(sPeerID), "&port=", str(iPort), "&uploaded=", str(dTorrentInfo["Uploaded"]), "&downloaded=0&left=0&corrupt=0&key=", sKey, "&event=", sEvent, "&numwant=", str(iNumwant), "&compact=1&no_peer_id=1", sIPv6]
	sReqString = ''.join(lString)

	mPrintMutex.acquire()
	if tVerbose :
		print "Sending request [" + dTorrentInfo["Name"] + "] Status: " + dTorrentInfo["Status"] + ", Uploaded: " + PrettyFileSize(dTorrentInfo["Uploaded"])
	else :
		print "REQ [" + dTorrentInfo["Name"] + "]"
	mPrintMutex.release()

	conn = httplib.HTTPConnection(dTorrentInfo["Host"])
	conn.request("GET", sReqString, None, dHeaders)
	rResponse = conn.getresponse()
	if sEvent == "started" :
		#changing status to completed after 1st request
		dTorrentInfoDict[iTorrentID]["Status"] = "completed"
	if rResponse.status != 200 :
		# :( HTTPconnection error occured
		dTorrentInfoDict[iTorrentID]["Status"] = "error"
		mPrintMutex.acquire()
		iTorrentNum -= 1
		if tVerbose :
			print "Error occured [" + dTorrentInfo["Name"] + "]" + "Status: " + str(rResponse.status)
		else :
			print "ERR [" + dTorrentInfo["Name"] + "]"
		mPrintMutex.release()
	else :
		if dTorrentInfoDict[iTorrentID]["Status"] != "stopped" :
			sData = rResponse.read()
			lHHeaders = rResponse.getheaders()
			tGzipEncoded = False
			for (sHKey, sHValue) in lHHeaders :
				if sHKey.lower() == "content-encoding" and sHValue.lower() == "gzip":
					tGzipEncoded = True
			if tGzipEncoded :
				#Gzip decoding
				strCompress = StringIO.StringIO(sData)
				fGzip = gzip.GzipFile(fileobj = strCompress)
				sInfo = fGzip.read()
			else :
				sInfo = sData
			#B decoding
			dMetainfo = bencode.bdecode(sInfo)

			#failure reason received
			if dMetainfo.has_key("failure reason") :
				dTorrentInfoDict[iTorrentID]["Status"] = "error"
				mPrintMutex.acquire()
				iTorrentNum -= 1
				if tVerbose :
					print "Error occured [" + dTorrentInfo["Name"] + "] " + "Falure reason: " + dMetainfo["failure reason"]
				else :
					print "ERR [" + dTorrentInfo["Name"] + "]"
				mPrintMutex.release()
			#interval received
			elif dMetainfo.has_key("interval") :
				dTorrentInfoDict[iTorrentID]["TimeLeft"] = int(dMetainfo["interval"]/60)
				mPrintMutex.acquire()
				if tVerbose :
					print "Response received [" + dTorrentInfo["Name"] + "] " + "Interval: " + str(dTorrentInfoDict[iTorrentID]["TimeLeft"]) + "min"
				else :
					print "RECV [" + dTorrentInfo["Name"] + "] " + "INT: " + str(dTorrentInfoDict[iTorrentID]["TimeLeft"]) + "min"
				mPrintMutex.release()
			#unknown condition
			else :
				dTorrentInfoDict[iTorrentID]["Status"] = "error"
				mPrintMutex.acquire()
				iTorrentNum -= 1
				if tVerbose :
					print "Error occured [" + dTorrentInfo["Name"] + "] " + "Unknown error"
				else :
					print "ERR [" + dTorrentInfo["Name"] + "]"
				mPrintMutex.release()	
		else :
			mPrintMutex.acquire()
			if tVerbose :
				print "Response received [" + dTorrentInfo["Name"] + "]"
			else :
				print "STOP [" + dTorrentInfo["Name"] + "]"
			mPrintMutex.release()	
	conn.close()

def main(argv):
	global iMinSpeed, iMaxSpeed, iTimer, tIsIPv6, tVerbose, iClientID, tStopped, sIPv6Addr

	tStart = False

	#default values
	iTimer = 9999
	tIsIPv6 = False
	tVerbose = False
	iClientID = 1
	
	#getting options
	try:
		opts, args = getopt.getopt(argv,"hsi:a:ec:t:v")
	except getopt.GetoptError:
		Usage()
		sys.exit(2)
	for opt, arg in opts:
		if opt == "-h" :
			Usage()
			sys.exit()
		elif opt == "-s" :
			tStart = True
		elif opt == "-i" :
			try :
				iMinSpeed = int(arg)
			except ValueError:
				print "Check input: Minimum speed must be a non-negetive integer, specified in KB/s"
				sys.exit()
			if iMinSpeed < 0 :
				print "Check input: Minimum speed must be a non-negetive integer, specified in KB/s"
				sys.exit()
		elif opt == "-a" :
			try :
				iMaxSpeed = int(arg)
			except ValueError:
				print "Check input: Maximum speed must be a positive integer, specified in KB/s"
				sys.exit()
			if iMaxSpeed < 1 :
				print "Check input: Maximum speed must be a positive integer, specified in KB/s"
				sys.exit()
		elif opt == "-e" :
			tIsIPv6 = True
		elif opt == "-c" :
			try :
				iClientID = int(arg)
			except ValueError:
				print "Check input: ClientID must be an integer"
				sys.exit()
			if not 0 <= iClientID <= 3 :
				print "Check input: ClientID must be within range [0,3]"
				sys.exit()
		elif opt == "-t" :
			try :
				iTimer = int(arg)
			except ValueError:
				print "Check input: Timer must be a positive integer, specified in minutes"
				sys.exit()
			if iTimer < 1 :
				print "Check input: Timer must be a positive integer, specified in minutes"
				sys.exit()
		elif opt == "-v" :
			tVerbose = True
	if tStart == False :
		Usage()
		sys.exit()

	if iMinSpeed == None :
		iMinSpeed = 0
		if iMaxSpeed == None :
			iMaxSpeed = 2048
	elif iMaxSpeed == None :
		if 2*iMinSpeed > 2048 :
			iMaxSpeed = 2048
		else :
			iMaxSpeed = 2*iMinSpeed
	elif iMinSpeed > iMaxSpeed :
		print "Check input: Max Speed should be greater than Min Speed"
		sys.exit

	print "Setting minimum upload speed to " + str(iMinSpeed) + "KB/s"
	print "Setting maximum upload speed to " + str(iMaxSpeed) + "KB/s"
	print "Setting BTClient to " + lBTClient[iClientID][0]
	print "Will disconnect at " + str(iTimer) + "min"
	if tIsIPv6 :
		print "IPv6 status: ON"
		sIPv6Addr = ipv6addr.GetIPv6Addr()
		if not sIPv6Addr :
			print "Error: Cannot get IPv6 Address"
			sys.exit()
	else :
		print "IPv6 status: OFF"
	try:
		Initializer()
	#keyboard interrupt disabled
	except KeyboardInterrupt:
		pass
	try:
		Controller()
	#keyboard interrupt disabled
	except KeyboardInterrupt:
		pass
			
if __name__ == "__main__":
    main(sys.argv[1:])
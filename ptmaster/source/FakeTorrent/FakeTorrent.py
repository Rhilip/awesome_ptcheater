# -*- coding: utf-8 -*-
#
# FakeTorrent, a fake seeding software, new name of PTLiar
# Acknowledge to PTLiar.com
# Copyright (C) 2011 PTLiar.com
#
# This program is free software; you can redistribute it and/or
# modify it under the terms of the GNU General Public License
# as published by the Free Software Foundation; either version 2
# of the License, or (at your option) any later version.
#
# This program is distributed in the hope that it will be useful,
# but WITHOUT ANY WARRANTY; without even the implied warranty of
# MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
# GNU General Public License for more details.
#
# You should have received a copy of the GNU General Public License
# along with this program; if not, write to the Free Software
# Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

# standard packages
import __future__
import threading
import StringIO
import hashlib
import httplib
import logging
import shutil
import getopt
import random
import socket
import gzip
import time
import sys
import ssl
import os

# internal packages
from client_info import *
from config import *
from utils import *
import bencode

__author__  = "PTLiar.com && wujiajie"
__version__ = "v1.0"
__date__    = "2012/06/11"
__licence__ = "GNU General Public License v2.0"
__url__     = "http://PTLiar.com"
__email__   = "wujiajie@sjtu.edu.cn"
__doc__     = """\
FakeTorrent %(version)s by %(author)s, %(date)s
url: %(url)s
email: %(email)s
happy with python 2.7
usage: FakeTorrent [options]
options:
    -h  help, print this message
    -l  print the list of supported clients
    -m  maximum up bandwidth, in KB/s, (default: %(DEFAULT_MAX_UP_SPEED)s)
    -M  maximum down bandwidth, in KB/s, (default: %(DEFAULT_MAX_DOWN_SPEED)s)
    -s  maximum speed per torrent, in KB/s, (default: %(DEFAULT_MAX_TORRENT_SPEED)s)
    -c  client to emulate (default: %(DEFAULT_CLIENT)s)
    -t  timer, in hours (default: %(LUCKY_NUMBER)s)
    -p  fake port number (default: random)
    -e  enable IPv6 (default: disabled)
    -z  enable 'zero-rate' (default: disabled)
    -n  disable 'scrape' (default: enabled)
    -f  skip some sleep
    -v  verbose
press [Ctrl+C] for exit""" % {
    "author"    : __author__,
    "version"   : __version__,
    "date"      : __date__,
    "url"       : __url__,
    "email"     : __email__,
    "DEFAULT_MAX_UP_SPEED"      : DEFAULT_MAX_UP_SPEED,
    "DEFAULT_MAX_DOWN_SPEED"    : DEFAULT_MAX_DOWN_SPEED,
    "DEFAULT_MAX_TORRENT_SPEED" : DEFAULT_MAX_TORRENT_SPEED,
    "DEFAULT_CLIENT"            : DEFAULT_CLIENT,
    "LUCKY_NUMBER"              : LUCKY_NUMBER,
}

# Q: what the fuck is SSSS?
# A: shanghai southwest some school

# project homepage: PTLiar.com
# email: s@PTLiar.com

# Newly update: wujiajie
# email: wujiajie@sjtu.edu.cn


class PTLiarSettings:
    """
    global settings
    """
    def __init__(self):
        # default
        self.use_ipv6       = False             # send ipv6 addr to tracker?
        self.use_zero_rate  = False             # enable zero-rate?
        self.no_sleep       = False             # skip sleep if possible?
        self.no_scrape      = False             # disable "scrape"?
        self.client_id      = DEFAULT_CLIENT    # the client we fake
        self.timer          = LUCKY_NUMBER*HOUR # timer
        self.max_up_speed   = DEFAULT_MAX_UP_SPEED*KILO      # maximum upload bandwidth
        self.max_down_speed = DEFAULT_MAX_DOWN_SPEED*KILO    # maximum download bandwidth
        self.max_torrent_speed = DEFAULT_MAX_TORRENT_SPEED*KILO # maximum speed per torrent
        self.logging_level  = logging.INFO      # logging level
        self.str_ipv6       = ""                # urlencoded ipv6 address
        # not set
        self.scrapable      = None              # whether "scrape" is supported
        self.peer_id        = None              # our fake identifier
        self.client_key     = None              # our fake client-key
        self.port_num       = None              # our fake port number
        self.http_headers = {                   # the required http headers
            "Accept-Encoding" : "gzip",
            "Connection"      : "Close",
        }

    def fuck_yourself(self):
        # set ipv6 address if needed
        if self.use_ipv6:
            ipv6_addr = get_ipv6_addr()
            if not ipv6_addr:
                ptl_error("Cannot get IPv6 address")
            self.str_ipv6 = "&ipv6=%s" % urllib.quote(ipv6_addr)
        # generate listening port number
        if not self.port_num:
            self.port_num = random.randint(MIN_PORT_NUM, MAX_PORT_NUM)
        # generate client key
        self.client_key = "".join(random.sample(HEX*3, 8))
        client_info = BT_CLIENTS[self.client_id]
        # generate peer_id : based on client chosen
        self.peer_id = client_info["peer-id"] + hex_to_byte("".join(random.sample(HEX*3, 24)))
        self.quoted_peer_id = urllib.quote(self.peer_id)
        # generate http header[user-agent] : based on client chosen
        self.http_headers.update({"User-Agent" : client_info["user-agent"]})
        # supports scrape?
        self.scrapable = not self.no_scrape and client_info["scrape"]
        # create directories if not exist
        for up_down in (UP, DOWN):
            if not os.path.exists(DIR[up_down]):
                try:
                    os.mkdir(DIR[up_down])
                except:
                    pass
        # logging
        try:
            # dont keep a huge log file
            if os.path.exists(LOG_FILE) and os.path.getsize(LOG_FILE) > MEGA:
                os.remove(LOG_FILE)
        except:
            pass
        format = logging.Formatter(FMT, DATEFMT)
        # stdout handler
        stdo_handler = logging.StreamHandler(sys.stdout)
        stdo_handler.setLevel(self.logging_level)
        stdo_handler.setFormatter(format)
        # log file handler
        file_handler = logging.FileHandler(LOG_FILE)
        file_handler.setLevel(logging.DEBUG)
        file_handler.setFormatter(format)
        self.log = logging.getLogger(LOG_NAME)
        self.log.addHandler(stdo_handler)
        self.log.addHandler(file_handler)
        self.log.setLevel(logging.DEBUG)
        self.log.debug("<= PTLiar started, version: %s" % __version__)
        self.log.info("Verbose            : %s"   % ("ON" if self.logging_level==10 else "OFF"))
        self.log.info("IPv6               : %s"   % ("ON" if self.use_ipv6          else "OFF"))
        self.log.info("Zero-Rate          : %s"   % ("ON" if self.use_zero_rate     else "OFF"))
        self.log.info("Timer              : %s"   % ptime(self.timer))
        self.log.info("Max Up Bandwidth   : %s/s" % psize(self.max_up_speed))
        self.log.info("Max Down Bandwidth : %s/s" % psize(self.max_down_speed))
        self.log.info("Max Torrent Speed  : %s/s" % psize(self.max_torrent_speed))
        self.log.info("Client             : %s"   % self.client_id)

class TicketSeller:
    """
    ticket-based speed manager
    """
    def __init__(self):
        self._tickets_left = {
            UP   : ALL_TICKETS,
            DOWN : ALL_TICKETS,
        }
        self._locks = {
            UP   : threading.Lock(),
            DOWN : threading.Lock(),
        }

    def fuck_yourself(self):
        self._safe_tickets = {
            UP   : ps.max_torrent_speed * ALL_TICKETS / ps.max_up_speed,
            DOWN : ps.max_torrent_speed * ALL_TICKETS / ps.max_down_speed,
        }

    def _has_no_luck(self, got):
        """
        zero rate stuff
        """
        luck = random.uniform(0, 1)
        if (got > ZR_GOT_L3 and luck < ZR_LUCK_L3) or \
           (got > ZR_GOT_L2 and luck < ZR_LUCK_L2) or \
           (got > ZR_GOT_L1 and luck < ZR_LUCK_L1):
            return True
        return False

    @property
    def up_speed(self):
        return ps.max_up_speed * (ALL_TICKETS - self._tickets_left[UP]) / ALL_TICKETS

    @property
    def down_speed(self):
        return ps.max_down_speed * (ALL_TICKETS - self._tickets_left[DOWN]) / ALL_TICKETS

    def get_up_speed(self, torrent):
        return ps.max_up_speed * torrent.tickets[UP] / ALL_TICKETS

    def get_down_speed(self, torrent):
        return ps.max_down_speed * torrent.tickets[DOWN] / ALL_TICKETS

    def get_tickets(self, torrent, up_down):
        """
        get random number of tickets
        """
        with self._locks[up_down]:
            safe_tickets = self._safe_tickets[up_down]
            if up_down == UP and torrent.down_peers < 8:
                safe_tickets /= 2
            possible_tickets = min(self._tickets_left[up_down], safe_tickets)
            tickets_got = random.randint(0, possible_tickets)
            if ps.use_zero_rate and self._has_no_luck(tickets_got):
                tickets_got = 0
            self._tickets_left[up_down] += torrent.tickets[up_down] - tickets_got
        return tickets_got

    def return_tickets(self, torrent, up_down):
        """
        return some lottery tickets
        """
        with self._locks[up_down]:
            self._tickets_left[up_down] += torrent.tickets[up_down]
        torrent.tickets[up_down] = 0
        torrent.speed[up_down] = 0

class Torrent:
    """
    torrent module
    """
    conns = {
        "http"  : httplib.HTTPConnection,
        "https" : httplib.HTTPSConnection,
    }

    def __init__(self):
        self.status = "started"     # some event or "noevent"
        self.uploaded   = 0         # bytes fake uploaded (committed)
        self.downloaded = 0         # bytes fake downloaded (comitted)
        self.up_peers   = 0         # number of complete peers
        self.down_peers = 0         # number of incomplete peers
        self.speed   = { UP : 0, DOWN : 0 }
        self.tickets = { UP : 0, DOWN : 0 }
        self.last_commit_time = 0   # the time of last commit (now/in the past)
        self.next_commit_time = 0   # the time of next commit (in the future)

    def hash(self):
        """
        uniquely identify a torrent
        """
        return (self.host, self.infohash)

    def _parse_url(self, url):
        """
        parse url to tuple (protocol, host, link)
        """
        protocol, rest = urllib.splittype(url)
        host, link = urllib.splithost(rest)
        return (protocol, host, link)

    @property
    def scrapable(self):
        """
        whether the tracker supports "scrape"
        see: http://wiki.theory.org/BitTorrentSpecification#Tracker_.27scrape.27_Convention
        """
        return ps.scrapable and self.link.rpartition("/")[-1].startswith("announce")

    def load(self, up_down, filename):
        """
        load torrent information from a file
        """
        self.filename = filename
        self.name = filename.rpartition(".")[0][:20]
        self.up_down = up_down
        full_path = os.path.join(DIR[up_down], filename)
        with open(full_path, "rb") as f:
            meta_info = bencode.bdecode(f.read())
        # generating Infohash
        info = meta_info["info"]
        sha1 = hashlib.sha1()
        sha1.update(bencode.bencode(info))
        infohash = sha1.hexdigest()
        # get url infomation
        self.protocol, self.host, self.link = self._parse_url(meta_info["announce"])
        # get infohash
        self.infohash = hex_to_byte(infohash)
        self.quoted_infohash = urllib.quote(self.infohash)
        # get torrent size
        if "files" in info:
            # a mutli-file torrent (having a sub-directory structure)
            self.size = sum(map(lambda x:x["length"], info["files"]))
        elif "length" in info:
            # The following table gives the structure of a single-file torrent
            # (does not have a sub-directory structure)
            self.size = int(info["length"])
        else:
            # weird cases
            self.size = int(info["piece length"])
        # get left
        if up_down == UP:
            self.left = 0
        elif up_down == DOWN:
            left = -1
            try:
                left_file = os.path.join(DIR[DOWN], "%s.left" % self.filename)
                with open(left_file, "r") as f:
                    left = int(f.readline())
            except:
                pass
            if left < 0 or left > self.size:
                left = self.size
            self.left = left

    @property
    def is_ready(self):
        return self.status == "stopped" or \
              (self.status != "error" and self.next_commit_time <= time.time())

    def _update_status(self):
        """
        update uploaded/downloaded/status etc.
        """
        if self.last_commit_time:
            # not the first commit, make up a uploaded value based on the tickets it has
            time_delta = time.time() - self.last_commit_time
            self.uploaded += int(ts.get_up_speed(self) * time_delta)
        else:
            # it's the first commit
            assert(self.uploaded == 0)
            assert(self.downloaded == 0)
        if self.up_down == UP:
            assert(self.tickets[DOWN] == 0)
            assert(self.speed[DOWN] == 0)
            assert(self.left == 0)
            return
        # make up a downloaded value based on the tickets it has
        time_delta = time.time() - self.last_commit_time
        this_downloaded = int(ts.get_down_speed(self) * time_delta)
        if this_downloaded >= self.left:
            this_downloaded = self.left
            self.status = "completed"
        self.left -= this_downloaded
        self.downloaded += this_downloaded
        if self.status != "completed":
            # record progress
            try:
                with open(os.path.join(DIR[DOWN], "%s.left" % self.filename), "w") as left_file:
                    left_file.write("%s\n" % self.left)
            except:
                ps.log.exception("Failed in recording progress [%s]" % self.filename)
            return
        # downloading finished
        ps.log.info("COMPLETED [%20s]" % self.name)
        ts.return_tickets(self, DOWN)
        self.up_down = UP
        try:
            shutil.move(os.path.join(DIR[DOWN], self.filename), os.path.join(DIR[UP], self.filename))
        except:
            ps.log.exception("Failed in moving file [%s]" % self.filename)
        try:
            os.remove(os.path.join(DIR[DOWN], "%s.left" % self.filename))
        except:
            ps.log.exception("Failed in removing down_torrent [%s]" % self.filename)

    @property
    def _first_char(self):
        return "&" if "?" in self.link.rpartition("/")[-1] else "?"

    def _get_req_string(self):
        """
        return the query url based on the status of the torrent
        """
        # set up inumwant
        numwant = 0 if self.status == "stopped" else NUMWANT
        req_string =  self.link + self._first_char
        req_string += "info_hash=%s" % self.quoted_infohash
        req_string += "&peer_id=%s" % ps.quoted_peer_id
        req_string += "&port=%s" % ps.port_num
        req_string += "&uploaded=%s" % self.uploaded
        req_string += "&downloaded=%s" % self.downloaded
        req_string += "&left=%s" % self.left
        req_string += "&corrupt=0"
        req_string += "&key=%s" % ps.client_key
        if self.status in ("started", "completed", "stopped"):
            req_string += "&event=%s" % self.status
        req_string += "&numwant=%s" % numwant
        req_string += "&compact=1&no_peer_id=1"
        if ps.use_ipv6 and self.status != "stopped":
            req_string += ps.str_ipv6
        ps.log.debug("REQ_STR [%20s] %s" % (self.name, req_string))
        return req_string

    def _get_scrape_string(self):
        """
        return the query url for scrape
        """
        link_part = list(self.link.rpartition("/"))
        link_part[-1] = link_part[-1].replace("announce", "scrape", 1)
        scrape_link = "".join(link_part)
        req_string =  scrape_link + self._first_char
        req_string += "info_hash=%s" % self.quoted_infohash
        ps.log.debug("SRP_STR [%20s] %s" % (self.name, req_string))
        return req_string

    def _send_message(self, message, method):
        """
        send the lie to tracker
        if success: return True, response
        if failure: return False, {"err_msg" : reason}
        """
        cnt_redir = 0               # count of redirections
        protocol = self.protocol    # "http" or "https"
        host = self.host
        while cnt_redir < REDIRECT_RETRY:
            cnt_tried = 0           # count of retries
            while True:
                if protocol not in Torrent.conns:
                    raise Exception("Weird protocol: %s" % protocol)
                conn = Torrent.conns[protocol](host, timeout=CONNECTION_TIMEOUT)
                # if this is a retry, append a string in output to indicate it
                sRetry = " RETRY %s" % cnt_tried if cnt_tried else ""
                if method == "SCRAPE":
                    ps.log.info("%s [%20s] %s%s" % (method, self.name, protocol, sRetry))
                elif method == "COMMIT":
                    ps.log.info("%s [%20s] UP:%s DOWN:%s LEFT:%s EVENT:%s %s%s" % \
                                   (method, self.name, \
                                    psize(self.uploaded), \
                                    psize(self.downloaded), \
                                    psize(self.left), \
                                    self.status, protocol, sRetry))
                try:
                    conn.putrequest("GET", message, True, True)
                    conn.putheader("Host", host)
                    conn.putheader("User-Agent",      ps.http_headers["User-Agent"])
                    conn.putheader("Accept-Encoding", ps.http_headers["Accept-Encoding"])
                    conn.putheader("Connection",      ps.http_headers["Connection"])
                    conn.endheaders()
                    response = conn.getresponse()
                    status  = response.status
                    headers = response.getheaders()
                    data    = response.read()
                    if status not in (500, 502):
                        conn.close()
                        break
                    # retry when encounters 500, 502, count them as timeout
                    ps.log.error("Internal Server Error [%20s]" % self.name)
                except (socket.timeout, ssl.SSLError):
                    ps.log.error("Timeout [%20s]" % self.name)
                except socket.error:
                    ps.log.error("Socket Error [%20s]" % self.name)
                except httplib.BadStatusLine:
                    ps.log.error("Bad Status Line [%20s]" % self.name)
                conn.close()
                cnt_tried += 1
                if cnt_tried >= REDIRECT_RETRY:
                    # seems like the tracker ignored us
                    return False, {"err_msg": "Timeout Several Times"}
                time.sleep(SLEEP_TIMEOUT)
            if status in (300, 301, 302, 303, 307):
                # handling redirection
                redir_url = None
                for (k, v) in headers:
                    if k.lower() == "location":
                        redir_url = v
                        break
                if redir_url == None:
                    # caught in a bad redirection
                    return False, {"err_msg": "Bad Redirection"}
                # get the new url to visit
                cnt_redir += 1
                protocol, host, message = self._parse_url(redir_url)
                ps.log.debug("REDIRECT [%20s] URL:%s" % (self.name, redir_url))
                continue
            elif status != 200:
                # unsupported HTTP status
                return False, {"err_msg": "Not Supported HTTP Status: %s" % status}
            # 200, succeeded in getting response
            is_gzip_encoded = False
            for (k, v) in headers:
                if k.lower() == "content-encoding" and v.lower() == "gzip":
                    is_gzip_encoded = True
            if is_gzip_encoded:
                # Gzip decoding
                str_compress = StringIO.StringIO(data)
                bencoded_info = gzip.GzipFile(fileobj=str_compress).read()
            else:
                bencoded_info = data
            # B decoding
            try:
                meta_info = bencode.bdecode(bencoded_info)
            except TypeError:
                return False, {"err_msg": "Bad Response Format"}
            return True, meta_info
        return False, {"err_msg": "Too Many Redirections"}

    def _error(self, err_msg, set_status=True):
        ts.return_tickets(self, UP)
        ts.return_tickets(self, DOWN)
        if set_status:
            # if set_status == False, it means we gonna retry for this kind of failure
            self.status = "error"
        ps.log.error("[%s] Reason: %s" % (self.name, err_msg))

    def scrape(self):
        scrape_string = self._get_scrape_string()
        is_success, meta_info = self._send_message(scrape_string, "SCRAPE")
        if not is_success:
            ps.log.warning("SCRAPE: %s" % meta_info["err_msg"])
            return
        if "files" not in meta_info or self.infohash not in meta_info["files"]:
            ps.log.warning("SCRAPE: Bad scrape response")
            return
        meta_info = meta_info["files"][self.infohash]
        if "incomplete" not in meta_info or "complete" not in meta_info:
            ps.log.warning("SCRAPE: Bad scrape response")
            return
        self.up_peers   = int(meta_info["complete"])
        self.down_peers = int(meta_info["incomplete"])
        ps.log.debug("SCARPE [%20s] INCOMPLETE %s COMPLETE %s" % \
                     (self.name, self.down_peers, self.up_peers))

    def commit(self):
        try:
            self._commit()
        except Exception:
            ps.log.exception("Exception in commit")

    def _commit(self):
        if self.status == "started" and self.scrapable:
            self.scrape()
        self._update_status()
        req_string = self._get_req_string()
        is_success, meta_info = self._send_message(req_string, "COMMIT")
        if not is_success:
            self._error(meta_info["err_msg"])
            return
        if self.status == "stopped":
            ps.log.info("RECEIVE [%20s] UP:%s DOWN:%s" % \
                        (self.name, psize(self.uploaded), psize(self.downloaded)))
            return
        # failure reason received
        if "failure reason" in meta_info:
            if meta_info["failure reason"] in CRITICAL_RESPONSES:
                self._error("Server Rejected [%s]" % meta_info["failure reason"])
                return
            self._error("Server Rejected [%s]" % meta_info["failure reason"], False)
            self.last_commit_time = time.time()
            self.next_commit_time = self.last_commit_time + 30*MIN
            return
        elif "interval" not in meta_info:
            # inteval not given
            self._error("Inteval Not Given")
            return
        # interval received, set next_commit_time
        self.last_commit_time = time.time()
        self.next_commit_time = self.last_commit_time + meta_info["interval"]
        if "incomplete" in meta_info and "complete" in meta_info:
            # got overall status from commit response
            self.up_peers   = int(meta_info["complete"])
            self.down_peers = int(meta_info["incomplete"])
        elif self.status == "started" and self.scrapable:
            # already scraped
            pass
        elif self.status != "started" and self.scrapable:
            # scrape supported
            self.scrape()
        elif "peers" in meta_info:
            # just assume [active peer number] = [total peer num] / [a certain rate]
            self.up_peers   = len(meta_info["peers"]) / PEER_UPLOAD_RATE
            self.down_peers = len(meta_info["peers"]) / PEER_DOWNLOAD_RATE
        if (self.up_down == UP and self.down_peers) or \
           (self.up_down == DOWN and self.tickets[DOWN] and self.down_peers > 1):
            self.tickets[UP] = ts.get_tickets(self, UP)
            self.speed[UP] = ts.get_up_speed(self)
        else:
            ts.return_tickets(self, UP)
        if self.up_down == DOWN:
            if self.down_peers > 1 and self.up_peers > 1:
                # fake download only when there is actually someone downloading and someone uploading
                self.tickets[DOWN] = ts.get_tickets(self, DOWN)
                self.speed[DOWN] = ts.get_down_speed(self)
                if self.speed[DOWN]:
                    time_left = self.left / self.speed[DOWN]
                    if time_left < meta_info["interval"]:
                        self.next_commit_time = self.last_commit_time + time_left + 10
            else:
                ts.return_tickets(self, DOWN)
        else:
            # action == upload, should not be downloading
            assert(self.tickets[DOWN] == 0)
            assert(self.speed[DOWN] == 0)
        if self.status in ("started", "completed"):
            self.status = "noevent"
        ps.log.info("RECEIVE [%20s] INT:%s (DOWNPEER:%s UPSPEED:%s/s) (UPPEER:%s DOWNSPEED:%s/s)" % \
                     (self.name, ptime(meta_info["interval"]), \
                      self.down_peers, psize(self.speed[UP]), \
                      self.up_peers, psize(self.speed[DOWN])))

class TorrentMerchant():
    """
    torrent manager
    """
    def __init__(self):
        self.torrents = {}
        self.files = set()
        self.done = False

    def _load_torrents(self):
        """
        reading torrents
        """
        for up_down in (UP, DOWN):
            for f in os.listdir(DIR[up_down]):
                if f in self.files:
                    continue
                self.files |= set([f])
                if not f.lower().endswith(".torrent"):
                    continue
                if not os.path.isfile(os.path.join(DIR[up_down], f)):
                    continue
                try:
                    torrent = Torrent()
                    torrent.load(up_down, f)
                except TypeError:
                    ps.log.error("Bad Torrent Format [%s]" % f)
                    continue
                except Exception:
                    ps.log.exception("Exception in loading torrent [%s]" % f)
                    continue
                torrent_id = torrent.hash()
                if torrent_id in self.torrents:
                    ps.log.warning("Duplicate Torrent [%s]" % f)
                    continue
                self.torrents[torrent_id] = torrent
                ps.log.info("ADD [%20s] SIZE:%s LEFT:%s FOR %s" % \
                        (torrent.name, psize(torrent.size), psize(torrent.left), WORD[up_down]))

    def _fool_around(self):
        thread_box = []
        def _run_threads(box):
            for th in box:
                th.start()
            for th in box:
                threading.Thread.join(th)
        for torrent in filter(lambda t:t.is_ready, self.torrents.values()):
            thread_box.append(threading.Thread(target=torrent.commit))
            if len(thread_box) == CONNECTION_PER_BOX:
                _run_threads(thread_box)
                thread_box = []
                if not ps.no_sleep:
                    time.sleep(SLEEP_THREAD)
        _run_threads(thread_box)
        # calculate committed values
        all_torrents = self.torrents.values()
        sum_uploaded = sum(map(lambda t:t.uploaded, all_torrents))
        sum_downloaded = sum(map(lambda t:t.downloaded, all_torrents))
        if self.done:
            time_elapsed = time.time() - self.time_started
            ps.log.info("Time Elapsed: %s" % ptime(time_elapsed))
            ps.log.info("Total Uploaded: %s"   % psize(sum_uploaded))
            ps.log.info("Total Downloaded: %s" % psize(sum_downloaded))
            ps.log.info("Average Upload Speed: %s/s"   % psize(sum_uploaded / time_elapsed))
            ps.log.info("Average Download Speed: %s/s" % psize(sum_downloaded / time_elapsed))
            ps.log.debug("<= PTLiar ended")
            print "Bye~"
            ptl_exit(0)
        # these are the active ones
        active_torrents = filter(lambda t:t.status != "error", all_torrents)
        # oops, we've got no good torrents
        if len(active_torrents) < 1:
            ptl_error("No torrents available")
        ps.log.info("Time: %s UpSpeed: %s/s DownSpeed: %s/s Committed [Up: %s Down: %s]" % \
                    (ptime(time.time() - self.time_started), \
                     psize(ts.up_speed), psize(ts.down_speed), \
                     psize(sum_uploaded), psize(sum_downloaded)))
        # calculate how long should we sleep
        next_commit = min(map(lambda t:t.next_commit_time, active_torrents))
        time_left = max(0, next_commit - time.time())
        # sleep one more second than needed
        sleep_time = min(SLEEP_SCAN, time_left + 1)
        ps.log.info("Next commit: %s from now, Sleep for %s.." % (ptime(time_left), ptime(sleep_time)))
        print "press [Ctrl+C] to leave"
        try:
            enable_interrupt()
            time.sleep(sleep_time)
            disable_interrupt()
        except (KeyboardInterrupt, SystemExit):
            # gracefully shutdown
            disable_interrupt()
            self.done = True
        if time.time() >= self.time_started + ps.timer:
            # timer
            self.done = True
        if self.done:
            ps.log.info("Stopping...")
            for torrent in active_torrents:
                torrent.status = "stopped"
            return
        # check whether we've got new torrent
        self._load_torrents()

    def fool_around(self):
        """
        fool the trackers around
        """
        try:
            self._load_torrents()
        except Exception:
            ps.log.exception("Exception in fool_around")
            ptl_exit(2)
        self.time_started = time.time()
        try:
            while True:
                self._fool_around()
        except Exception:
            ps.log.exception("Exception in fool_around")

ps = PTLiarSettings()
ts = TicketSeller()
tm = TorrentMerchant()

def _usage():
    print __doc__

def main(argv):
    disable_interrupt()
    try:
        opts, args = getopt.getopt(argv, "c:DefhM:m:nlp:s:t:vz")
    except getopt.GetoptError:
        _usage()
        ptl_exit(2)
    for opt, arg in opts:
        if opt == "-h":
            _usage()
            ptl_exit(0)
        if opt == "-l":
            client_list()
            ptl_exit(0)
        if opt == "-m":
            try:
                ps.max_up_speed = int(arg)*KILO
            except ValueError:
                ps.max_up_speed = -1
            if ps.max_up_speed < 0:
                ptl_error("Maximum upload bandwidth must be a positive integer, in KB/s")
        elif opt == "-M":
            try:
                ps.max_down_speed = int(arg)*KILO
            except ValueError:
                ps.max_down_speed = -1
            if ps.max_down_speed < 0:
                ptl_error("Maximum download bandwidth must be a positive integer, in KB/s")
        elif opt == "-s":
            try:
                ps.max_torrent_speed = int(arg)*KILO
            except ValueError:
                ps.max_torrent_speed = -1
            if ps.max_torrent_speed < 0:
                ptl_error("Maximum torrent speed must be a positive integer, in KB/s")
        elif opt == "-p":
            try:
                ps.port_num = int(arg)
            except ValueError:
                ptl_error("Port number should be an integer")
            if ps.port_num < MIN_PORT_NUM or ps.port_num > MAX_PORT_NUM:
                ptl_error("Port number should be with range (%s, %s)" % (MIN_PORT_NUM, MAX_PORT_NUM))
        elif opt == "-v":
            ps.logging_level = logging.DEBUG
        elif opt == "-e":
            ps.use_ipv6 = True
        elif opt == "-z":
            ps.use_zero_rate = True
        elif opt == "-f":
            ps.no_sleep = True
        elif opt == "-n":
            ps.no_scrape = True
        elif opt == "-c":
            ps.client_id = arg
            if ps.client_id not in BT_CLIENTS:
                BT_CLIENTS[ps.client_id] = {
                    "user-agent" : ps.client_id,
                    "peer-id"    : "-UT"+ps.client_id[9:12]+"0-",
                    "scrape"     : True,
                    "comment"    : "new",
                }
                #ptl_error("Client not in supported client-list, see option -l")
        elif opt == "-t":
            try:
                ps.timer = int(arg)*HOUR
            except ValueError:
                ps.timer = -1
            if ps.timer < 1:
                ptl_error("Timer must be a positive integer, in hours")
    print BANNER % { "version" : __version__, "date" : __date__, "url" : __url__ }
    if not ps.no_sleep:
        time.sleep(SLEEP_BANNER)
    ps.fuck_yourself()
    ts.fuck_yourself()
    if not ps.no_sleep:
        time.sleep(SLEEP_SETTINGS)
    tm.fool_around()

if __name__ == "__main__":
    main(sys.argv[1:])

# -*- coding: utf-8 -*-
#
# PTLiar, a fake seeding software
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

# In most cases, these values are fine..
# Torrent Paths
UP_TORRENT_DIR   = "up_torrents"
DOWN_TORRENT_DIR = "down_torrents"
UP   = 89
DOWN = 64
DIR  = { UP:UP_TORRENT_DIR, DOWN:DOWN_TORRENT_DIR }
WORD = { UP:"uploading",    DOWN:"downloading" }
# Port Number Limits
MAX_PORT_NUM = 65000
MIN_PORT_NUM = 10000
# Number of Connections
CONNECTION_PER_BOX = 4
# "numwant" value that uTorrent prefers
NUMWANT = 200
# Timeout Limit
CONNECTION_TIMEOUT = 40
# Assuming every 20 peers there is one downloading, it's safer to make this number larger
PEER_UPLOAD_RATE = 20
PEER_DOWNLOAD_RATE = 20
# Number of timeout retries
TIMEOUT_RETRY = 3
# Maximum number of redirect retries, avoid loops
REDIRECT_RETRY = 5
# A lucky number
LUCKY_NUMBER = 8964
# Lottery
ALL_TICKETS = 1000000
ZR_LUCK_L1 = 0.3
ZR_LUCK_L2 = 0.3
ZR_LUCK_L3 = 0.95
ZR_GOT_L1  = 0
ZR_GOT_L2  = int(0.3*ALL_TICKETS)
ZR_GOT_L3  = int(0.5*ALL_TICKETS)
# Logging
LOG_NAME = "ptliar"
LOG_FILE = "debug.log"
FMT = "%(asctime)s %(levelname)s: %(message)s"
DATEFMT = "%m-%d %H:%M:%S"
# Default values
DEFAULT_MAX_UP_SPEED   = 2048
DEFAULT_MAX_DOWN_SPEED = 1024
# Max torrent speed
DEFAULT_MAX_TORRENT_SPEED = 400
# Zzz...
SLEEP_BANNER   = 1      # Time of sleep after banner
SLEEP_SETTINGS = 2      # Time of sleep after printing settings
SLEEP_TIMEOUT  = 10     # Time of sleep before another retry
SLEEP_THREAD   = 0.2    # Time of sleep between groups of requests
SLEEP_SCAN     = 60     # Maximum inteval of checking folder change
# Banner
BANNER = """\
====================
FakeTorrent %(version)s (%(date)s)
http://JiajieWu.com/PTMaster/
%(url)s
====================\
"""
# Critical server responses
CRITICAL_RESPONSES = (
    "torrent not registered with this tracker",
)

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

__all__ = ["psize", "ptime", "hex_to_byte", "get_ipv6_addr", "urllib", \
           "ptl_exit", "ptl_error", "disable_interrupt", "enable_interrupt", \
           "KILO", "MEGA", "GIGA", "TERA", "DAY", "HOUR", "MIN", "HEX"]

KILO = 1024
MEGA = KILO*KILO
GIGA = MEGA*KILO
TERA = GIGA*KILO

def pretty_size(b):
    b = float(b)
    for sz, fmt in zip([TERA, GIGA, MEGA, KILO], ["%sTB", "%sGB", "%sMB", "%sKB"]):
        if b >= sz:
            return fmt % round(b / sz, 1)
    return "%sB" % round(b, 1)

MIN  = 60
HOUR = MIN*60
DAY  = HOUR*24

def pretty_time(t):
    t = float(t)
    l = []
    for sz, fmt in zip([DAY, HOUR, MIN], ["%dd", "%dh", "%dm"]):
        if t >= sz:
            r, t = divmod(t, sz)
            l.append(fmt % r)
    l.append("%.2fs" % t)
    return "".join(l)

psize = pretty_size
ptime = pretty_time

def hex_to_byte(h):
    return "".join(map(lambda i:chr(int(h[i:i+2], 16)), range(0, len(h), 2)))

def get_ipv6_addr():
    import socket
    IPV6_SITE = "ipv6.google.com"
    if socket.has_ipv6:
        try:
            s = socket.socket(socket.AF_INET6, socket.SOCK_DGRAM)
            s.connect((IPV6_SITE, 0))
            return s.getsockname()[0]
        except socket.gaierror:
            return None
    return None

def ptl_exit(code):
    import platform, sys, os
    if platform.system() == "Windows":
        os.system("PAUSE")
    sys.exit(code)

def ptl_error(msg):
    import logging
    log = logging.getLogger("ptliar")
    log.critical(msg)
    ptl_exit(2)

import signal

def disable_interrupt():
    """
    disable keyboard interruption
    """
    signal.signal(signal.SIGINT, signal.SIG_IGN)

def enable_interrupt():
    """
    enable keyboard interruption
    """
    signal.signal(signal.SIGINT, signal.default_int_handler)

def fuck_urllib(urllib):
    """
    fuck a standard python library
    """
    # hack : switch urllib.quote to lower case quote
    # this is done for emulating uTorrent
    safe_map = {}
    for i, c in zip(xrange(256), str(bytearray(xrange(256)))):
        safe_map[c] = c if (i < 128 and c in urllib.always_safe) else "%%%02x" % i
    urllib._safe_map[("/", urllib.always_safe)] = safe_map  

import urllib
fuck_urllib(urllib)

HEX = list("0123456789ABCDEF")

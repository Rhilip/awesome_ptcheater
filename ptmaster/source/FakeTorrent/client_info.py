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

BT_CLIENTS = {
    "uTorrent3.0-1" : {
        "user-agent" : "uTorrent/3000(25756)",
        "peer-id"    : "-UT3000-",
        "scrape"     : True,
        "comment"    : "new",
    },
    "uTorrent3.0" : {
        "user-agent" : "uTorrent/3000(25460)",
        "peer-id"    : "-UT3000-",
        "scrape"     : True,
        "comment"    : "new",
    },
#    "uTorrent3.3.2" : {
#        "user-agent" : "uTorrent/3320(26726)",
#        "peer-id"    : "-UT3320-",
#        "scrape"     : True,
#        "comment"    : "very new",
#    },
    "uTorrent2.2.1" : {
        "user-agent" : "uTorrent/2210(25130)",
        "peer-id"    : "-UT2210-",
        "scrape"     : False,
        "comment"    : "stable",
    },
    "uTorrent2.0B" : {
        "user-agent" : "uTorrent/200B(17539)",
        "peer-id"    : "-UT200B-",
        "scrape"     : False,
        "comment"    : "stable",
    },
    "uTorrent1.85" : {
        "user-agent" : "uTorrent/1850(17414)",
        "peer-id"    : "-UT1850-",
        "scrape"     : False,
        "comment"    : "stable",
    },
    "uTorrent1.83" : {
        "user-agent" : "uTorrent183B(14809)",
        "peer-id"    : "-UT183B-",
        "scrape"     : False,
        "comment"    : "deprecated",
    },
    "uTorrent1.61" : {
        "user-agent" : "uTorrent/1610",
        "peer-id"    : "-UT1610-",
        "scrape"     : False,
        "comment"    : "deprecated",
    },
}

DEFAULT_CLIENT = "uTorrent2.2.1"  #old


def client_list():
    print "Supported clients:"
    for client, stuff in sorted(BT_CLIENTS.items(), reverse=True):
        print "    %-15s -    %-20s" % (client, stuff["comment"])

# -*- coding: utf-8 -*-
#
# bencode/bdecode library
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

def _bdecode(s, i):
    """
    bdecode string starting from s[i], return (value, end_index)
    """
    if s[i].isdigit():
        # string : "6:ptliar" => "ptliar"
        size_end  = s.index(":",i)
        str_start = size_end + 1
        str_end   = str_start + int(s[i:size_end])
        return (s[str_start:str_end], str_end)
    elif s[i] is "i":
        # integer : "i8964e" => 8964
        int_end = s.index("e", i)
        return (int(s[i+1:int_end]), int_end+1)
    elif s[i] is "l":
        # list : "li89ei64ee" => [89,64]
        lst = []
        i += 1
        while s[i] is not "e":
            stuff, i = _bdecode(s, i)
            lst.append(stuff)
        return (lst, i+1)
    elif s[i] is "d":
        # dictionary : "d2:pt4:liare" => {"pt":"liar"}
        dct = {}
        i += 1
        while s[i] is not "e":
            if not s[i].isdigit():
                raise TypeError("Invalid bencoded string: key is not string")
            key,   i = _bdecode(s, i)
            value, i = _bdecode(s, i)
            dct.update({key:value})
        return (dct, i+1)
    raise TypeError("Invalid bencoded string: unknown object type")

def bdecode(s):
    """
    bdecode a bencoded string. raise TypeError if it's invalid
    """
    try:
        return _bdecode(s, 0)[0]
    except:
        raise TypeError("Invalid bencoded string.")

def bencode(o):
    """
    bencode an object. raise TypeError if it's invalid
    """
    t = type(o)
    if t is str:
        return "%d:%s" % (len(o), o)
    elif t in (int, long):
        return "i%de" % o
    elif t is list:
        return "l%se" % "".join(map(bencode, o))
    elif t is dict:
        keys = sorted(o.keys())
        for key in keys:
            if type(key) is not str:
                raise TypeError("Invalid object to bencode")
        return "d%se" % "".join(map(lambda k:bencode(k)+bencode(o[k]), keys))
    raise TypeError("Invalid object to bencode")
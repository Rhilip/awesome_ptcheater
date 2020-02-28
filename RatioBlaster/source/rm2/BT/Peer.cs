namespace rm2
{
    using System;
    using System.Net;

    public class Peer
    {
        public IPAddress IpAddress;
        public string Peer_ID;
        public ushort Port;

        public Peer(byte[] ip, short port)
        {
            this.Peer_ID = "";
            this.IpAddress = new IPAddress(ip);
            this.Port = (ushort) IPAddress.NetworkToHostOrder(port);
            this.Peer_ID = "";
        }

        public Peer(string ip, string port, string peer_id)
        {
            this.Peer_ID = "";
            try
            {
                this.IpAddress = IPAddress.Parse(ip);
                this.Port = (ushort) IPAddress.NetworkToHostOrder(short.Parse(port));
                this.Peer_ID = peer_id;
            }
            catch (Exception)
            {
            }
        }

        public override string ToString()
        {
            if (this.Peer_ID.Length > 0)
            {
                return (this.IpAddress.ToString() + " : " + this.Port.ToString() + "(PeerID=" + this.Peer_ID + ")");
            }
            return (this.IpAddress.ToString() + " : " + this.Port.ToString());
        }
    }
}


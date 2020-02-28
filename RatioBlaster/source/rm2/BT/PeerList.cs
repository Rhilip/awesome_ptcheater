namespace rm2
{
    using System;
    using System.Collections.Generic;

    public class PeerList : List<Peer>
    {
        //moved to rm2.Program
        //public int maxPeersToShow = 5;
        public int peerCounter;

        public override string ToString()
        {
            string str = "";
            str = "(" + this.Count.ToString() + ") \r\n";
            
            foreach (Peer peer in this)
            {
                if (this.peerCounter < rm2.Program.settings.MaxNumOfPeersToShow)
                {
                    str = str + peer.ToString() + "\r\n";
                }
                this.peerCounter++;
            }
            
            return str;
        }
    }
}


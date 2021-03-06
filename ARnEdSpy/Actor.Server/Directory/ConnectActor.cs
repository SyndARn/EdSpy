﻿/*****************************************************************************
		               ARnActor Actor Model Library .Net
     
	 Copyright (C) {2015}  {ARn/SyndARn} 
 
 
     This program is free software; you can redistribute it and/or modify 
     it under the terms of the GNU General Public License as published by 
     the Free Software Foundation; either version 2 of the License, or 
     (at your option) any later version. 
 
 
     This program is distributed in the hope that it will be useful, 
     but WITHOUT ANY WARRANTY; without even the implied warranty of 
     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the 
     GNU General Public License for more details. 
 
 
     You should have received a copy of the GNU General Public License along 
     with this program; if not, write to the Free Software Foundation, Inc., 
     51 Franklin Street, Fifth Floor, Boston, MA 02110-1301 USA. 
*****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actor.Base;

namespace Actor.Server
{
    /// <summary>
    /// Connect to a shard Service
    /// </summary>
    public class ConnectActor : BaseActor
    {
        private string fServiceName;
        private string fUri;
        private IActor fSender;
        public ConnectActor(IActor lSender, string hostAddress, string serviceName)
            : base()
        {
            fUri = hostAddress;
            fServiceName = serviceName;
            fSender = lSender;
            Become(new Behavior<string>(DoDisco));
            SendMessage("DoConnect");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1806:DoNotIgnoreMethodResults", MessageId = "Actor.Server.ConnectActor")]
        public static void Connect(IActor lSender, string hostAddress, string serviceName)
        {
            new ConnectActor(lSender, hostAddress, serviceName);
        }

        private void DoDisco(string msg)
        {
            Become(new Behavior<Dictionary<string,string>>(Found));
            BaseActor rem = new RemoteActor(new ActorTag(fUri));
            rem += new DiscoCommand(this); // send message
        }

        private void Found(Dictionary<string,string> someServices)
        {
            string service;
            if (someServices.TryGetValue(fServiceName, out service))
            {
                if (!string.IsNullOrEmpty(service))
                {
                    ActorTag tag = new ActorTag(service);
                    Become(new Behavior<ActorTag>(DoConnect));
                    SendMessage(tag);
                }
                else
                // service with no end point
                {
                    Become(null);
                }
            }
            else
            // not found
            {
                Become(null);
            }
        }

        private void DoConnect(ActorTag tag)
        {
            IActor remoteSend = new RemoteActor(tag);
            fSender.SendMessage(new Tuple<string, ActorTag, IActor>(fServiceName, tag, remoteSend));
        }

    }

}

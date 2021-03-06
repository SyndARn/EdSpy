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
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Actor.Base
{

    class ActorMailBox : ActorMailBox<Object>
    {
    }

    /// <summary>
    /// ActorMailBox
    /// </summary>
    class ActorMailBox<T> 
    {
        private ConcurrentQueue<T> fQueue = new ConcurrentQueue<T>(); // all actors may push here, only this one may dequeue
        private ConcurrentQueue<T> fMissed = new ConcurrentQueue<T>(); // only this one use it in run mode
        
        public ActorMailBox()
        {
        }

        public void AddMiss(T aMessage)
        {
            fMissed.Enqueue(aMessage);
        }

        public int RefreshFromMissed()
        {
            int i = 0;
            T val = default(T);
            while (fMissed.TryDequeue(out val))
            {
                fQueue.Enqueue(val) ;
                i++ ;
            }
            return i;
        }

        public void AddMessage(T aMessage)
        {
            fQueue.Enqueue(aMessage);
        }

        public T GetMessage()
        {
            T val = default(T);
            fQueue.TryDequeue(out val) ;
            return val;
        }

    }

}

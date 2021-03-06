﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Actor.Base
{

    public interface ISupervisedActor : IActor
    {
        ISupervisedActor Respawn();
    }


    public class SupervisedActor : BaseActor, ISupervisedActor
    {
        public virtual ISupervisedActor Respawn()
        {
            ISupervisedActor actor = new SupervisedActor(this.Tag);
            return actor;
        }
        public SupervisedActor(ActorTag previousTag) : base(previousTag)
        {
            Become(new bhvSupervisedBehavior());
        }
        public SupervisedActor() : base()
        {
            Become(new bhvSupervisedBehavior());
        }
    }

    public enum SupervisorAction { Register, Unregister, Respawn, Kill} ;

    public class bhvSupervisedBehavior : Behavior<SupervisorAction>
    {
        public bhvSupervisedBehavior()
        {
            Pattern =  t =>{return true;} ;
            Apply = t =>
             {
                 if (t.Equals(SupervisorAction.Kill))
                 {
                     LinkedActor.SendMessage(SystemMessage.NullBehavior);
                 }
             };
        }
    }

    public class SupervisorActor : BaseActor
    {
        public SupervisorActor() : base (new SupervisorBehavior())
        {

        }
    }

    public class SupervisorBehavior : Behaviors
    {

        private List<ISupervisedActor> fSupervised = new List<ISupervisedActor>();

        public SupervisorBehavior() : base()
        {
            this.AddBehavior(new Behavior<Tuple<SupervisorAction, ISupervisedActor>>(
                DoSupervision
                )) ;
        }

        private void DoSupervision(Tuple<SupervisorAction, ISupervisedActor> msg)
        {
            switch(msg.Item1)
            {
                case SupervisorAction.Register: 
                    {
                        fSupervised.Add(msg.Item2);
                        break; 
                    }
                case SupervisorAction.Unregister:
                    {
                        fSupervised.Remove(msg.Item2);
                        break;
                    }
                case SupervisorAction.Respawn:
                    {
                        // how to relaunch this actor ?
                        fSupervised.Remove(msg.Item2);
                        // create actor
                        var newactor = msg.Item2.Respawn();
                        fSupervised.Add(newactor);
                        break;
                    }
            }
        }
    }
}

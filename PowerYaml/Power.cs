using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using PowerYaml.Structure;
using System;
using System.Collections.Generic;

namespace PowerYaml
{
    public class Power
    {
        public Dictionary<string, Action<Player, Dictionary<string, dynamic>>> CoreExecutors { get; set; }
        public Dictionary<PlayerEvent, List<IAction>> Events { get; set; }
        public Power()
        {
            Events = new();
            CoreExecutors = new();
        }
        public void AddCoreExecutor(string Name, Action<Player, Dictionary<string, dynamic>> Action)
        {
            if (Name is not null && Action is not null)
            {
                CoreExecutors.Add(Name, Action);
            }
        }
        public void RemoveCoreExecutor(string Name)
        {
            CoreExecutors.Remove(Name);
        }
        public void RegisterEvent(PlayerEvent Event, IAction Action)
        {
            if (Action is not null)
            {
                if (Events.ContainsKey(Event))
                {
                    Events[Event].Add(Action);
                }
                else
                {
                    Events[Event] = new()
                    {
                        Action
                    };
                }
            }
        }
        public List<ActionStatus> Execute(PlayerEvent Event, IPlayerEvent EventArgs)
        {
            List<ActionStatus> Status = new();
            if (Events.ContainsKey(Event))
            {
                foreach (IAction Action in Events[Event])
                {
                    Status.Add(Action.Execute(EventArgs.Player, EventArgs, this));
                }
            }
            return Status;
        }
    }
}

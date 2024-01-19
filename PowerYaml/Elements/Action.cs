using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using PowerYaml.Structure;
using System.Collections.Generic;

namespace PowerYaml.Elements
{
    public class Action : IAction
    {
        public List<Condition> Conditions { get; set; } = new();
        public bool Allowed { get; set; } = true;
        public List<Executor> Actions { get; set; } = new();
        public Action() { }
        public Action(List<Condition> conditions, bool allowed, List<Executor> actions)
        {
            Conditions = conditions;
            Allowed = allowed;
            Actions = actions;
        }

        public ActionStatus Execute(Player Player, IPlayerEvent EventArgs, Power PowerHub)
        {
            {
                bool ConditionsStatus = true;
                foreach (Condition Condition in Conditions)
                {
                    ConditionsStatus = ConditionsStatus && Condition.Execute(Player, EventArgs);
                }
                if (!ConditionsStatus)
                {
                    return ActionStatus.Rejected;
                }

                if (!Allowed)
                {
                    if (EventArgs is IDeniableEvent)
                    {
                        ((IDeniableEvent)EventArgs).IsAllowed = false;
                    }
                    return ActionStatus.AllowedAndStop;
                }
                return ActionStatus.AllowedAndNotStop;
            }
        }
    }
}

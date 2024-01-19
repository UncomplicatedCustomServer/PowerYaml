using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using PowerYaml.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerYaml.Structure
{
    public interface IAction
    {
        public abstract List<Condition> Conditions { get; set; }
        public abstract bool Allowed { get; set; }
        public abstract List<Executor> Actions { get; set; }
        public abstract ActionStatus Execute(Player Player, IPlayerEvent EventArgs, Power PowerHub);
    }
}

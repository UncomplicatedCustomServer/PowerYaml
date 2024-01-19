using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerYaml.Structure
{
#nullable enable
    internal interface IExecutor
    {
        public abstract dynamic Collection { get; set; }
        public abstract string Action { get; set; }
        public abstract dynamic? Args { get; set; }
        public abstract void Execute(Player Player, object EventArgs, Power PowerHub);
    }
}

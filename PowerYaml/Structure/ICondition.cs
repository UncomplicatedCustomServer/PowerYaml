using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerYaml.Structure
{
#nullable enable
    internal interface ICondition
    {
        public abstract ConditionType Type { get; set; }
        public abstract string? Element { get; set; }
        public abstract ComparisonType ShouldBe { get; }
        public abstract dynamic Value { get; set; }
        public bool Execute(Player Player, object EventData);
    }
}
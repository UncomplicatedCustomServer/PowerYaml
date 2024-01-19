using Exiled.API.Features;
using PowerYaml.Helper;
using PowerYaml.Structure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace PowerYaml.Elements
{
#nullable enable
    public class Executor : IExecutor
    {
        public dynamic Collection { get; set; } = CollectionType.Core;
        public string Action { get; set; } = "Kill";
        public dynamic? Args { get; set; } = null;
        public Executor() { }
        public Executor(dynamic collection, string action, dynamic args)
        {
            Collection = collection;
            Action = action;
            Args = args;
        }
        public void Execute(Player Player, object EventArgsExt, Power PowerHub)
        {
            if (Collection is not CollectionType && Collection is not string)
            {
                return;
            }
            MethodInfo? MethodInfo = null;
            dynamic? ExecutorObject = null;
            if (Collection is not CollectionType)
            {
                if (Collection.Contains(".")) 
                {
                    return;
                }
                Dictionary<string, dynamic> EventArgs = PlayerHelper.DecodeObject(EventArgsExt);
                if (!EventArgs.ContainsKey(Collection))
                {
                    return;
                }
                if (EventArgs[Collection] is not object) 
                {
                    return;
                }
                if (EventArgs[Collection] is object Object)
                {
                    MethodInfo = Object.GetType().GetMethod(Action);
                    ExecutorObject = Object;
                }
                return;
            }

            switch (Collection)
            {
                case CollectionType.Core:
                    if (PowerHub.CoreExecutors.ContainsKey(Action))
                    {
                        MethodInfo = PowerHub.CoreExecutors[Action].Method;
                        ExecutorObject = PowerHub.CoreExecutors[Action];
                    }
                    break;
                case CollectionType.Player:
                    MethodInfo = Player.GetType().GetMethod(Action);
                    ExecutorObject = Player;
                    break;
            }


            if (MethodInfo is not null && Args is not null && ExecutorObject is not null)
            {
                if (ExecutorObject is Action<Player, Dictionary<string, dynamic>>)
                {
                    if (Args is Dictionary<string, dynamic> && ExecutorObject is Action<Player, Dictionary<string, dynamic>>)
                    {
                        ExecutorObject.Invoke(Player, Args);
                        return;
                    }
                }
                List<object> NewArgs = new();
                Dictionary<string, dynamic?> NewArgsExt = new();
                if (Args is not List<dynamic?>)
                {
                    return;
                }
                Args = (List<dynamic?>)Args;
                foreach (ParameterInfo ParamInfo in MethodInfo.GetParameters())
                {
                    if (!ParamInfo.IsOptional && Args.Count() > ParamInfo.Position + 1)
                    {
                        return;
                    }
                    else if (Args.Count() > ParamInfo.Position + 1)
                    {
                        NewArgsExt.Add(ParamInfo.Name, null);
                        NewArgs.Add(string.Empty);
                        continue;
                    }
                    // Check for the type
                    if (ParamInfo.ParameterType != Args[ParamInfo.Position].GetType())
                    {
                        NewArgsExt.Add(ParamInfo.Name, TypeDescriptor.GetConverter(ParamInfo.ParameterType).ConvertFrom(Args[ParamInfo.Position]));
                        NewArgs.Add(TypeDescriptor.GetConverter(ParamInfo.ParameterType).ConvertFrom(Args[ParamInfo.Position]));
                    }
                }
                MethodInfo.Invoke(ExecutorObject, NewArgs.ToArray());
            }
        }
    }
}

using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Rendering;
using UnityEngine.SocialPlatforms;

namespace PowerYaml.Helper
{
    public class PlayerHelper
    {
        public static Dictionary<string, dynamic> DecodeObject(object Player)
        {
            Dictionary<string, dynamic> Data = new();
            foreach (PropertyInfo info in Player.GetType().GetProperties())
            {
                Data.Add(info.Name, info.GetValue(Player, null));
            }
            return Data;
        }

        public static Action CreateAction(Action Action)
        {
            return Action;
        }
    }
}

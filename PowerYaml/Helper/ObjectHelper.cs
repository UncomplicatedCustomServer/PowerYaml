using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerYaml.Helper
{
#nullable enable
    public class ObjectHelper
    {
        public static dynamic? TryGetProperty(object Object, string Name)
        {
            return TryGetProperty(PlayerHelper.DecodeObject(Object), Name);
        }
        public static dynamic? TryGetProperty(Dictionary<string, dynamic> InternalObject, string Name)
        {
            if (InternalObject == null)
            {
                return null;
            }
            List<string> Path;
            if (Name.Contains("."))
            {
                Path = new(Name.Split('.'));
            }
            else
            {
                Path = new()
                {
                    Name
                };
            }
            dynamic? Value = InternalObject;
            foreach (string Element in Path)
            {
                if (Value.ContainsKey(Element))
                {
                    Value = Value[Element];
                    if (Value is object && Value.GetType() != typeof(Dictionary<string, dynamic>))
                    {
                        Value = PlayerHelper.DecodeObject(Value);
                    }
                    if (Value is not object)
                    {
                        return Value;
                    }
                    Path.Remove(Element);
                    if (Path.Count == 0)
                    {
                        return Value;
                    }
                }
            }
            return null;
        }

        public static bool ValidateType(Type Type, dynamic Var)
        {
            if (Var.GetType() == Type)
            {
                return true;
            }
            if (Var.GetType == typeof(int) && Type == typeof(string))
            {
                return true;
            }
            return false;
        }
    }
}

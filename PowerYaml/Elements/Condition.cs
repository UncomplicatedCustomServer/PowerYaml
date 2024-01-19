using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using PowerYaml.Helper;
using PowerYaml.Structure;
using System;
using System.Collections;
using System.Collections.Generic;

namespace PowerYaml.Elements
{
#nullable enable
    public class Condition : ICondition
    {
        public ConditionType Type { get; set; } = ConditionType.Team;
        public string? Element { get; set; } = null;
        public ComparisonType ShouldBe { get; set; } = ComparisonType.ContainedIn;
        public dynamic Value { get; set; } = new List<Team>();
        public Condition() { }
        public Condition(ConditionType type, string? element, ComparisonType shouldBe, dynamic value)
        {
            Type = type;
            Element = element;
            ShouldBe = shouldBe;
            Value = value;
        }

        public bool Execute(Player Player, object EventData)
        {
            if (ShouldBe != ComparisonType.ContainedIn)
            {
                if (!ObjectHelper.ValidateType(Type.GetType(), Value))
                {
                    return false;
                }
            }
            else
            {
                if (Value is not IList)
                {
                    return false;
                }
                if (Value.GetType().GetGenericArguments()[0] != Type.GetType())
                {
                    return false;
                }
            }

            dynamic? CompareTerm = null;

            switch (Type)
            {
                case ConditionType.Team:
                    CompareTerm = Player.Role.Team;
                    break;
                case ConditionType.Role:
                    CompareTerm = Player.Role;
                    break;
                case ConditionType.Nickname:
                    CompareTerm = Player.Nickname;
                    break;
                case ConditionType.SteamId:
                    CompareTerm = Player.UserId;
                    break;
                case ConditionType.Group:
                    CompareTerm = Player.Group;
                    break;
                case ConditionType.Permission:
                    if (Value is not string && Value is not float && Value is not int)
                    {
                        break;
                    }
                    if (Value is not string)
                    {
                        Value = Value.ToString();
                    }
                    return Permissions.CheckPermission(Player, Value);
                case ConditionType.CustomPlayer:
                    if (Element == null)
                    {
                        break;
                    }
                    CompareTerm = ObjectHelper.TryGetProperty(Player, Element);
                    break;
                case ConditionType.CustomExternal:
                    if (Element == null)
                    {
                        break;
                    }
                    string SearchFirstKey;
                    if (Element.Contains("."))
                    {
                        SearchFirstKey = Element.Split('.')[0];
                    }
                    else
                    {
                        SearchFirstKey = Element;
                    }
                    if (PlayerHelper.DecodeObject(EventData).ContainsKey(SearchFirstKey))
                    {
                        CompareTerm = ObjectHelper.TryGetProperty(PlayerHelper.DecodeObject(EventData), Element);
                    }
                    break;
            }

            if (CompareTerm == null)
            {
                return false;
            }

            switch (ShouldBe)
            {
                case ComparisonType.EqualThan:
                    return CompareTerm == Value;
                case ComparisonType.GreaterThan:
                    return CompareTerm > Value;
                case ComparisonType.LessThan:
                    return CompareTerm < Value;
                case ComparisonType.GreaterOrEqualThan:
                    return CompareTerm >= Value;
                case ComparisonType.LessOrEqualThan: 
                    return CompareTerm <= Value;
                case ComparisonType.Not:
                    return CompareTerm != Value;
                case ComparisonType.Contained:
                    if (!(CompareTerm is string && (Value is string || Value is char))) {
                        return false;
                    }
                    return CompareTerm.Contains(Value);
                case ComparisonType.True:
                    if (!CompareTerm is bool)
                    {
                        return false;
                    }
                    return CompareTerm;
                case ComparisonType.False:
                    if (!CompareTerm is bool)
                    {
                        return false;
                    }
                    return !CompareTerm;
            }

            return false;
        }
    }
}

using System.Collections;
using System.Reflection;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helpers;

public abstract class CommandOptionsObjectArgumentParser
{
    public static void AddArgumentsFromOptionsObject(List<string> parsedArgs, object optionsArgumentsObject)
    {
        foreach (var propertyInfo in optionsArgumentsObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            var propertyValues = GetValues(optionsArgumentsObject, propertyInfo)?.ToList();

            if (propertyValues is null || !propertyValues.Any())
            {
                continue;
            }

            AddSwitches(parsedArgs, propertyValues, propertyInfo);
        }
    }

    private static void AddSwitches(List<string> parsedArgs, List<string> propertyValues, PropertyInfo propertyInfo)
    {
        foreach (var propertyValue in propertyValues)
        {
            if (TryAddBooleanSwitch(parsedArgs, propertyInfo, propertyValue))
            {
                continue;
            }

            AddValueSwitch(parsedArgs, propertyInfo, propertyValue);
        }
    }

    private static void AddValueSwitch(ICollection<string> parsedArgs, MemberInfo propertyInfo, string propertyValue)
    {
        var commandSwitchAttribute = propertyInfo.GetCustomAttribute<CommandSwitchAttribute>(true);

        if (commandSwitchAttribute == null)
        {
            return;
        }

        var precedingHyphens = new string('-', commandSwitchAttribute.HyphenCount);
        var @switch = $"{precedingHyphens}{commandSwitchAttribute.SwitchName}";

        if (commandSwitchAttribute.SwitchValueSeparator == " ")
        {
            parsedArgs.Add(@switch);
            parsedArgs.Add(propertyValue);
        }
        else
        {
            parsedArgs.Add($"{@switch}{commandSwitchAttribute.SwitchValueSeparator}{propertyValue}");
        }
    }

    private static bool TryAddBooleanSwitch(List<string> parsedArgs, PropertyInfo propertyInfo, string propertyValue)
    {
        var booleanCommandSwitchAttribute = propertyInfo.GetCustomAttribute<BooleanCommandSwitchAttribute>();

        if (booleanCommandSwitchAttribute is not null &&
            bool.TryParse(propertyValue, out var boolValue)
            && boolValue)
        {
            var hyphens = new string('-', booleanCommandSwitchAttribute.HyphenCount);
            parsedArgs.Add($"{hyphens}{booleanCommandSwitchAttribute.SwitchName}");
            return true;
        }

        return false;
    }

    private static IEnumerable<string>? GetValues(object optionsArgumentsObject, PropertyInfo propertyInfo)
    {
        var rawValue = propertyInfo.GetValue(optionsArgumentsObject);

        return GetValues(rawValue);
    }

    private static IEnumerable<string>? GetValues(object? rawValue)
    {
        if (rawValue is null)
        {
            yield break;
        }

        if (rawValue is string stringValue)
        {
            yield return stringValue;
            yield break;
        }

        if (rawValue is bool boolValue)
        {
            yield return boolValue.ToString().ToLowerInvariant();
            yield break;
        }

        if (rawValue
            is byte
            or short
            or ushort
            or int
            or uint
            or long
            or ulong
            or float
            or double
            or decimal)
        {
            yield return rawValue.ToString()!;
            yield break;
        }

        if (rawValue.GetType().IsEnum)
        {
            yield return ParseEnum(rawValue);
        }

        if (rawValue is IEnumerable enumerable)
        {
            foreach (var obj in enumerable.Cast<object>().SelectMany(GetValues!))
            {
                yield return obj;
            }
            
            yield break;
        }

        if (rawValue is Uri uri)
        {
            yield return ParseUri(uri);
        }

        yield return rawValue.ToString()!;
    }

    private static string ParseEnum(object rawValue)
    {
        var enumValueAttribute = rawValue
            .GetType()
            .GetField(rawValue.ToString()!)
            ?.GetCustomAttribute<EnumValueAttribute>();

        return enumValueAttribute is not null 
            ? enumValueAttribute.Value 
            : rawValue.ToString()!;
    }

    private static string ParseUri(Uri uri)
    {
        return uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.ToString();
    }
}
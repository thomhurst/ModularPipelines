using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ModularPipelines.Engine.Attributes;

internal static class CustomAttributeMetadata
{
    public static IReadOnlyList<CustomAttributeData> GetApplicable(
        Type type,
        Func<Type, bool> predicate)
    {
        var result = new List<CustomAttributeData>();
        var inheritedNonMultipleTypes = new HashSet<Type>();
        for (var current = type; current is not null; current = current.BaseType)
        {
            foreach (var data in current.CustomAttributes.Where(data => predicate(data.AttributeType)))
            {
                var usage = data.AttributeType.GetCustomAttribute<AttributeUsageAttribute>(inherit: true);
                if (current != type && !(usage?.Inherited ?? true))
                {
                    continue;
                }

                if (!(usage?.AllowMultiple ?? false)
                    && !inheritedNonMultipleTypes.Add(data.AttributeType))
                {
                    continue;
                }

                result.Add(data);
            }
        }

        return result;
    }

    public static T Create<T>(CustomAttributeData data)
    {
        var constructorArguments = data.ConstructorArguments
            .Select(ConvertArgument)
            .ToArray();
        var attribute = (T) data.Constructor.Invoke(constructorArguments);
        foreach (var namedArgument in data.NamedArguments)
        {
            var value = ConvertArgument(namedArgument.TypedValue);
            if (namedArgument.IsField)
            {
                ((FieldInfo) namedArgument.MemberInfo).SetValue(attribute, value);
            }
            else
            {
                ((PropertyInfo) namedArgument.MemberInfo).SetValue(attribute, value);
            }
        }

        return attribute;
    }

    [UnconditionalSuppressMessage(
        "Aot",
        "IL3050",
        Justification = "Attribute array types are statically present in the inspected custom-attribute metadata.")]
    private static object? ConvertArgument(CustomAttributeTypedArgument argument)
    {
        if (argument.Value is IReadOnlyCollection<CustomAttributeTypedArgument> values
            && argument.ArgumentType.GetElementType() is { } elementType)
        {
            var array = Array.CreateInstance(elementType, values.Count);
            var index = 0;
            foreach (var value in values)
            {
                array.SetValue(ConvertArgument(value), index++);
            }

            return array;
        }

        return argument.ArgumentType.IsEnum && argument.Value is not null
            ? Enum.ToObject(argument.ArgumentType, argument.Value)
            : argument.Value;
    }
}

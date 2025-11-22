using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace ModularPipelines.Attributes;

public static class ExcludeFromCodeCoverageAttributeChanger
{
#pragma warning disable CA2255
    [ModuleInitializer]
#pragma warning restore CA2255
    public static void SetInheritedTrue()
    {
        var attribute = typeof(ExcludeFromCodeCoverageAttribute);

        var attributeUsage = attribute.GetCustomAttribute<AttributeUsageAttribute>()!;

        attributeUsage.Inherited = true;
    }
}

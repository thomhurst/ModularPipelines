using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc />
internal sealed class ToolResolver : IToolResolver
{
    /// <inheritdoc />
    public string? ResolveTool(Type optionsType)
    {
        // Walk the inheritance chain looking for [CliTool]
        var currentType = optionsType;
        while (currentType != null && currentType != typeof(object))
        {
            var attribute = currentType.GetCustomAttribute<CliToolAttribute>(inherit: false);
            if (attribute != null)
            {
                return attribute.Tool;
            }

            currentType = currentType.BaseType;
        }

        return null;
    }

    /// <inheritdoc />
    public string? ResolveTool(CommandLineToolOptions options)
    {
        return options.Tool ?? ResolveTool(options.GetType());
    }
}

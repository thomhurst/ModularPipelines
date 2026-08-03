using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandPartsProvider : ICommandPartsProvider
{
    /// <inheritdoc/>
    public IReadOnlyList<string> GetRawCommandParts(object optionsObject)
    {
        if (optionsObject is CommandLineToolOptions { CommandParts: not null } commandLineToolOptions)
        {
            return commandLineToolOptions.CommandParts;
        }

        var type = optionsObject.GetType();

        var preferredAlias = type.GetCustomAttributes<CliCommandAliasAttribute>()
            .FirstOrDefault(a => a.IsPreferred);

        if (preferredAlias is not null)
        {
            return preferredAlias.CommandParts;
        }

        var cliSubCommandAttribute = type.GetCustomAttribute<CliSubCommandAttribute>();
        if (cliSubCommandAttribute is not null)
        {
            return cliSubCommandAttribute.SubCommands;
        }

        return [];
    }
}

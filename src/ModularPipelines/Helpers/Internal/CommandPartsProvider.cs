using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

/// <inheritdoc/>
internal sealed class CommandPartsProvider : ICommandPartsProvider
{
    /// <inheritdoc/>
    public List<string> GetRawCommandParts(object optionsObject)
    {
        if (optionsObject is CommandLineToolOptions { CommandParts: not null } commandLineToolOptions)
        {
            return commandLineToolOptions.CommandParts.ToList();
        }

        var type = optionsObject.GetType();

        // Try new CliCommand attribute first
        // Check for preferred alias first
        var preferredAlias = type.GetCustomAttributes<CliCommandAliasAttribute>()
            .FirstOrDefault(a => a.IsPreferred);

        if (preferredAlias is not null)
        {
            return preferredAlias.CommandParts.ToList();
        }

        // Prefer the explicit CliSubCommand attribute if it exists (for classes that inherit tool from base)
        var cliSubCommandAttribute = type.GetCustomAttribute<CliSubCommandAttribute>();
        if (cliSubCommandAttribute is not null)
        {
            return cliSubCommandAttribute.SubCommands.ToList();
        }

        // Fall back to full CliCommand attribute (defines both tool and subcommands)
        var cliCommandAttribute = type.GetCustomAttribute<CliCommandAttribute>();
        if (cliCommandAttribute is not null)
        {
            // Only return SubCommands, not the tool name (which is already used by Cli.Wrap)
            return cliCommandAttribute.SubCommands.ToList();
        }

        return new List<string>();
    }
}

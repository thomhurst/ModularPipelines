using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("access-context-manager", "authorized-orgs", "update")]
public record GcloudAccessContextManagerAuthorizedOrgsUpdateOptions(
[property: PositionalArgument] string AuthorizedOrgsDesc,
[property: PositionalArgument] string Policy
) : GcloudOptions
{
    [CommandSwitch("--add-orgs")]
    public string[]? AddOrgs { get; set; }

    [BooleanCommandSwitch("--clear-orgs")]
    public bool? ClearOrgs { get; set; }

    [CommandSwitch("--remove-orgs")]
    public string[]? RemoveOrgs { get; set; }

    [CommandSwitch("--set-orgs")]
    public string[]? SetOrgs { get; set; }
}
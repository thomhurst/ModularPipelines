using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("access-context-manager", "authorized-orgs", "update")]
public record GcloudAccessContextManagerAuthorizedOrgsUpdateOptions(
[property: CliArgument] string AuthorizedOrgsDesc,
[property: CliArgument] string Policy
) : GcloudOptions
{
    [CliOption("--add-orgs")]
    public string[]? AddOrgs { get; set; }

    [CliFlag("--clear-orgs")]
    public bool? ClearOrgs { get; set; }

    [CliOption("--remove-orgs")]
    public string[]? RemoveOrgs { get; set; }

    [CliOption("--set-orgs")]
    public string[]? SetOrgs { get; set; }
}
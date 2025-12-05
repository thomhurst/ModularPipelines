using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "update")]
public record GcloudIdentityGroupsUpdateOptions(
[property: CliArgument] string Email
) : GcloudOptions
{
    [CliOption("--dynamic-user-query")]
    public string? DynamicUserQuery { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }

    [CliFlag("--clear-description")]
    public bool? ClearDescription { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--clear-display-name")]
    public bool? ClearDisplayName { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}
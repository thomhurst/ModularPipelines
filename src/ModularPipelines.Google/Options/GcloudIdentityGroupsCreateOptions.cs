using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identity", "groups", "create")]
public record GcloudIdentityGroupsCreateOptions(
[property: CliArgument] string Email,
[property: CliOption("--customer")] string Customer,
[property: CliOption("--organization")] string Organization
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--dynamic-user-query")]
    public string? DynamicUserQuery { get; set; }

    [CliOption("--with-initial-owner")]
    public string? WithInitialOwner { get; set; }

    [CliOption("--group-type")]
    public string? GroupType { get; set; }

    [CliFlag("discussion")]
    public bool? Discussion { get; set; }

    [CliFlag("dynamic")]
    public bool? Dynamic { get; set; }

    [CliFlag("security")]
    public bool? Security { get; set; }

    [CliOption("--labels")]
    public string? Labels { get; set; }
}
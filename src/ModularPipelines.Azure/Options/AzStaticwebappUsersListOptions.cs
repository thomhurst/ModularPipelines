using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("staticwebapp", "users", "list")]
public record AzStaticwebappUsersListOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--authentication-provider")]
    public string? AuthenticationProvider { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}
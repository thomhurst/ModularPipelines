using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("staticwebapp", "backends", "unlink")]
public record AzStaticwebappBackendsUnlinkOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliFlag("--remove-backend-auth")]
    public bool? RemoveBackendAuth { get; set; }
}
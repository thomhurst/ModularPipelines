using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "environment-type", "create")]
public record AzDevcenterAdminEnvironmentTypeCreateOptions(
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--environment-type-name")] string EnvironmentTypeName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}
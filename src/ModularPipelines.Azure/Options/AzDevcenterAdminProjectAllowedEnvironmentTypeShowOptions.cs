using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "project-allowed-environment-type", "show")]
public record AzDevcenterAdminProjectAllowedEnvironmentTypeShowOptions : AzOptions
{
    [CliOption("--environment-type-name")]
    public string? EnvironmentTypeName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
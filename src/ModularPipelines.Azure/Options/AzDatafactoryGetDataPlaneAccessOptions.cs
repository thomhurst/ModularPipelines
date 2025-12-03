using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "get-data-plane-access")]
public record AzDatafactoryGetDataPlaneAccessOptions : AzOptions
{
    [CliOption("--access-resource-path")]
    public string? AccessResourcePath { get; set; }

    [CliOption("--expire-time")]
    public string? ExpireTime { get; set; }

    [CliOption("--factory-name")]
    public string? FactoryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--start-time")]
    public string? StartTime { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opensearchserverless", "update-account-settings")]
public record AwsOpensearchserverlessUpdateAccountSettingsOptions : AwsOptions
{
    [CliOption("--capacity-limits")]
    public string? CapacityLimits { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
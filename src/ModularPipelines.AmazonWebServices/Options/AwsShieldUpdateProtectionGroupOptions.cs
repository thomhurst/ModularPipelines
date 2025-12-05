using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("shield", "update-protection-group")]
public record AwsShieldUpdateProtectionGroupOptions(
[property: CliOption("--protection-group-id")] string ProtectionGroupId,
[property: CliOption("--aggregation")] string Aggregation,
[property: CliOption("--pattern")] string Pattern
) : AwsOptions
{
    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--members")]
    public string[]? Members { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
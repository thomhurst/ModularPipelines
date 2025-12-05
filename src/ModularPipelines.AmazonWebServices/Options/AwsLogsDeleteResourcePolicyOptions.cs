using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "delete-resource-policy")]
public record AwsLogsDeleteResourcePolicyOptions : AwsOptions
{
    [CliOption("--policy-name")]
    public string? PolicyName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-destination-policy")]
public record AwsLogsPutDestinationPolicyOptions(
[property: CliOption("--destination-name")] string DestinationName,
[property: CliOption("--access-policy")] string AccessPolicy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
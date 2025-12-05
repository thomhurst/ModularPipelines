using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sts", "get-federation-token")]
public record AwsStsGetFederationTokenOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--policy-arns")]
    public string[]? PolicyArns { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
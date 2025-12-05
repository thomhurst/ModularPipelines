using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("braket", "get-job")]
public record AwsBraketGetJobOptions(
[property: CliOption("--job-arn")] string JobArn
) : AwsOptions
{
    [CliOption("--additional-attribute-names")]
    public string[]? AdditionalAttributeNames { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
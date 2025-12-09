using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "generate-service-last-accessed-details")]
public record AwsIamGenerateServiceLastAccessedDetailsOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--granularity")]
    public string? Granularity { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
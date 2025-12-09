using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "start-app-assessment")]
public record AwsResiliencehubStartAppAssessmentOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--app-version")] string AppVersion,
[property: CliOption("--assessment-name")] string AssessmentName
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
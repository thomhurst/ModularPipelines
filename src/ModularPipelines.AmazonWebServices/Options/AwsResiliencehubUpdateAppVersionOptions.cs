using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "update-app-version")]
public record AwsResiliencehubUpdateAppVersionOptions(
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
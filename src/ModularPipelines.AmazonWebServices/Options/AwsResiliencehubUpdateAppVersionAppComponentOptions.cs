using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "update-app-version-app-component")]
public record AwsResiliencehubUpdateAppVersionAppComponentOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--additional-info")]
    public IEnumerable<KeyValue>? AdditionalInfo { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
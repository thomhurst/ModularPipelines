using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("batch", "create-scheduling-policy")]
public record AwsBatchCreateSchedulingPolicyOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--fairshare-policy")]
    public string? FairsharePolicy { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
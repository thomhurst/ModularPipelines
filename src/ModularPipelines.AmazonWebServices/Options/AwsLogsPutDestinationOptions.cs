using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-destination")]
public record AwsLogsPutDestinationOptions(
[property: CliOption("--destination-name")] string DestinationName,
[property: CliOption("--target-arn")] string TargetArn,
[property: CliOption("--role-arn")] string RoleArn
) : AwsOptions
{
    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
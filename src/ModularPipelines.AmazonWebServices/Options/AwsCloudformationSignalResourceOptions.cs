using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "signal-resource")]
public record AwsCloudformationSignalResourceOptions(
[property: CliOption("--stack-name")] string StackName,
[property: CliOption("--logical-resource-id")] string LogicalResourceId,
[property: CliOption("--unique-id")] string UniqueId,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
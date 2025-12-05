using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "delete-change-set")]
public record AwsCloudformationDeleteChangeSetOptions(
[property: CliOption("--change-set-name")] string ChangeSetName
) : AwsOptions
{
    [CliOption("--stack-name")]
    public string? StackName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
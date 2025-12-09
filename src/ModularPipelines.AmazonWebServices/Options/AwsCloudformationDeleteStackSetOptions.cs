using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "delete-stack-set")]
public record AwsCloudformationDeleteStackSetOptions(
[property: CliOption("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
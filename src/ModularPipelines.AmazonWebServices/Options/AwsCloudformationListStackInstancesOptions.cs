using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "list-stack-instances")]
public record AwsCloudformationListStackInstancesOptions(
[property: CliOption("--stack-set-name")] string StackSetName
) : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--stack-instance-account")]
    public string? StackInstanceAccount { get; set; }

    [CliOption("--stack-instance-region")]
    public string? StackInstanceRegion { get; set; }

    [CliOption("--call-as")]
    public string? CallAs { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
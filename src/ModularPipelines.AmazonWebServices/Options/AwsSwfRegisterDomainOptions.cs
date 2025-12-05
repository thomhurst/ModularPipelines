using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "register-domain")]
public record AwsSwfRegisterDomainOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--workflow-execution-retention-period-in-days")] string WorkflowExecutionRetentionPeriodInDays
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
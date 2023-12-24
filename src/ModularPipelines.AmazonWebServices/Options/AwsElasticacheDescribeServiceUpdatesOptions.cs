using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "describe-service-updates")]
public record AwsElasticacheDescribeServiceUpdatesOptions : AwsOptions
{
    [CommandSwitch("--service-update-name")]
    public string? ServiceUpdateName { get; set; }

    [CommandSwitch("--service-update-status")]
    public string[]? ServiceUpdateStatus { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
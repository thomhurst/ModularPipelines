using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticbeanstalk", "wait", "environment-terminated")]
public record AwsElasticbeanstalkWaitEnvironmentTerminatedOptions : AwsOptions
{
    [CommandSwitch("--application-name")]
    public string? ApplicationName { get; set; }

    [CommandSwitch("--version-label")]
    public string? VersionLabel { get; set; }

    [CommandSwitch("--environment-ids")]
    public string[]? EnvironmentIds { get; set; }

    [CommandSwitch("--environment-names")]
    public string[]? EnvironmentNames { get; set; }

    [CommandSwitch("--included-deleted-back-to")]
    public long? IncludedDeletedBackTo { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
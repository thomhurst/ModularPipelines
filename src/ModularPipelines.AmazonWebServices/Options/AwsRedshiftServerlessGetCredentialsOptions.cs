using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "get-credentials")]
public record AwsRedshiftServerlessGetCredentialsOptions : AwsOptions
{
    [CommandSwitch("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
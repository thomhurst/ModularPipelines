using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "get-cluster-credentials-with-iam")]
public record AwsRedshiftGetClusterCredentialsWithIamOptions : AwsOptions
{
    [CommandSwitch("--db-name")]
    public string? DbName { get; set; }

    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CommandSwitch("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
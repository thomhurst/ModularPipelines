using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift-serverless", "create-workgroup")]
public record AwsRedshiftServerlessCreateWorkgroupOptions(
[property: CliOption("--namespace-name")] string NamespaceName,
[property: CliOption("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CliOption("--base-capacity")]
    public int? BaseCapacity { get; set; }

    [CliOption("--config-parameters")]
    public string[]? ConfigParameters { get; set; }

    [CliOption("--max-capacity")]
    public int? MaxCapacity { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mgn", "delete-replication-configuration-template")]
public record AwsMgnDeleteReplicationConfigurationTemplateOptions(
[property: CliOption("--replication-configuration-template-id")] string ReplicationConfigurationTemplateId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
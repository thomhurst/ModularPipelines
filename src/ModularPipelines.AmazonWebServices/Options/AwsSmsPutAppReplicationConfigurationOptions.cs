using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sms", "put-app-replication-configuration")]
public record AwsSmsPutAppReplicationConfigurationOptions : AwsOptions
{
    [CliOption("--app-id")]
    public string? AppId { get; set; }

    [CliOption("--server-group-replication-configurations")]
    public string[]? ServerGroupReplicationConfigurations { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
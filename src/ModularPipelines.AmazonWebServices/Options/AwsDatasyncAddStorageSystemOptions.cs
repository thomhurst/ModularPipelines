using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datasync", "add-storage-system")]
public record AwsDatasyncAddStorageSystemOptions(
[property: CliOption("--server-configuration")] string ServerConfiguration,
[property: CliOption("--system-type")] string SystemType,
[property: CliOption("--agent-arns")] string[] AgentArns,
[property: CliOption("--credentials")] string Credentials
) : AwsOptions
{
    [CliOption("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar-connections", "create-connection")]
public record AwsCodestarConnectionsCreateConnectionOptions(
[property: CliOption("--connection-name")] string ConnectionName
) : AwsOptions
{
    [CliOption("--provider-type")]
    public string? ProviderType { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--host-arn")]
    public string? HostArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
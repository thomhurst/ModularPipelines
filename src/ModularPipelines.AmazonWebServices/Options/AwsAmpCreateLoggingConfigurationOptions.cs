using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amp", "create-logging-configuration")]
public record AwsAmpCreateLoggingConfigurationOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--log-group-arn")] string LogGroupArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amp", "update-logging-configuration")]
public record AwsAmpUpdateLoggingConfigurationOptions(
[property: CliOption("--workspace-id")] string WorkspaceId,
[property: CliOption("--log-group-arn")] string LogGroupArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
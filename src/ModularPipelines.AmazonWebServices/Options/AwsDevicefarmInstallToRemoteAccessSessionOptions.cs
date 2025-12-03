using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "install-to-remote-access-session")]
public record AwsDevicefarmInstallToRemoteAccessSessionOptions(
[property: CliOption("--remote-access-session-arn")] string RemoteAccessSessionArn,
[property: CliOption("--app-arn")] string AppArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
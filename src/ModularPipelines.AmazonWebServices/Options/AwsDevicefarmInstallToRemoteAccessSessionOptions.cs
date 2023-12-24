using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "install-to-remote-access-session")]
public record AwsDevicefarmInstallToRemoteAccessSessionOptions(
[property: CommandSwitch("--remote-access-session-arn")] string RemoteAccessSessionArn,
[property: CommandSwitch("--app-arn")] string AppArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
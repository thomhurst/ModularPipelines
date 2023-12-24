using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "test-identity-provider")]
public record AwsTransferTestIdentityProviderOptions(
[property: CommandSwitch("--server-id")] string ServerId,
[property: CommandSwitch("--user-name")] string UserName
) : AwsOptions
{
    [CommandSwitch("--server-protocol")]
    public string? ServerProtocol { get; set; }

    [CommandSwitch("--source-ip")]
    public string? SourceIp { get; set; }

    [CommandSwitch("--user-password")]
    public string? UserPassword { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
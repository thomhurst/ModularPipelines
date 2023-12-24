using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar-connections", "create-connection")]
public record AwsCodestarConnectionsCreateConnectionOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName
) : AwsOptions
{
    [CommandSwitch("--provider-type")]
    public string? ProviderType { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--host-arn")]
    public string? HostArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "create-connector")]
public record AwsTransferCreateConnectorOptions(
[property: CommandSwitch("--url")] string Url,
[property: CommandSwitch("--access-role")] string AccessRole
) : AwsOptions
{
    [CommandSwitch("--as2-config")]
    public string? As2Config { get; set; }

    [CommandSwitch("--logging-role")]
    public string? LoggingRole { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--sftp-config")]
    public string? SftpConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
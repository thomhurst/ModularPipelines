using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "update-connector")]
public record AwsTransferUpdateConnectorOptions(
[property: CommandSwitch("--connector-id")] string ConnectorId
) : AwsOptions
{
    [CommandSwitch("--url")]
    public string? Url { get; set; }

    [CommandSwitch("--as2-config")]
    public string? As2Config { get; set; }

    [CommandSwitch("--access-role")]
    public string? AccessRole { get; set; }

    [CommandSwitch("--logging-role")]
    public string? LoggingRole { get; set; }

    [CommandSwitch("--sftp-config")]
    public string? SftpConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "create-location-object-storage")]
public record AwsDatasyncCreateLocationObjectStorageOptions(
[property: CommandSwitch("--server-hostname")] string ServerHostname,
[property: CommandSwitch("--bucket-name")] string BucketName,
[property: CommandSwitch("--agent-arns")] string[] AgentArns
) : AwsOptions
{
    [CommandSwitch("--server-port")]
    public int? ServerPort { get; set; }

    [CommandSwitch("--server-protocol")]
    public string? ServerProtocol { get; set; }

    [CommandSwitch("--subdirectory")]
    public string? Subdirectory { get; set; }

    [CommandSwitch("--access-key")]
    public string? AccessKey { get; set; }

    [CommandSwitch("--secret-key")]
    public string? SecretKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--server-certificate")]
    public string? ServerCertificate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
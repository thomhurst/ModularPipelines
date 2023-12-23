using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "update-location-object-storage")]
public record AwsDatasyncUpdateLocationObjectStorageOptions(
[property: CommandSwitch("--location-arn")] string LocationArn
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

    [CommandSwitch("--agent-arns")]
    public string[]? AgentArns { get; set; }

    [CommandSwitch("--server-certificate")]
    public string? ServerCertificate { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
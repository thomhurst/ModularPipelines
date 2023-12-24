using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datasync", "add-storage-system")]
public record AwsDatasyncAddStorageSystemOptions(
[property: CommandSwitch("--server-configuration")] string ServerConfiguration,
[property: CommandSwitch("--system-type")] string SystemType,
[property: CommandSwitch("--agent-arns")] string[] AgentArns,
[property: CommandSwitch("--credentials")] string Credentials
) : AwsOptions
{
    [CommandSwitch("--cloud-watch-log-group-arn")]
    public string? CloudWatchLogGroupArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
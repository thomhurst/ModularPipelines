using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "create-studio-component")]
public record AwsNimbleCreateStudioComponentOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--studio-id")] string StudioId,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--ec2-security-group-ids")]
    public string[]? Ec2SecurityGroupIds { get; set; }

    [CommandSwitch("--initialization-scripts")]
    public string[]? InitializationScripts { get; set; }

    [CommandSwitch("--runtime-role-arn")]
    public string? RuntimeRoleArn { get; set; }

    [CommandSwitch("--script-parameters")]
    public string[]? ScriptParameters { get; set; }

    [CommandSwitch("--secure-initialization-role-arn")]
    public string? SecureInitializationRoleArn { get; set; }

    [CommandSwitch("--subtype")]
    public string? Subtype { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
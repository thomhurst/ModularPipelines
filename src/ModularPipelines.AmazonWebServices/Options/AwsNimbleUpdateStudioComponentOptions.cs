using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("nimble", "update-studio-component")]
public record AwsNimbleUpdateStudioComponentOptions(
[property: CommandSwitch("--studio-component-id")] string StudioComponentId,
[property: CommandSwitch("--studio-id")] string StudioId
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

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--runtime-role-arn")]
    public string? RuntimeRoleArn { get; set; }

    [CommandSwitch("--script-parameters")]
    public string[]? ScriptParameters { get; set; }

    [CommandSwitch("--secure-initialization-role-arn")]
    public string? SecureInitializationRoleArn { get; set; }

    [CommandSwitch("--subtype")]
    public string? Subtype { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
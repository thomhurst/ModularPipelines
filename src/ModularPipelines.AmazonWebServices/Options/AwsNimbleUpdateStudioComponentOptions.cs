using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "update-studio-component")]
public record AwsNimbleUpdateStudioComponentOptions(
[property: CliOption("--studio-component-id")] string StudioComponentId,
[property: CliOption("--studio-id")] string StudioId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--ec2-security-group-ids")]
    public string[]? Ec2SecurityGroupIds { get; set; }

    [CliOption("--initialization-scripts")]
    public string[]? InitializationScripts { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--runtime-role-arn")]
    public string? RuntimeRoleArn { get; set; }

    [CliOption("--script-parameters")]
    public string[]? ScriptParameters { get; set; }

    [CliOption("--secure-initialization-role-arn")]
    public string? SecureInitializationRoleArn { get; set; }

    [CliOption("--subtype")]
    public string? Subtype { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
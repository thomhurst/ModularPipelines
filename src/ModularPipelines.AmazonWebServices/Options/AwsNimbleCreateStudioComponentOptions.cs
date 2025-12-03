using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("nimble", "create-studio-component")]
public record AwsNimbleCreateStudioComponentOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--studio-id")] string StudioId,
[property: CliOption("--type")] string Type
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

    [CliOption("--runtime-role-arn")]
    public string? RuntimeRoleArn { get; set; }

    [CliOption("--script-parameters")]
    public string[]? ScriptParameters { get; set; }

    [CliOption("--secure-initialization-role-arn")]
    public string? SecureInitializationRoleArn { get; set; }

    [CliOption("--subtype")]
    public string? Subtype { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "create-index")]
public record AwsKendraCreateIndexOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--edition")]
    public string? Edition { get; set; }

    [CommandSwitch("--server-side-encryption-configuration")]
    public string? ServerSideEncryptionConfiguration { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--user-token-configurations")]
    public string[]? UserTokenConfigurations { get; set; }

    [CommandSwitch("--user-context-policy")]
    public string? UserContextPolicy { get; set; }

    [CommandSwitch("--user-group-resolution-configuration")]
    public string? UserGroupResolutionConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
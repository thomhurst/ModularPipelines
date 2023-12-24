using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "put-resource-policy")]
public record AwsSecretsmanagerPutResourcePolicyOptions(
[property: CommandSwitch("--secret-id")] string SecretId,
[property: CommandSwitch("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
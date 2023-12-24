using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("secretsmanager", "validate-resource-policy")]
public record AwsSecretsmanagerValidateResourcePolicyOptions(
[property: CommandSwitch("--resource-policy")] string ResourcePolicy
) : AwsOptions
{
    [CommandSwitch("--secret-id")]
    public string? SecretId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
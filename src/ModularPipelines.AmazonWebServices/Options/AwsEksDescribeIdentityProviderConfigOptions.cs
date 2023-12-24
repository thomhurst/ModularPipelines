using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "describe-identity-provider-config")]
public record AwsEksDescribeIdentityProviderConfigOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--identity-provider-config")] string IdentityProviderConfig
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
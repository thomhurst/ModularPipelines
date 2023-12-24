using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("securitylake", "create-data-lake")]
public record AwsSecuritylakeCreateDataLakeOptions(
[property: CommandSwitch("--configurations")] string[] Configurations,
[property: CommandSwitch("--meta-store-manager-role-arn")] string MetaStoreManagerRoleArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
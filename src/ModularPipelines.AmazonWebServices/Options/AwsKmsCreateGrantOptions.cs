using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "create-grant")]
public record AwsKmsCreateGrantOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--grantee-principal")] string GranteePrincipal,
[property: CommandSwitch("--operations")] string[] Operations
) : AwsOptions
{
    [CommandSwitch("--retiring-principal")]
    public string? RetiringPrincipal { get; set; }

    [CommandSwitch("--constraints")]
    public string? Constraints { get; set; }

    [CommandSwitch("--grant-tokens")]
    public string[]? GrantTokens { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
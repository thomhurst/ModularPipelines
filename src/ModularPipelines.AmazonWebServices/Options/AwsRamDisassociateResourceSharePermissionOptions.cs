using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "disassociate-resource-share-permission")]
public record AwsRamDisassociateResourceSharePermissionOptions(
[property: CommandSwitch("--resource-share-arn")] string ResourceShareArn,
[property: CommandSwitch("--permission-arn")] string PermissionArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "associate-resource-share-permission")]
public record AwsRamAssociateResourceSharePermissionOptions(
[property: CommandSwitch("--resource-share-arn")] string ResourceShareArn,
[property: CommandSwitch("--permission-arn")] string PermissionArn
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "replace-permission-associations")]
public record AwsRamReplacePermissionAssociationsOptions(
[property: CommandSwitch("--from-permission-arn")] string FromPermissionArn,
[property: CommandSwitch("--to-permission-arn")] string ToPermissionArn
) : AwsOptions
{
    [CommandSwitch("--from-permission-version")]
    public int? FromPermissionVersion { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
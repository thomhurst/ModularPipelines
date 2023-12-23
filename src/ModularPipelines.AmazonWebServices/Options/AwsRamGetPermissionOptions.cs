using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ram", "get-permission")]
public record AwsRamGetPermissionOptions(
[property: CommandSwitch("--permission-arn")] string PermissionArn
) : AwsOptions
{
    [CommandSwitch("--permission-version")]
    public int? PermissionVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
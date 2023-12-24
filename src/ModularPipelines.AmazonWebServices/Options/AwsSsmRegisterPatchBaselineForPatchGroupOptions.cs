using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "register-patch-baseline-for-patch-group")]
public record AwsSsmRegisterPatchBaselineForPatchGroupOptions(
[property: CommandSwitch("--baseline-id")] string BaselineId,
[property: CommandSwitch("--patch-group")] string PatchGroup
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
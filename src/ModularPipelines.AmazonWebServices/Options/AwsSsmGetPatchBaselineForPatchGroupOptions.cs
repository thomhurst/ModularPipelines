using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-patch-baseline-for-patch-group")]
public record AwsSsmGetPatchBaselineForPatchGroupOptions(
[property: CommandSwitch("--patch-group")] string PatchGroup
) : AwsOptions
{
    [CommandSwitch("--operating-system")]
    public string? OperatingSystem { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "get-default-patch-baseline")]
public record AwsSsmGetDefaultPatchBaselineOptions : AwsOptions
{
    [CommandSwitch("--operating-system")]
    public string? OperatingSystem { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
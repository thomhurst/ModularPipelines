using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "update-framework")]
public record AwsBackupUpdateFrameworkOptions(
[property: CommandSwitch("--framework-name")] string FrameworkName
) : AwsOptions
{
    [CommandSwitch("--framework-description")]
    public string? FrameworkDescription { get; set; }

    [CommandSwitch("--framework-controls")]
    public string[]? FrameworkControls { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
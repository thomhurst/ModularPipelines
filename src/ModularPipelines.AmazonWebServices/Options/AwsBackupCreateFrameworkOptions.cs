using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("backup", "create-framework")]
public record AwsBackupCreateFrameworkOptions(
[property: CommandSwitch("--framework-name")] string FrameworkName,
[property: CommandSwitch("--framework-controls")] string[] FrameworkControls
) : AwsOptions
{
    [CommandSwitch("--framework-description")]
    public string? FrameworkDescription { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--framework-tags")]
    public IEnumerable<KeyValue>? FrameworkTags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
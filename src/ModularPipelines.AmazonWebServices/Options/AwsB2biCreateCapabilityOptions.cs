using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "create-capability")]
public record AwsB2biCreateCapabilityOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--configuration")] string Configuration
) : AwsOptions
{
    [CommandSwitch("--instructions-documents")]
    public string[]? InstructionsDocuments { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
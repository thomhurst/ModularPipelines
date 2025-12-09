using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("b2bi", "create-capability")]
public record AwsB2biCreateCapabilityOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--type")] string Type,
[property: CliOption("--configuration")] string Configuration
) : AwsOptions
{
    [CliOption("--instructions-documents")]
    public string[]? InstructionsDocuments { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
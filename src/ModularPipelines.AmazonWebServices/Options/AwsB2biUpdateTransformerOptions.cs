using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "update-transformer")]
public record AwsB2biUpdateTransformerOptions(
[property: CommandSwitch("--transformer-id")] string TransformerId
) : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--file-format")]
    public string? FileFormat { get; set; }

    [CommandSwitch("--mapping-template")]
    public string? MappingTemplate { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }

    [CommandSwitch("--edi-type")]
    public string? EdiType { get; set; }

    [CommandSwitch("--sample-document")]
    public string? SampleDocument { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("b2bi", "create-transformer")]
public record AwsB2biCreateTransformerOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--file-format")] string FileFormat,
[property: CommandSwitch("--mapping-template")] string MappingTemplate,
[property: CommandSwitch("--edi-type")] string EdiType
) : AwsOptions
{
    [CommandSwitch("--sample-document")]
    public string? SampleDocument { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
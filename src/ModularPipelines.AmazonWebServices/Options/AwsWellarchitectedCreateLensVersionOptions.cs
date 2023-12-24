using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-lens-version")]
public record AwsWellarchitectedCreateLensVersionOptions(
[property: CommandSwitch("--lens-alias")] string LensAlias,
[property: CommandSwitch("--lens-version")] string LensVersion
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
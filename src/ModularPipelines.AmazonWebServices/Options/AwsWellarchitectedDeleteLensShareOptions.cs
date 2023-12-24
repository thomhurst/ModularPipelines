using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "delete-lens-share")]
public record AwsWellarchitectedDeleteLensShareOptions(
[property: CommandSwitch("--share-id")] string ShareId,
[property: CommandSwitch("--lens-alias")] string LensAlias
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
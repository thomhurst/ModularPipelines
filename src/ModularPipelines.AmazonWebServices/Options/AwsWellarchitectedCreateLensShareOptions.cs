using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("wellarchitected", "create-lens-share")]
public record AwsWellarchitectedCreateLensShareOptions(
[property: CommandSwitch("--lens-alias")] string LensAlias,
[property: CommandSwitch("--shared-with")] string SharedWith
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
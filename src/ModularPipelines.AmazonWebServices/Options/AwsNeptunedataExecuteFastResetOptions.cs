using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptunedata", "execute-fast-reset")]
public record AwsNeptunedataExecuteFastResetOptions(
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--token")]
    public string? Token { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
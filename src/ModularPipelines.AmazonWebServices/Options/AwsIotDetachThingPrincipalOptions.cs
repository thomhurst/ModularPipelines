using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "detach-thing-principal")]
public record AwsIotDetachThingPrincipalOptions(
[property: CliOption("--thing-name")] string ThingName,
[property: CliOption("--principal")] string Principal
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
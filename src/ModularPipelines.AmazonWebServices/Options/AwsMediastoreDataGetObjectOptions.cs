using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediastore-data", "get-object")]
public record AwsMediastoreDataGetObjectOptions(
[property: CliOption("--path")] string Path
) : AwsOptions
{
    [CliOption("--range")]
    public string? Range { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediastore-data", "get-object")]
public record AwsMediastoreDataGetObjectOptions(
[property: CommandSwitch("--path")] string Path
) : AwsOptions
{
    [CommandSwitch("--range")]
    public string? Range { get; set; }
}
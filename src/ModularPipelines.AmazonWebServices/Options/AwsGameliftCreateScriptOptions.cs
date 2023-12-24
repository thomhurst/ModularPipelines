using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-script")]
public record AwsGameliftCreateScriptOptions : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--storage-location")]
    public string? StorageLocation { get; set; }

    [CommandSwitch("--zip-file")]
    public string? ZipFile { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--script-version")]
    public string? ScriptVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
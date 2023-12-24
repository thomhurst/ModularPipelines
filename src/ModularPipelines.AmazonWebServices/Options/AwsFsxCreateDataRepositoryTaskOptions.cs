using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "create-data-repository-task")]
public record AwsFsxCreateDataRepositoryTaskOptions(
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--file-system-id")] string FileSystemId,
[property: CommandSwitch("--report")] string Report
) : AwsOptions
{
    [CommandSwitch("--paths")]
    public string[]? Paths { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--capacity-to-release")]
    public long? CapacityToRelease { get; set; }

    [CommandSwitch("--release-configuration")]
    public string? ReleaseConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "update-package")]
public record AwsEsUpdatePackageOptions(
[property: CommandSwitch("--package-id")] string PackageId,
[property: CommandSwitch("--package-source")] string PackageSource
) : AwsOptions
{
    [CommandSwitch("--package-description")]
    public string? PackageDescription { get; set; }

    [CommandSwitch("--commit-message")]
    public string? CommitMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
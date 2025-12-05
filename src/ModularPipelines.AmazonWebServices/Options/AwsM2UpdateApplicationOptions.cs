using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("m2", "update-application")]
public record AwsM2UpdateApplicationOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--current-application-version")] int CurrentApplicationVersion
) : AwsOptions
{
    [CliOption("--definition")]
    public string? Definition { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
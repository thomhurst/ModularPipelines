using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("resiliencehub", "delete-app-version-app-component")]
public record AwsResiliencehubDeleteAppVersionAppComponentOptions(
[property: CliOption("--app-arn")] string AppArn,
[property: CliOption("--id")] string Id
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
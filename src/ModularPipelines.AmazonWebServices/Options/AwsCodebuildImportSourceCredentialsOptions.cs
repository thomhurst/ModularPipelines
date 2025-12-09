using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "import-source-credentials")]
public record AwsCodebuildImportSourceCredentialsOptions(
[property: CliOption("--token")] string Token,
[property: CliOption("--server-type")] string ServerType,
[property: CliOption("--auth-type")] string AuthType
) : AwsOptions
{
    [CliOption("--username")]
    public string? Username { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
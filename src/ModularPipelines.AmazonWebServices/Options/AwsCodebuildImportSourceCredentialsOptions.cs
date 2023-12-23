using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "import-source-credentials")]
public record AwsCodebuildImportSourceCredentialsOptions(
[property: CommandSwitch("--token")] string Token,
[property: CommandSwitch("--server-type")] string ServerType,
[property: CommandSwitch("--auth-type")] string AuthType
) : AwsOptions
{
    [CommandSwitch("--username")]
    public string? Username { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "import-model")]
public record AwsComprehendImportModelOptions(
[property: CommandSwitch("--source-model-arn")] string SourceModelArn
) : AwsOptions
{
    [CommandSwitch("--model-name")]
    public string? ModelName { get; set; }

    [CommandSwitch("--version-name")]
    public string? VersionName { get; set; }

    [CommandSwitch("--model-kms-key-id")]
    public string? ModelKmsKeyId { get; set; }

    [CommandSwitch("--data-access-role-arn")]
    public string? DataAccessRoleArn { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
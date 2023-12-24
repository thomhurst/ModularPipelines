using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "activate-type")]
public record AwsCloudformationActivateTypeOptions : AwsOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }

    [CommandSwitch("--public-type-arn")]
    public string? PublicTypeArn { get; set; }

    [CommandSwitch("--publisher-id")]
    public string? PublisherId { get; set; }

    [CommandSwitch("--type-name")]
    public string? TypeName { get; set; }

    [CommandSwitch("--type-name-alias")]
    public string? TypeNameAlias { get; set; }

    [CommandSwitch("--logging-config")]
    public string? LoggingConfig { get; set; }

    [CommandSwitch("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CommandSwitch("--version-bump")]
    public string? VersionBump { get; set; }

    [CommandSwitch("--major-version")]
    public long? MajorVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
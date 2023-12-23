using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "publish-schema")]
public record AwsClouddirectoryPublishSchemaOptions(
[property: CommandSwitch("--development-schema-arn")] string DevelopmentSchemaArn,
[property: CommandSwitch("--schema-version")] string SchemaVersion
) : AwsOptions
{
    [CommandSwitch("--minor-version")]
    public string? MinorVersion { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
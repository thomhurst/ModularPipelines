using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("clouddirectory", "upgrade-published-schema")]
public record AwsClouddirectoryUpgradePublishedSchemaOptions(
[property: CommandSwitch("--development-schema-arn")] string DevelopmentSchemaArn,
[property: CommandSwitch("--published-schema-arn")] string PublishedSchemaArn,
[property: CommandSwitch("--minor-version")] string MinorVersion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}
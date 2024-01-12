using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("metastore", "services", "get-iam-policy")]
public record GcloudMetastoreServicesGetIamPolicyOptions(
[property: PositionalArgument] string Service,
[property: PositionalArgument] string Location
) : GcloudOptions;
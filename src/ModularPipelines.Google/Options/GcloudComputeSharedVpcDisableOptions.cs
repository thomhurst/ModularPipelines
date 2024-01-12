using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "disable")]
public record GcloudComputeSharedVpcDisableOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;
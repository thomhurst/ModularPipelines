using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "shared-vpc", "enable")]
public record GcloudComputeSharedVpcEnableOptions(
[property: PositionalArgument] string ProjectId
) : GcloudOptions;
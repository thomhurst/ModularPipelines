using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Attributes;

namespace ModularPipelines.Build.Modules.Tests;

/// <summary>
/// Runs unit tests on macOS operating system.
/// In distributed execution mode, this module will only execute on macOS workers.
/// </summary>
[RequiresOs(OS.MacOS)]
public class RunUnitTestsOnMacModule : RunUnitTestsModule
{
}

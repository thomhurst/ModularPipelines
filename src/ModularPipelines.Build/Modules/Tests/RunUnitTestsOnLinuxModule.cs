using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Attributes;

namespace ModularPipelines.Build.Modules.Tests;

/// <summary>
/// Runs unit tests on Linux operating system.
/// In distributed execution mode, this module will only execute on Linux workers.
/// </summary>
[RequiresOs(OS.Linux)]
public class RunUnitTestsOnLinuxModule : RunUnitTestsModule
{
}

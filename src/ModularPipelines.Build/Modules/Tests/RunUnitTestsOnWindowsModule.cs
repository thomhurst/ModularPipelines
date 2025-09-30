using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Distributed.Attributes;

namespace ModularPipelines.Build.Modules.Tests;

/// <summary>
/// Runs unit tests on Windows operating system.
/// In distributed execution mode, this module will only execute on Windows workers.
/// </summary>
[RequiresOs(OS.Windows)]
public class RunUnitTestsOnWindowsModule : RunUnitTestsModule
{
}

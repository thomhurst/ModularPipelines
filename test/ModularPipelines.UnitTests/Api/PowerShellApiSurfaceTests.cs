using ModularPipelines.Options;

namespace ModularPipelines.UnitTests.Api;

public class PowerShellApiSurfaceTests
{
    [Test]
    public async Task Uses_Canonical_PowerShell_Casing()
    {
        var assembly = typeof(PowerShellOptions).Assembly;

        using (Assert.Multiple())
        {
            await Assert.That(assembly.GetType("ModularPipelines.Options.PowerShellOptions"))
                .IsEqualTo(typeof(PowerShellOptions));
            await Assert.That(assembly.GetType("ModularPipelines.Options.PowerShellScriptOptions"))
                .IsEqualTo(typeof(PowerShellScriptOptions));
            await Assert.That(assembly.GetType("ModularPipelines.Options.PowerShellFileOptions"))
                .IsEqualTo(typeof(PowerShellFileOptions));
            await Assert.That(assembly.GetType("ModularPipelines.Context.PowerShell"))
                .IsNotNull();
            await Assert.That(assembly.GetType("ModularPipelines.Options.PowershellOptions"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Options.PowershellScriptOptions"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Options.PowershellFileOptions"))
                .IsNull();
            await Assert.That(assembly.GetType("ModularPipelines.Context.Powershell"))
                .IsNull();
        }
    }
}

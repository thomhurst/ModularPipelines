using ModularPipelines.Context;
using ModularPipelines.TestHelpers;
using ModularPipelines.Vault.Options;

namespace ModularPipelines.Vault.UnitTests;

public class VaultOptionsTests : TestBase
{
    [Test]
    public async Task Audit_Group_Renders_Required_Subcommand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var commandLine = builder.Build(new VaultAuditOptions("disable"));

        await Assert.That(commandLine.ToString()).IsEqualTo("vault audit disable");
    }

    [Test]
    public async Task Audit_Group_Rejects_Missing_Subcommand()
    {
        var builder = await GetService<ICommandLineBuilder>();

        var exception = Assert.Throws<ArgumentException>(() => builder.Build(new VaultAuditOptions(default!)));
        var expectedDiagnostic = exception is ArgumentNullException
            ? nameof(VaultAuditOptions.Subcommand)
            : $"{nameof(VaultAuditOptions)}.{nameof(VaultAuditOptions.Subcommand)}";

        using (Assert.Multiple())
        {
            await Assert.That(exception.ParamName).IsEqualTo(nameof(VaultAuditOptions.Subcommand));
            await Assert.That(exception.Message).Contains(expectedDiagnostic);
        }
    }
}

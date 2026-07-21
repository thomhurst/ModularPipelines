using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Liquibase.Enums;
using ModularPipelines.Liquibase.Options;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Commands;

public class LiquibaseOptionsTests : TestBase
{
    [Test]
    public async Task Update_Options_Parse_As_Expected()
    {
        var result = await GetResult(new LiquibaseUpdateOptions("changelog.xml", "jdbc:h2:mem:test")
        {
            ChangelogProperty =
            [
                new KeyValue("tenant", "alpha"),
                new KeyValue("region", "eu"),
            ],
            Password = "secret",
            SearchPath = "changelogs",
            ShowSummary = LiquibaseShowSummary.Verbose,
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "liquibase --search-path=changelogs update --changelog-file=changelog.xml --url=jdbc:h2:mem:test -Dtenant=alpha -Dregion=eu --password=secret --show-summary=verbose");
    }

    [Test]
    public async Task Value_Booleans_And_Numbers_Parse_As_Expected()
    {
        var result = await GetResult(new LiquibaseInitStartH2Options
        {
            DbPort = 9090,
            Detached = false,
            WebPort = 8080,
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "liquibase init start-h2 --db-port=9090 --detached=false --web-port=8080");
    }

    [Test]
    public async Task Password_Is_Marked_As_Secret()
    {
        var passwordProperty = typeof(LiquibaseUpdateOptions).GetProperty(nameof(LiquibaseUpdateOptions.Password));

        await Assert.That(passwordProperty).IsNotNull();
        await Assert.That(passwordProperty!.IsDefined(typeof(SecretValueAttribute), inherit: true)).IsTrue();
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommand>();
        return await command.ExecuteCommandLineTool(options, new CommandExecutionOptions { InternalDryRun = true });
    }
}

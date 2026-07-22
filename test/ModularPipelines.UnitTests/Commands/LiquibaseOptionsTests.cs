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
        var result = await GetResult(new LiquibaseUpdateOptions
        {
            ChangelogFile = "changelog.xml",
            ChangelogProperty =
            [
                new KeyValue("tenant", "alpha"),
                new KeyValue("region", "eu"),
            ],
            Password = "secret",
            SearchPath = "changelogs",
            ShowSummary = LiquibaseShowSummary.Verbose,
            Url = "jdbc:h2:mem:test",
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "liquibase --search-path=changelogs update --changelog-file=changelog.xml -Dtenant=alpha -Dregion=eu --password=secret --show-summary=verbose --url=jdbc:h2:mem:test");
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

    [Test]
    public async Task Monitor_Performance_Renders_A_Filename()
    {
        var result = await GetResult(new LiquibaseUpdateOptions
        {
            ChangelogFile = "changelog.xml",
            MonitorPerformance = "perf.jfr",
            Url = "jdbc:h2:mem:test",
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "liquibase --monitor-performance=perf.jfr update --changelog-file=changelog.xml --url=jdbc:h2:mem:test");
    }

    [Test]
    public async Task Databricks_Diff_Filters_Render_As_Global_Options()
    {
        var result = await GetResult(new LiquibaseDiffOptions
        {
            DatabricksDiffTblPropertiesExcludePatterns = "delta.,pipelines.",
            DatabricksDiffTblPropertiesIgnoreAll = true,
            ReferenceUrl = "jdbc:databricks://reference",
            Url = "jdbc:databricks://target",
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "liquibase --databricks-diff-tblproperties-exclude-patterns=delta.,pipelines. --databricks-diff-tblproperties-ignore-all=true diff --reference-url=jdbc:databricks://reference --url=jdbc:databricks://target");
    }

    [Test]
    public async Task Defaults_File_Can_Supply_Required_Update_Values()
    {
        var result = await GetResult(new LiquibaseUpdateOptions
        {
            DefaultsFile = "liquibase.properties",
        });

        await Assert.That(result.CommandInput).IsEqualTo(
            "liquibase --defaults-file=liquibase.properties update");
    }

    private async Task<CommandResult> GetResult(CommandLineToolOptions options)
    {
        var command = await GetService<ICommand>();
        return await command.ExecuteCommandLineTool(options, new CommandExecutionOptions { InternalDryRun = true });
    }
}

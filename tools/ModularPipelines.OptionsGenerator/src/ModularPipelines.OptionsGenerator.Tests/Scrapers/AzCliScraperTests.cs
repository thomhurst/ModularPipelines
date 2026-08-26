using Microsoft.Extensions.Logging.Abstractions;
using ModularPipelines.Attributes;
using ModularPipelines.OptionsGenerator.Models;
using ModularPipelines.OptionsGenerator.Scrapers.Cli;
using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers;

public class AzCliScraperTests
{
    [Test]
    public async Task Boolean_Accepted_Values_Require_An_Explicit_Value()
    {
        const string helpText = """
            Command
                az eventhubs namespace create : Create an Event Hubs namespace.

            Optional Arguments
                --disable-local-auth : A boolean value that indicates whether SAS
                                       authentication is enabled/disabled for the
                                       Event Hubs. Allowed values: false, true.
                --force              : Force the operation without confirmation.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "eventhubs", "namespace", "create"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--disable-local-auth");
        var force = command.Options.Single(item => item.SwitchName == "--force");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.ValueSeparator).IsEqualTo(" ");
            await Assert.That(force.IsFlag).IsTrue();
        }
    }

    [Test]
    public async Task Boolean_Lists_Remain_Collections()
    {
        const string helpText = """
            Command
                az vm application set : Set applications for a VM.

            Optional Arguments
                --treat-deployment-as-failure : Space-separated list of true or false corresponding
                                                to the application version ids.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "vm", "application", "set"],
            helpText);
        var option = command!.Options.Single(
            item => item.SwitchName == "--treat-deployment-as-failure");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
        }
    }

    [Test]
    public async Task Space_Delimited_Resource_Ids_Are_Grouped_Collections()
    {
        const string helpText = """
            Command
                az lock delete : Delete a lock.

            Arguments
                --ids : One or more resource IDs (space-delimited). If provided, no other
                        "Resource Id" arguments should be specified.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "lock", "delete"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
            await Assert.That(option.GroupValues).IsTrue();
        }
    }

    [Test]
    public async Task Comma_Separated_Boolean_Values_Are_Recognized()
    {
        const string helpText = """
            Command
                az service update : Update a service.

            Optional Arguments
                --enabled : Allowed values: false, true.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "service", "update"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--enabled");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Unrelated_Multiple_Wording_Does_Not_Make_Boolean_A_Collection()
    {
        const string helpText = """
            Command
                az service update : Update a service.

            Optional Arguments
                --enabled : Applies to multiple resources. Allowed values: true, false.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "service", "update"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--enabled");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.AcceptsMultipleValues).IsFalse();
        }
    }

    [Test]
    public async Task Tri_State_Allowed_Values_Are_Not_Collapsed_To_Boolean()
    {
        const string helpText = """
            Command
                az service update : Update a service.

            Optional Arguments
                --mode MODE : Allowed values: true, false, auto.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "service", "update"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--mode");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Repeatable_Explicit_Boolean_Values_Remain_A_Collection()
    {
        const string helpText = """
            Command
                az service update : Update a service.

            Optional Arguments
                --enabled : One or more values: true or false.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "service", "update"],
            helpText);
        var option = command!.Options.Single(item => item.SwitchName == "--enabled");

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.AcceptsMultipleValues).IsTrue();
        }
    }

    [Test]
    public async Task Description_Only_Value_Arguments_Are_Not_Flags()
    {
        const string helpText = """
            Command
                az stack-whatif group create : Create a deployment stack what-if result.

            Optional Arguments
                --description     : The description of deployment stack.
                --no-color        : Disable color in pretty-printed results.
                --parameters -p   : Parameters may be supplied from a file or as KEY=VALUE pairs.
                --tags            : Space-separated tags: key[=value] [key[=value] ...].
                --template-file -f: A path to a template file or Bicep file.
                --template-uri -u : A uri to a remote template file.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "stack-whatif", "group", "create"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Single(option => option.PropertyName == "Description").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.PropertyName == "Parameters").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.PropertyName == "Tags").CSharpType)
                .IsEqualTo("IEnumerable<string>?");
            await Assert.That(command.Options.Single(option => option.PropertyName == "TemplateFile").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.PropertyName == "TemplateUri").IsFlag)
                .IsFalse();
            await Assert.That(command.Options.Single(option => option.PropertyName == "NoColor").IsFlag)
                .IsTrue();
        }
    }

    [Test]
    public async Task Current_Description_Only_Values_Are_Not_Flags()
    {
        const string helpText = """
            Command
                az service update : Update a service.

            Optional Arguments
                --default-identity       : Accept system or user assigned identity separated.
                --install-script         : Install script configurations. Provide key-value pairs.
                --credentials-secret-uri : Key Vault secret URI for credentials.
                --source                 : Source URI or path for the storage mount.
                --id                     : The deployment stack what-if result resource ID.
                --maintenance-batch      : The batch of the custom-managed maintenance window. Accepted values: Default, Batch1, Batch2.
                --related                : Related resource or alert to add to the issue.
                --force                  : Force the operation without confirmation.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "service", "update"],
            helpText);

        using (Assert.Multiple())
        {
            foreach (var propertyName in new[]
                     {
                         "DefaultIdentity",
                         "InstallScript",
                         "CredentialsSecretUri",
                         "Source",
                         "Id",
                         "MaintenanceBatch",
                         "Related",
                     })
            {
                await Assert.That(command!.Options.Single(option =>
                    option.PropertyName == propertyName).IsFlag).IsFalse();
            }

            await Assert.That(command!.Options.Single(option => option.PropertyName == "Force").IsFlag)
                .IsTrue();
        }
    }

    [Test]
    public async Task Accept_Term_Remains_A_Presence_Only_Flag()
    {
        const string helpText = """
            Command
                az vm create : Create a virtual machine.

            Optional Arguments
                --accept-term : Accept the license agreement and privacy statement.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "vm", "create"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Identity_Value_Is_Optional_When_Valueless_Form_Is_Documented()
    {
        const string helpText = """
            Command
                az appconfig identity assign : Update managed identities.

            Optional Arguments
                --identities : Accept system-assigned or user-assigned managed identities separated by spaces. If this argument is provided without any value, system-assigned managed identity is used.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "appconfig", "identity", "assign"],
            helpText);
        var identities = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(identities.IsFlag).IsFalse();
            await Assert.That(identities.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(identities.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(identities.GroupValues).IsTrue();
        }
    }

    private sealed class TestAzCliScraper()
        : AzCliScraper(
            new ProcessCliCommandExecutor(NullLogger<ProcessCliCommandExecutor>.Instance),
            new HelpTextCache(NullLogger<HelpTextCache>.Instance),
            NullLogger<AzCliScraper>.Instance)
    {
        public Task<CliCommandDefinition?> Parse(string[] commandPath, string helpText) =>
            ParseCommandAsync(commandPath, helpText, CancellationToken.None);
    }
}

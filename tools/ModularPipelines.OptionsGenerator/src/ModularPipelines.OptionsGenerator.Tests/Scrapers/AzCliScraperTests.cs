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
                --parameters -p   : Parameters may be supplied from a file or as KEY=VALUE pairs. Parameters are evaluated in order, so the latter value will be used.
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
            var parameters = command.Options.Single(option => option.PropertyName == "Parameters");
            await Assert.That(parameters.IsFlag).IsFalse();
            await Assert.That(parameters.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(parameters.GroupValues).IsTrue();
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
    public async Task Inline_Required_And_Resource_Id_Arguments_Are_Parsed()
    {
        const string helpText = """
            Command
                az stack-whatif group create : Create a deployment stack what-if result.

            Arguments
                --name -n           [Required] : The name of the deployment stack what-if result.
                --action-on-unmanage --aou [Required] : Action to take on resources that stop being managed.
                --issue-name --name -i [Required] : The name of the issue resource.
                --retention-interval --ri [Required] : The duration to retain deleted resources.
                --stack-id          [Required] : The fully-qualified ID of the deployment stack.

            Resource Id Arguments
                --resource-group -g [Required] : The resource group where the result will be created.

            Network Arguments
                --subnet --subnet-id [Required] : The subnet resource ID.

            Optional Arguments
                --description : The description of deployment stack.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "stack-whatif", "group", "create"],
            helpText);
        var requiredOptions = command!.Options.Where(option => option.IsRequired).ToArray();

        using (Assert.Multiple())
        {
            await Assert.That(command.Options).Contains(option => option.PropertyName == "Description");
            await Assert.That(requiredOptions.Select(option => option.PropertyName))
                .IsEquivalentTo([
                    "ActionOnUnmanage",
                    "IssueName",
                    "Name",
                    "ResourceGroup",
                    "RetentionInterval",
                    "StackId",
                    "Subnet",
                ]);
            await Assert.That(requiredOptions
                .All(option => !option.IsFlag && option.CSharpType == "string?")).IsTrue();
            await Assert.That(command.Options.Single(option => option.PropertyName == "ActionOnUnmanage").ShortForm)
                .IsEqualTo("--aou");
            await Assert.That(command.Options.Single(option => option.PropertyName == "Name").ShortForm)
                .IsEqualTo("-n");
            await Assert.That(command.Options.Single(option => option.PropertyName == "IssueName").ShortForm)
                .IsEqualTo("-i");
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
    public async Task Monitor_And_Stack_Descriptions_Require_Values()
    {
        const string helpText = """
            Command
                az monitor account issue create : Create an issue.

            Optional Arguments
                --background                     : The issue background information.
                --impact-time                    : The issue impact time (in UTC).
                --notifications                  : The issue notification settings.
                --severity                       : The issue severity.
                --status                         : The issue status. Allowed values: Closed, New.
                --title                          : The issue title.
                --deny-settings-excluded-actions : List of operations excluded from deny settings.
                --deployment-resource-group      : The scope at which deployment is created.
                --resources-without-delete-support: Defines what happens to unsupported resources.
                --validation-level               : Validation level for the deployment stack.
                --no-color                       : Disable color in results.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "monitor", "account", "issue", "create"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Where(option => option.PropertyName != "NoColor")
                .All(option => !option.IsFlag && option.CSharpType != "bool?")).IsTrue();
            await Assert.That(command.Options.Single(option =>
                    option.PropertyName == "DenySettingsExcludedActions").GroupValues)
                .IsTrue();
            await Assert.That(command.Options.Single(option => option.PropertyName == "NoColor").IsFlag)
                .IsTrue();
        }
    }

    [Test]
    public async Task CrLf_Section_Headers_Do_Not_Merge()
    {
        const string helpText = "Command\r\n"
                                + "    az service create : Create a service.\r\n\r\n"
                                + "Optional Arguments\r\n"
                                + "Required Arguments\r\n"
                                + "    --name : The name of the service.\r\n\r\n"
                                + "Global Arguments\r\n"
                                + "    --debug : Increase logging verbosity.\r\n";

        var command = await new TestAzCliScraper().Parse(
            ["az", "service", "create"],
            helpText);

        var option = command!.Options.Single();
        using (Assert.Multiple())
        {
            await Assert.That(option.PropertyName).IsEqualTo("Name");
            await Assert.That(option.IsRequired).IsTrue();
        }
    }

    [Test]
    public async Task Policy_Options_Are_Inherited_After_Subcommands()
    {
        var tool = new TestAzCliScraper().CreateToolDefinition();

        var acquireToken = tool.GetGlobalOptions().Single(option =>
            option.SwitchName == "--acquire-policy-token");
        var changeReference = tool.GetGlobalOptions().Single(option =>
            option.SwitchName == "--change-reference");

        using (Assert.Multiple())
        {
            await Assert.That(tool.GlobalOptionsBeforeSubcommands).IsFalse();
            await Assert.That(acquireToken.IsFlag).IsTrue();
            await Assert.That(acquireToken.CSharpType).IsEqualTo("bool?");
            await Assert.That(changeReference.IsFlag).IsFalse();
            await Assert.That(changeReference.CSharpType).IsEqualTo("string?");
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

    [Test]
    public async Task Identity_Resource_Id_List_Allows_A_Bare_Option()
    {
        const string helpText = """
            Command
                az appservice plan identity remove : Remove managed identities.

            Optional Arguments
                --user-assigned : Remove user-assigned managed identities. Accepts space-separated list of identity resource IDs. If --user-assigned is specified without any resource IDs, all user-assigned managed identities are removed.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "appservice", "plan", "identity", "remove"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.ValueArity).IsEqualTo(CliOptionValueArity.Optional);
            await Assert.That(option.PropertyType).IsEqualTo("IEnumerable<CliOptionValue>?");
            await Assert.That(option.GroupValues).IsTrue();
        }
    }

    [Test]
    public async Task Resource_Count_Wording_Does_Not_Make_A_Value_A_Collection()
    {
        const string helpText = """
            Command
                az appservice plan managed-instance network add : Add VNet integration.

            Optional Arguments
                --vnet : Name or resource ID of the regional virtual network. If there are multiple vnets of the same name across different resource groups, use vnet resource id.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "appservice", "plan", "managed-instance", "network", "add"],
            helpText);
        var option = command!.Options.Single();

        await Assert.That(option.CSharpType).IsEqualTo("string?");
    }

    [Test]
    public async Task List_Verb_Remains_A_Presence_Only_Flag()
    {
        const string helpText = """
            Command
                az account list : List subscriptions.

            Optional Arguments
                --all : List all subscriptions from all clouds, including disabled subscriptions.
            """;

        var command = await new TestAzCliScraper().Parse(["az", "account", "list"], helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Version_Description_Requires_A_Value()
    {
        const string helpText = """
            Command
                az network virtual-appliance migration prepare : Prepare a migration.

            Optional Arguments
                --marketplace-version : The marketplace version to migrate to.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "network", "virtual-appliance", "migration", "prepare"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("string?");
        }
    }

    [Test]
    public async Task Generic_Update_Options_Are_Grouped_Value_Collections()
    {
        const string helpText = """
            Command
                az monitor account issue update : Update an issue.

            Generic Update Arguments
                --add    : Add an object to a list by specifying a path and key value pairs.
                --remove : Remove a property or an element from a list.
                --set    : Update an object by specifying a property path and value to set.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "monitor", "account", "issue", "update"],
            helpText);

        using (Assert.Multiple())
        {
            foreach (var option in command!.Options)
            {
                await Assert.That(option.IsFlag).IsFalse();
                await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
                await Assert.That(option.GroupValues).IsTrue();
            }
        }
    }

    [Test]
    public async Task Repeatable_Set_Option_Is_Not_Grouped()
    {
        const string helpText = """
            Command
                az acr run : Run an ACR task.

            Optional Arguments
                --set : Value in 'name[=value]' format. Multiples supported by passing --set multiple times.
            """;

        var command = await new TestAzCliScraper().Parse(["az", "acr", "run"], helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(option.GroupValues).IsFalse();
        }
    }

    [Test]
    public async Task Expanded_Boolean_Value_List_Is_A_Boolean_Option()
    {
        const string helpText = """
            Command
                az network virtual-appliance migration prepare : Prepare a migration.

            Optional Arguments
                --no-wait : Do not wait for completion. Allowed values: 0, 1, f, false, n, no, t, true, y, yes.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "network", "virtual-appliance", "migration", "prepare"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Appservice_Plan_IsLinux_Preserves_Explicit_False_Value()
    {
        const string helpText = """
            Command
                az appservice plan create : Create an app service plan.

            Optional Arguments
                --is-linux : Host web app on Linux worker.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "appservice", "plan", "create"],
            helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsFalse();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
            await Assert.That(option.Description).Contains("--is-linux false");
            await Assert.That(option.Description).Contains("Allowed values: false, true");
        }
    }

    [Test]
    public async Task Preview_Options_Are_Parsed()
    {
        const string helpText = """
            Command
                az appservice plan create : Create an app service plan.

            Optional Arguments
                --default-identity [Preview] : Accept system or user assigned identity separated.
                --subnet NAME       [Preview] : Name or ID of the subnet.
            """;

        var command = await new TestAzCliScraper().Parse(
            ["az", "appservice", "plan", "create"],
            helpText);

        using (Assert.Multiple())
        {
            await Assert.That(command!.Options.Select(option => option.PropertyName))
                .IsEquivalentTo(["DefaultIdentity", "Subnet"]);
            await Assert.That(command.Options.All(option => !option.IsFlag)).IsTrue();
        }
    }

    [Test]
    public async Task Unrelated_IsLinux_Remains_A_Presence_Only_Flag()
    {
        const string helpText = """
            Command
                az example create : Create an example.

            Optional Arguments
                --is-linux : Host workload on Linux.
            """;

        var command = await new TestAzCliScraper().Parse(["az", "example", "create"], helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
        }
    }

    [Test]
    public async Task Acr_Task_Value_Shapes_Preserve_Cardinality_And_Secrets()
    {
        const string helpText = """
            Command
                az acr task create : Create a task.

            Optional Arguments
                --assign-identity : Assign managed identities to the task. Use '[system]' to refer to the system-assigned identity or a resource ID to refer to a user-assigned identity.
                --git-access-token : The access token used to access the source control provider.
                --schedule         : Schedule for a timer trigger represented as a cron expression. Multiples supported by passing --schedule multiple times.
            """;

        var command = await new TestAzCliScraper().Parse(["az", "acr", "task", "create"], helpText);
        var identity = command!.Options.Single(option => option.SwitchName == "--assign-identity");
        var token = command.Options.Single(option => option.SwitchName == "--git-access-token");
        var schedule = command.Options.Single(option => option.SwitchName == "--schedule");

        using (Assert.Multiple())
        {
            await Assert.That(identity.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(identity.GroupValues).IsTrue();
            await Assert.That(schedule.CSharpType).IsEqualTo("IEnumerable<string>?");
            await Assert.That(schedule.GroupValues).IsFalse();
            await Assert.That(token.CSharpType).IsEqualTo("string?");
            await Assert.That(token.IsSecret).IsTrue();
        }
    }

    [Test]
    public async Task Stack_Child_Scope_Is_A_Presence_Only_Flag()
    {
        const string helpText = """
            Command
                az stack group create : Create a stack.

            Optional Arguments
                --cs --deny-settings-apply-to-child-scopes : DenySettings will be applied to child scopes.
            """;

        var command = await new TestAzCliScraper().Parse(["az", "stack", "group", "create"], helpText);
        var option = command!.Options.Single();

        using (Assert.Multiple())
        {
            await Assert.That(option.IsFlag).IsTrue();
            await Assert.That(option.CSharpType).IsEqualTo("bool?");
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

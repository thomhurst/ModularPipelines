using ModularPipelines.Azure.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Azure.UnitTests;

public class AzureCollectionContractTests
{
    private static readonly string[] UserNames = ["alice", "bob"];
    private static readonly string[] ScopeNames = ["ClientConnection", "ServerConnection"];

    [Test]
    [Arguments("Administrators", "--administrators")]
    [Arguments("BackupOperators", "--backup-operators")]
    [Arguments("SecurityOperators", "--security-operators")]
    public async Task Netapp_User_Lists_Render_As_Grouped_Values(string propertyName, string switchName)
    {
        var options = new AzNetappfilesAccountAdAddOptions("account", "group");
        var property = options.GetType().GetProperty(propertyName)!;
        await Assert.That(property.PropertyType).IsEqualTo(typeof(IEnumerable<string>));
        property.SetValue(options, UserNames);

        await AssertArguments(BuildArguments(options),
            ["--account-name", "account", "--resource-group", "group", switchName, "alice", "bob"]);
    }

    [Test]
    [Arguments("Allow", "--allow")]
    [Arguments("Deny", "--deny")]
    [Arguments("ConnectionName", "--connection-name")]
    public async Task Signalr_Optional_Lists_Render_Grouped_Values_And_Bare(string propertyName, string switchName)
    {
        var options = new AzSignalrNetworkRuleUpdateOptions();
        var property = options.GetType().GetProperty(propertyName)!;
        var values = switchName is "--allow" or "--deny" ? ScopeNames : UserNames;
        await Assert.That(property.PropertyType).IsEqualTo(typeof(IEnumerable<CliOptionValue>));
        property.SetValue(options, values.Select(value => (CliOptionValue) value).ToArray());
        await AssertArguments(BuildArguments(options), [switchName, .. values]);

        property.SetValue(options, new[] { CliOptionValue.Bare });
        await AssertArguments(BuildArguments(options), [switchName]);
        property.SetValue(options, null);
        await AssertArguments(BuildArguments(options), []);
    }

    [Test]
    [Arguments(typeof(AzContainerappAuthGoogleUpdateOptions), "AllowedAudiences", "--allowed-audiences")]
    [Arguments(typeof(AzContainerappAuthMicrosoftUpdateOptions), "AllowedAudiences", "--allowed-audiences")]
    public async Task Audience_Lists_Are_One_Parser_Value(Type optionsType, string propertyName, string switchName)
    {
        var options = Activator.CreateInstance(optionsType)!;
        var property = optionsType.GetProperty(propertyName)!;
        const string value = "first audience second audience";
        await Assert.That(property.PropertyType).IsEqualTo(typeof(string));
        property.SetValue(options, value);

        await AssertArguments(BuildArguments(options), [switchName, value]);
    }
}

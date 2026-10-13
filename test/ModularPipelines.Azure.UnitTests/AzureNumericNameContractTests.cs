using ModularPipelines.Azure.Options;
using ModularPipelines.Models;
using static ModularPipelines.TestHelpers.OptionsRenderingTestHelper;

namespace ModularPipelines.Azure.UnitTests;

public class AzureNumericNameContractTests
{
    [Test]
    [Arguments(typeof(AzStorageAccountBlobServicePropertiesUpdateOptions), "EnableStaticWebsite")]
    [Arguments(typeof(AzStorageBlobServicePropertiesUpdateOptions), "StaticWebsite")]
    [Arguments(typeof(AzStorageFsServicePropertiesUpdateOptions), "StaticWebsite")]
    public async Task Error_Document_Name_Preserves_Switch_And_Static_Website_Option(
        Type optionsType,
        string staticWebsiteProperty)
    {
        var options = Activator.CreateInstance(optionsType)!;
        optionsType.GetProperty("Number404Document")!.SetValue(options, "error.html");

        await Assert.That(optionsType.GetProperty(staticWebsiteProperty)).IsNotNull();
        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--404-document", "error.html"]);
    }

    [Test]
    [Arguments(typeof(AzFunctionappConfigSetOptions))]
    [Arguments(typeof(AzWebappConfigSetOptions))]
    public async Task Numeric_Name_Segment_Preserves_Worker_Process_Switch(Type optionsType)
    {
        var options = Activator.CreateInstance(optionsType)!;
        optionsType.GetProperty("UseNumber32bitWorkerProcess")!.SetValue(options, (CliOptionValue) "true");

        await Assert.That(BuildArguments(options)).IsEquivalentTo(["--use-32bit-worker-process", "true"]);
    }
}

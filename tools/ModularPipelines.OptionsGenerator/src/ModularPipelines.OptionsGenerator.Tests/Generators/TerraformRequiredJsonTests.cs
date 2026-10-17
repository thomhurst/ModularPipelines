using System.Reflection;
using ModularPipelines.Attributes;
using ModularPipelines.Generated;
using ModularPipelines.Helpers.Internal;
using ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

namespace ModularPipelines.OptionsGenerator.Tests.Generators;

public partial class RequiredConstructorValidationTests
{
    [Test]
    public async Task Terraform_Metadata_Functions_Requires_And_Renders_Json()
    {
        var help = await File.ReadAllTextAsync(Path.Combine(
            AppContext.BaseDirectory, "Fixtures", "terraform-1.16.6-metadata-functions-help.txt"));
        var command = await new TerraformCliScraperTests.TestTerraformCliScraper()
            .Parse(["terraform", "metadata", "functions"], help);
        var generated = await Generate([.. command!.Options]);
        var options = Compile(generated).GetType("ModularPipelines.Tool.Options.ToolRunOptions")!;
        var constructor = options.GetConstructors().Single();
        await Assert.That(constructor.GetParameters().Single().ParameterType).IsEqualTo(typeof(bool));

        var exception = await Assert.That(() => constructor.Invoke([false])).Throws<TargetInvocationException>();
        await Assert.That(exception!.InnerException).IsTypeOf<ArgumentException>();

        var instance = constructor.Invoke([true]);
        var property = options.GetProperty("Json")!;
        var attribute = property.CustomAttributes.Single(attribute => attribute.AttributeType.Name == nameof(CliFlagAttribute));
        var switchName = (string) attribute.ConstructorArguments.Single().Value!;
        var part = new FlagPart(property.Name, property.GetValue, new CliFlagAttribute(switchName));
        await Assert.That(new CommandArgumentBuilder().BuildArguments([part], instance)).IsEquivalentTo(["-json"]);
    }
}

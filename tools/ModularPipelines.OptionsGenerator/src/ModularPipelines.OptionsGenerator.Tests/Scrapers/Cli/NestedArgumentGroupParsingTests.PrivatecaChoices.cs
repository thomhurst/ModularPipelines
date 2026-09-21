using System.ComponentModel.DataAnnotations;
using ModularPipelines.OptionsGenerator.Generators;

namespace ModularPipelines.OptionsGenerator.Tests.Scrapers.Cli;

public partial class NestedArgumentGroupParsingTests
{
    [Test]
    [Arguments("CopyKnownExtensions", "Constraints on known extensions.", "Constraints on unknown extensions")]
    [Arguments("DropKnownExtensions", "Constraints on known extensions.", "Constraints on unknown extensions")]
    [Arguments("CopyExtensionsByOid", "Constraints on unknown extensions by their OIDs.", "Constraints on known extensions.")]
    [Arguments("DropOidExtensions", "Constraints on unknown extensions by their OIDs.", "Constraints on known extensions.")]
    public async Task Captured_Privateca_Choice_Descriptions_Stay_With_Their_Own_Members(
        string property, string ownDescription, string siblingDescription)
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("privateca templates update");
        var description = command.Options.Single(option => option.PropertyName == property).Description;
        await Assert.That(description).Contains(ownDescription);
        await Assert.That(description).DoesNotContain(siblingDescription);
    }

    [Test]
    public async Task Captured_Privateca_Update_Choices_Stay_Independently_Optional()
    {
        var command = await GcloudCapturedSemanticsTests.Scrape("privateca templates update");
        var tool = CreateGcloudValidationTool(command);
        var generated = (await new OptionsClassGenerator().GenerateAsync(tool)).Single().Content;
        var enums = (await new EnumGenerator().GenerateAsync(tool)).Select(file => file.Content).ToArray();
        await VerifyGeneratedValidation(generated, command.ClassName, async type =>
        {
            foreach (var (properties, valid) in new (string[], bool)[]
            {
                ([], true),
                (["CopyKnownExtensions"], true),
                (["CopyExtensionsByOid"], true),
                (["CopyKnownExtensions", "CopyExtensionsByOid"], true),
                (["DropKnownExtensions"], true),
                (["DropOidExtensions"], true),
                (["CopyAllRequestedExtensions"], true),
                (["CopyAllRequestedExtensions", "CopyKnownExtensions"], false),
                (["CopyExtensionsByOid", "DropOidExtensions"], false),
                (["CopyKnownExtensions", "DropKnownExtensions"], false),
            })
            {
                var instance = Activator.CreateInstance(type, ["template"])!;
                foreach (var name in properties)
                {
                    var property = type.GetProperty(name)!;
                    if (property.PropertyType == typeof(bool?))
                    {
                        property.SetValue(instance, true);
                        continue;
                    }

                    var elementType = property.PropertyType.GetGenericArguments().Single();
                    var values = Array.CreateInstance(elementType, 1);
                    values.SetValue(elementType.IsEnum ? Enum.GetValues(elementType).GetValue(0) : "value", 0);
                    property.SetValue(instance, values);
                }

                var errors = ((IValidatableObject) instance).Validate(new(instance)).ToArray();
                await Assert.That(errors.Length == 0).IsEqualTo(valid)
                    .Because(string.Join(", ", properties) + ": " + string.Join("; ", errors.Select(error => error.ErrorMessage)));
            }
        }, enums);
    }
}

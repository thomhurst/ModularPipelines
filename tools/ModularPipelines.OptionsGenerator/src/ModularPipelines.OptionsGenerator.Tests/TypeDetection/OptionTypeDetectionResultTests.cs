using ModularPipelines.OptionsGenerator.TypeDetection;

namespace ModularPipelines.OptionsGenerator.Tests.TypeDetection;

public class OptionTypeDetectionResultTests
{
    [Test]
    public async Task Unknown_Creates_Result_With_Unknown_Type()
    {
        var result = OptionTypeDetectionResult.Unknown("TestSource");

        await Assert.That(result.Type).IsEqualTo(CliOptionType.Unknown);
    }

    [Test]
    public async Task Unknown_Creates_Result_With_Zero_Confidence()
    {
        var result = OptionTypeDetectionResult.Unknown("TestSource");

        await Assert.That(result.Confidence).IsEqualTo(0);
    }

    [Test]
    public async Task Unknown_Creates_Result_With_Provided_Source()
    {
        var result = OptionTypeDetectionResult.Unknown("MyDetector");

        await Assert.That(result.Source).IsEqualTo("MyDetector");
    }

    [Test]
    public async Task Unknown_Creates_Result_With_Null_Notes()
    {
        var result = OptionTypeDetectionResult.Unknown("TestSource");

        await Assert.That(result.Notes).IsNull();
    }

    [Test]
    public async Task Unknown_Creates_Result_With_Null_EnumValues()
    {
        var result = OptionTypeDetectionResult.Unknown("TestSource");

        await Assert.That(result.EnumValues).IsNull();
    }

    [Test]
    public async Task Can_Create_Result_With_EnumValues()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.Enum,
            Confidence = 90,
            Source = "TestSource",
            EnumValues = ["value1", "value2", "value3"],
        };

        await Assert.That(result.EnumValues).IsNotNull();
        await Assert.That(result.EnumValues!.Length).IsEqualTo(3);
        await Assert.That(result.EnumValues).Contains("value1");
        await Assert.That(result.EnumValues).Contains("value2");
        await Assert.That(result.EnumValues).Contains("value3");
    }

    [Test]
    public async Task Can_Create_Result_With_Notes()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.Bool,
            Confidence = 100,
            Source = "ManualOverride",
            Notes = "Manually configured as boolean",
        };

        await Assert.That(result.Notes).IsEqualTo("Manually configured as boolean");
    }

    [Test]
    public async Task Confidence_Can_Be_Set_To_Any_Value()
    {
        var result = new OptionTypeDetectionResult
        {
            Type = CliOptionType.String,
            Confidence = 75,
            Source = "TestSource",
        };

        await Assert.That(result.Confidence).IsEqualTo(75);
    }
}

using ModularPipelines.Attributes.Events;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.TestHelpers;

namespace ModularPipelines.UnitTests.Attributes;

public class MetadataCrossPhaseIntegrationTests : TestBase
{
    private static readonly List<string> EventLog = new();

    public class SetMetadataOnRegistrationAttribute : Attribute, IModuleRegistrationEventReceiver
    {
        private readonly string _key;
        private readonly string _value;

        public SetMetadataOnRegistrationAttribute(string key, string value)
        {
            _key = key;
            _value = value;
        }

        public Task OnRegistrationAsync(IModuleRegistrationContext context)
        {
            context.SetMetadata(_key, _value);
            EventLog.Add($"Registration:SetMetadata:{_key}={_value}");
            return Task.CompletedTask;
        }
    }

    public class ReadMetadataOnStartAttribute : Attribute, IModuleStartEventReceiver
    {
        private readonly string _key;

        public ReadMetadataOnStartAttribute(string key)
        {
            _key = key;
        }

        public Task OnModuleStartAsync(IModuleEventContext context)
        {
            var value = context.GetMetadata<string>(_key);
            EventLog.Add($"Start:ReadMetadata:{_key}={value ?? "null"}");
            return Task.CompletedTask;
        }
    }

    public class ReadMetadataOnEndAttribute : Attribute, IModuleEndEventReceiver
    {
        private readonly string _key;

        public ReadMetadataOnEndAttribute(string key)
        {
            _key = key;
        }

        public Task OnModuleEndAsync(IModuleEventContext context, ModuleResult result)
        {
            var value = context.GetMetadata<string>(_key);
            EventLog.Add($"End:ReadMetadata:{_key}={value ?? "null"}");
            return Task.CompletedTask;
        }
    }

    [SetMetadataOnRegistration("config", "value-from-registration")]
    [ReadMetadataOnStart("config")]
    [ReadMetadataOnEnd("config")]
    public class MetadataModule : Module<string>
    {
        public override async Task<string?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        {
            await Task.Yield();
            return "Done";
        }
    }

    [Before(Test)]
    public void ClearEventLog()
    {
        EventLog.Clear();
    }

    [Test]
    public async Task Metadata_SetDuringRegistration_AvailableDuringLifecycleEvents()
    {
        var result = await TestPipelineHostBuilder.Create()
            .AddModule<MetadataModule>()
            .ExecutePipelineAsync();

        await Assert.That(result.Status).IsEqualTo(Enums.Status.Successful);

        // Verify the registration event set the metadata
        await Assert.That(EventLog).Contains("Registration:SetMetadata:config=value-from-registration");

        // Verify the start event could read the metadata
        await Assert.That(EventLog).Contains("Start:ReadMetadata:config=value-from-registration");

        // Verify the end event could read the metadata
        await Assert.That(EventLog).Contains("End:ReadMetadata:config=value-from-registration");
    }
}

using Microsoft.Extensions.Options;

namespace ModularPipelines.Options;

internal sealed class PipelineOptionsFactory(
    PipelineOptions value,
    IEnumerable<IConfigureOptions<PipelineOptions>> setups,
    IEnumerable<IPostConfigureOptions<PipelineOptions>> postConfigures,
    IEnumerable<IValidateOptions<PipelineOptions>> validations)
    : IOptionsFactory<PipelineOptions>
{
    public PipelineOptions Create(string name)
    {
        var options = value with { };

        foreach (var setup in setups)
        {
            if (setup is IConfigureNamedOptions<PipelineOptions> namedSetup)
            {
                namedSetup.Configure(name, options);
            }
            else if (name == Microsoft.Extensions.Options.Options.DefaultName)
            {
                setup.Configure(options);
            }
        }

        foreach (var postConfigure in postConfigures)
        {
            postConfigure.PostConfigure(name, options);
        }

        var failures = new List<string>();
        foreach (var validation in validations)
        {
            var result = validation.Validate(name, options);
            if (result.Failed)
            {
                failures.AddRange(result.Failures);
            }
        }

        return failures.Count == 0
            ? options
            : throw new OptionsValidationException(name, typeof(PipelineOptions), failures);
    }
}

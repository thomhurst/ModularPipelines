# Secrets

## Use IOptions<>[​](#use-ioptions "Direct link to Use IOptions<>")

If you don't know how IOptions work, then go check out a tutorial. This is the recommended and supported way of storing configuration in Modular Pipelines.

Your options classes should be registered as IOptions.

If you have any sensitive/secret data stored in these classes, you can attribute your property with `[SecretValue]`.

This attribute, combined with the logger exposed by `context.Logger`, means that if that value is ever attempted to be written to logs, it'll be censored out, so that secret values aren't visible to those unauthorised.

`[SecretValue]` supports scalar strings, character sequences such as `char[]`, `IEnumerable<char>`, `Memory<char>`, and `ReadOnlyMemory<char>`, and collections of secret values. Character sequences are treated as one secret, while secret collections are masked item by item.

## Example[​](#example "Direct link to Example")

```
public record MySettings

{

    [SecretValue]

    public string? ApiKey { get; set; }

}
```

## Mask a configuration section[​](#mask-a-configuration-section "Direct link to Mask a configuration section")

When a configuration provider supplies a group of secrets, you can register every leaf value in that section without adding `[SecretValue]` to an options class:

```
var builder = Pipeline.CreateBuilder();



builder.MaskConfigurationSection("Secrets");
```

Nested values such as `Secrets:Database:Password` are included. Missing sections and empty values are ignored. The values are registered during pipeline startup and use the same log and CI-native masking as `[SecretValue]` and `ISecretRegistry`.

You can also configure multiple sections through options:

```
builder.ConfigureOptions(options => options with

{

    Secrets = options.Secrets with

    {

        MaskedConfigurationSections = ["Secrets", "ConnectionStrings"],

    },

});
```

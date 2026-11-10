using System.Text;
using ModularPipelines.Constants;
using ModularPipelines.Options;

namespace ModularPipelines.Http;

/// <summary>
/// Redacts credentials from URI display text without changing the URI used for requests.
/// </summary>
internal static class HttpUriRedactor
{
    public static string Redact(Uri? uri, HttpLoggingOptions? options = null, Uri? baseAddress = null)
    {
        if (uri is { IsAbsoluteUri: false }
            && uri.OriginalString.StartsWith("//", StringComparison.Ordinal)
            && Uri.TryCreate("https:" + uri.OriginalString, UriKind.Absolute, out var absoluteUri))
        {
            return Redact(absoluteUri, options)["https:".Length..];
        }

        var effectiveUri = uri;
        if (baseAddress is not null && uri?.IsAbsoluteUri != true)
        {
            effectiveUri = uri is null ? baseAddress : new Uri(baseAddress, uri);
        }

        if (effectiveUri is { IsAbsoluteUri: true } && IsWebhook(effectiveUri))
        {
            // Resolve only for display: HttpClient still owns request URI resolution.
            // Webhook paths are credentials, including paths inherited from BaseAddress.
            return effectiveUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped) + "/" + LoggingConstants.SecretMask;
        }

        return uri is null ? string.Empty : RedactQuery(RedactUserInfo(uri), options ?? HttpLoggingOptions.Default);
    }

    private static string RedactUserInfo(Uri uri)
    {
        var text = uri.IsAbsoluteUri ? uri.AbsoluteUri : uri.OriginalString;
        if (!uri.IsAbsoluteUri || string.IsNullOrEmpty(uri.UserInfo))
        {
            return text;
        }

        var schemeDelimiter = text.IndexOf("://", StringComparison.Ordinal);
        var authorityStart = schemeDelimiter >= 0 ? schemeDelimiter + 3 : uri.Scheme.Length + 1;
        var userInfoEnd = text.IndexOf('@', authorityStart);
        if (userInfoEnd >= authorityStart)
        {
            return text[..authorityStart] + LoggingConstants.SecretMask + text[userInfoEnd..];
        }

        return text;
    }

    private static string RedactQuery(string text, HttpLoggingOptions options)
    {
        var fragmentStart = text.IndexOf('#');
        var queryEnd = fragmentStart >= 0 ? fragmentStart : text.Length;
        var queryStart = text.IndexOf('?');
        if (queryStart < 0 || queryStart > queryEnd)
        {
            return text;
        }

        var sensitiveNames = options.SensitiveQueryParameterNames;
        var result = new StringBuilder(text[..(queryStart + 1)]);
        var parameters = text[(queryStart + 1)..queryEnd].Split('&');
        for (var index = 0; index < parameters.Length; index++)
        {
            if (index > 0)
            {
                result.Append('&');
            }

            var parameter = parameters[index];
            var equalsIndex = parameter.IndexOf('=');
            if (equalsIndex >= 0 && IsSensitive(parameter[..equalsIndex], sensitiveNames))
            {
                result.Append(parameter.AsSpan(0, equalsIndex + 1));
                result.Append(LoggingConstants.SecretMask);
            }
            else
            {
                result.Append(parameter);
            }
        }

        result.Append(text.AsSpan(queryEnd));
        return result.ToString();
    }

    private static bool IsSensitive(string name, IReadOnlyList<string> sensitiveNames)
    {
        var decodedName = Uri.UnescapeDataString(name.Replace('+', ' '));
        foreach (var sensitiveName in sensitiveNames)
        {
            if (string.Equals(decodedName, sensitiveName, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsWebhook(Uri uri)
    {
        var host = uri.Host.TrimEnd('.');
        return host.Equals("hooks.slack.com", StringComparison.OrdinalIgnoreCase)
               || host.Equals("hooks.slack-gov.com", StringComparison.OrdinalIgnoreCase)
               || host.Equals("webhook.office.com", StringComparison.OrdinalIgnoreCase)
               || host.EndsWith(".webhook.office.com", StringComparison.OrdinalIgnoreCase)
               || ((host.Equals("outlook.office.com", StringComparison.OrdinalIgnoreCase)
                    || host.Equals("outlook.office365.com", StringComparison.OrdinalIgnoreCase))
                   && uri.AbsolutePath.StartsWith("/webhook/", StringComparison.OrdinalIgnoreCase));
    }
}

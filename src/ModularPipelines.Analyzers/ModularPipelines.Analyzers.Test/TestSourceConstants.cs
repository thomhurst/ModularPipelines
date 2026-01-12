namespace ModularPipelines.Analyzers.Test;

/// <summary>
/// Shared test source code constants to reduce duplication across analyzer tests.
/// </summary>
internal static class TestSourceConstants
{
    /// <summary>
    /// Standard using statements for module test source code.
    /// </summary>
    public const string StandardUsings = @"#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;";

    /// <summary>
    /// Standard using statements including Options namespace.
    /// </summary>
    public const string StandardUsingsWithOptions = @"#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;";

    /// <summary>
    /// Standard using statements including Microsoft.Extensions.Logging.
    /// </summary>
    public const string StandardUsingsWithLogging = @"#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;
using Microsoft.Extensions.Logging;";

    /// <summary>
    /// Standard using statements including Extensions namespace.
    /// </summary>
    public const string StandardUsingsWithExtensions = @"#nullable enable
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModularPipelines.Context;
using ModularPipelines.Extensions;
using ModularPipelines.Models;
using ModularPipelines.Options;
using ModularPipelines.Modules;
using ModularPipelines.Attributes;";

    /// <summary>
    /// Standard namespace declaration for example modules.
    /// </summary>
    public const string ExamplesNamespace = @"namespace ModularPipelines.Examples.Modules;";

    /// <summary>
    /// Combines standard usings with the examples namespace.
    /// </summary>
    public const string StandardModuleHeader = StandardUsings + @"

" + ExamplesNamespace;

    /// <summary>
    /// Combines standard usings (with options) with the examples namespace.
    /// </summary>
    public const string StandardModuleHeaderWithOptions = StandardUsingsWithOptions + @"

" + ExamplesNamespace;

    /// <summary>
    /// Combines standard usings (with logging) with the examples namespace.
    /// </summary>
    public const string StandardModuleHeaderWithLogging = StandardUsingsWithLogging + @"

" + ExamplesNamespace;

    /// <summary>
    /// Combines standard usings (with extensions) with the examples namespace.
    /// </summary>
    public const string StandardModuleHeaderWithExtensions = StandardUsingsWithExtensions + @"

" + ExamplesNamespace;

    /// <summary>
    /// A simple async ExecuteAsync method body with Task.Delay.
    /// </summary>
    public const string SimpleAsyncExecuteBody = @"protected override async Task<List<string>?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await Task.Delay(1, cancellationToken);
        return new List<string>();
    }";
}

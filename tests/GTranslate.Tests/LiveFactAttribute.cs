using System.Runtime.CompilerServices;

namespace GTranslate.Tests;

internal sealed class LiveFactAttribute : FactAttribute
{
    public LiveFactAttribute([CallerFilePath] string? sourceFilePath = null, [CallerLineNumber] int sourceLineNumber = -1)
        : base(sourceFilePath, sourceLineNumber)
    {
        if (!string.Equals(Environment.GetEnvironmentVariable("RUN_TRANSLATION_LIVE_TESTS"), "true", StringComparison.OrdinalIgnoreCase))
        {
            Skip = "Set RUN_TRANSLATION_LIVE_TESTS=true to run live provider tests.";
        }
    }
}

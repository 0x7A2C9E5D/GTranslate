using System.Collections.Generic;
using JetBrains.Annotations;

namespace GTranslate.Results;

/// <summary>
/// Represents dictionary entries grouped by part of speech.
/// </summary>
[PublicAPI]
public sealed class DictionaryGroup : IDictionaryGroup
{
    internal DictionaryGroup(string? partOfSpeech, IReadOnlyList<IDictionaryEntry>? entries = null,
        IReadOnlyList<string>? definitions = null, IReadOnlyList<string>? synonyms = null)
    {
        PartOfSpeech = partOfSpeech;
        Entries = entries ?? [];
        Definitions = definitions ?? [];
        Synonyms = synonyms ?? [];
    }

    /// <inheritdoc/>
    public string? PartOfSpeech { get; }

    /// <inheritdoc/>
    public IReadOnlyList<IDictionaryEntry> Entries { get; }

    /// <inheritdoc/>
    public IReadOnlyList<string> Definitions { get; }

    /// <inheritdoc/>
    public IReadOnlyList<string> Synonyms { get; }
}

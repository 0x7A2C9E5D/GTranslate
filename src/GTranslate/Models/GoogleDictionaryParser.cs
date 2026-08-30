using System;
using System.Collections.Generic;
using System.Linq;
using GTranslate.Results;

namespace GTranslate.Models;

internal static class GoogleDictionaryParser
{
    public static DictionaryResult Parse(GoogleTranslationResultModel model, string source, Language targetLanguage, Language sourceLanguage, string service)
    {
        var groups = new List<IDictionaryGroup>();
        var dictionaryGroups = model.Dictionary ?? [];

        foreach (var modelGroup in dictionaryGroups)
        {
            var entries = modelGroup.Entries?
                .Where(static x => !string.IsNullOrWhiteSpace(x.Word))
                .Select(static IDictionaryEntry (x) => new DictionaryEntry(x.Word!, x.Score, x.Frequency, x.ReverseTranslations))
                .ToArray() ?? [];

            var definitions = model.Definitions?
                .Where(x => PartOfSpeechEquals(x.PartOfSpeech, modelGroup.PartOfSpeech))
                .SelectMany(static x => x.Entries ?? [])
                .Select(static x => x.Gloss)
                .Where(static x => !string.IsNullOrWhiteSpace(x))
                .Select(static x => x!)
                .Distinct(StringComparer.Ordinal)
                .ToArray() ?? [];

            var synonyms = model.Synsets?
                .Where(x => PartOfSpeechEquals(x.PartOfSpeech, modelGroup.PartOfSpeech))
                .SelectMany(static x => x.Entries ?? [])
                .SelectMany(static x => x.Synonyms ?? [])
                .Where(static x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.Ordinal)
                .ToArray() ?? [];

            groups.Add(new DictionaryGroup(modelGroup.PartOfSpeech, entries, definitions, synonyms));
        }

        if (groups.Count > 0)
        {
            var knownEntries = new HashSet<string>(groups.SelectMany(static x => x.Entries).Select(static x => x.Text), StringComparer.Ordinal);
            var alternatives = model.AlternativeTranslations?
                .SelectMany(static x => x.Alternatives ?? [])
                .Where(static x => !string.IsNullOrWhiteSpace(x.Word))
                .Where(x => knownEntries.Add(x.Word!))
                .Select(static IDictionaryEntry (x) => new DictionaryEntry(x.Word!, x.Score))
                .ToArray() ?? [];

            if (alternatives.Length > 0)
            {
                groups.Add(new DictionaryGroup(null, alternatives));
            }
        }

        var examples = model.Examples?.Examples?
            .Where(static x => !string.IsNullOrWhiteSpace(x.Text))
            .Select(static IDictionaryExample (x) => new DictionaryExample(x.Text!))
            .ToArray() ?? [];

        string? pronunciation = model.Sentences?
            .Select(static x => x.SourceTransliteration)
            .FirstOrDefault(static x => !string.IsNullOrWhiteSpace(x));

        return new DictionaryResult(source, service, targetLanguage, sourceLanguage, source, pronunciation, groups, examples);
    }

    private static bool PartOfSpeechEquals(string? left, string? right) => string.Equals(left, right, StringComparison.OrdinalIgnoreCase);
}

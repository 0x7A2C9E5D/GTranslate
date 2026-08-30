using GTranslate.Translators;

namespace GTranslate.Tests;

public sealed class DictionaryCapabilityTests
{
    [Fact]
    public void SupportedProvidersImplementDictionaryTranslator()
    {
        using var google = new GoogleTranslator();
        using var bing = new BingTranslator();
        using var microsoft = new MicrosoftTranslator();

        Assert.IsType<IDictionaryTranslator>(google, exactMatch: false);
        Assert.IsType<IDictionaryTranslator>(bing, exactMatch: false);
        Assert.IsType<IDictionaryTranslator>(microsoft, exactMatch: false);
    }

    [Fact]
    public void GoogleRpcDoesNotImplementDictionaryTranslator()
    {
        using var translator = new GoogleTranslator2();

        Assert.IsNotType<IDictionaryTranslator>(translator, exactMatch: false);
    }
}

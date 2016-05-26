namespace Elision.Seo.Pipelines.GetCanonicalUrl
{
    public interface IGetCanonicalUrlProcessor
    {
        void Process(GetCanonicalUrlArgs args);
    }
}
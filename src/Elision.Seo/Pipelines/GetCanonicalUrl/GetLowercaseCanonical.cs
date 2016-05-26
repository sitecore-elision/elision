namespace Elision.Seo.Pipelines.GetCanonicalUrl
{
    public class GetLowercaseCanonical : IGetCanonicalUrlProcessor
    {
        public void Process(GetCanonicalUrlArgs args)
        {
            if (!string.IsNullOrWhiteSpace(args.CanonicalUrl))
                args.CanonicalUrl = args.CanonicalUrl.ToLowerInvariant();
            else if (!string.IsNullOrWhiteSpace(args.RawUrl) && args.RawUrl != args.RawUrl.ToLowerInvariant())
                args.CanonicalUrl = args.RawUrl.ToLowerInvariant();
        }
    }
}
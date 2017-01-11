using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Elision.Foundation.Mvc.ValueProviders
{
    public class PipelineValueProvider : DictionaryValueProvider<object>
    {
        public PipelineValueProvider(IDictionary<string, object> dictionary, CultureInfo culture) : base(dictionary, culture)
        {
        }
    }
}

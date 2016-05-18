using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace Elision.Mvc.ValueProviders
{
    public class PipelineValueProvider : DictionaryValueProvider<object>
    {
        public PipelineValueProvider(IDictionary<string, object> dictionary, CultureInfo culture) : base(dictionary, culture)
        {
        }
    }
}

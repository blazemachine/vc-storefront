using System.Linq;
using System.Text.RegularExpressions;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.PostRenderTemplateChange.Operations
{
    public class ExternalLinksPostRenderTemplateOperation : IPostRenderTemplateChangeOperation
    {
        private readonly IWorkContextAccessor _workContextAccessor;

        public ExternalLinksPostRenderTemplateOperation(IWorkContextAccessor workContextAccessor)
        {
            _workContextAccessor = workContextAccessor;
        }

        private readonly Regex _linksTagsRegex = new Regex(@"<\s*a[^>]*>(.*?)<\s*/\s*a>", RegexOptions.Compiled);
        private readonly Regex _hrefAttrRegex = new Regex(@"(?<=\bhref\s*=\s*[""'])[^""']*", RegexOptions.Compiled);

        public string Run(string renderResult)
        {
            var matches = _linksTagsRegex.Matches(renderResult).Where(m => m.Success).Select(m => m.Value);
            foreach (var match in matches)
            {
                var hrefAttrValue = _hrefAttrRegex.Match(match).Value.Trim().ToUpper();
                var isHrefStartsWithCurrentStoreUrl = hrefAttrValue.StartsWith(_workContextAccessor.WorkContext.CurrentStore.Url.ToUpper());
                if ((hrefAttrValue.StartsWith("HTTP:") || hrefAttrValue.StartsWith("HTTPS:")) && !isHrefStartsWithCurrentStoreUrl)
                {
                    var matchWithRel = match.Replace("<a", "<a rel=\"nofollow\" target=\"_blank\"");
                    renderResult = renderResult.Replace(match, matchWithRel);
                }
            }
            return renderResult;
        }
    }
}

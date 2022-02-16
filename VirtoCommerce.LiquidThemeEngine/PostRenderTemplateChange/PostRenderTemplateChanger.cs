using System.Collections.Generic;
using VirtoCommerce.LiquidThemeEngine.PostRenderTemplateChange.Operations;
using VirtoCommerce.Storefront.Model;

namespace VirtoCommerce.LiquidThemeEngine.PostRenderTemplateChange
{
    public class PostRenderTemplateChanger : IPostRenderTemplateChange
    {
        private readonly IList<IPostRenderTemplateChangeOperation> _operations;

        public PostRenderTemplateChanger(IWorkContextAccessor workContextAccessor)
        {
            _operations = new[]
            {
                new ExternalLinksPostRenderTemplateOperation(workContextAccessor)
            };
        }

        public IList<IPostRenderTemplateChangeOperation> Operations => _operations;

        public string Change(string renderResult)
        {
            foreach (var operation in Operations)
            {
                renderResult = operation.Run(renderResult);
            }
            return renderResult;
        }
    }
}

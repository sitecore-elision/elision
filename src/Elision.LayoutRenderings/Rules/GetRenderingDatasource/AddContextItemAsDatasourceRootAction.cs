namespace Elision.LayoutRenderings.Rules.GetRenderingDatasource
{
    public class AddContextItemAsDatasourceRootAction<T> : AddDatasourceRootAction<T> where T : GetRenderingDatasourceRuleContext
    {
        public override void Apply(T ruleContext)
        {
            var contextItem = ruleContext.Args.RenderingItem.Database.GetItem(ruleContext.Args.ContextItemPath);
            if (contextItem == null)
                return;

            NewDatasourceRoot = contextItem;

            base.Apply(ruleContext);
        }
    }
}
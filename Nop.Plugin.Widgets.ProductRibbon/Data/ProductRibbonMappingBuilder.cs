using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.ProductRibbon.Domain;

namespace Nop.Plugin.Widgets.ProductRibbon.Data
{
    public class ProductRibbonMappingBuilder : NopEntityBuilder<ProductRibbonMapping>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(ProductRibbonMapping.ProductId)).AsInt32().NotNullable()
                .WithColumn(nameof(ProductRibbonMapping.ProductRibbonId)).AsInt32().NotNullable();
        }
    }
}

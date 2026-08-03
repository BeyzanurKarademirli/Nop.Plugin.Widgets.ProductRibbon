using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widgets.ProductRibbon.Domain;

namespace Nop.Plugin.Widgets.ProductRibbon.Data
{
    public class ProductRibbonBuilder : NopEntityBuilder<Domain.ProductRibbon>
    {
        public override void MapEntity(CreateTableExpressionBuilder table)
        {
            table
                .WithColumn(nameof(Domain.ProductRibbon.Text)).AsString(200).NotNullable()
                .WithColumn(nameof(Domain.ProductRibbon.BackgroundColor)).AsString(50).Nullable()
                .WithColumn(nameof(Domain.ProductRibbon.TextColor)).AsString(50).Nullable()
                .WithColumn(nameof(Domain.ProductRibbon.IsActive)).AsBoolean().NotNullable()
                .WithColumn(nameof(Domain.ProductRibbon.DisplayOrder)).AsInt32().NotNullable();
        }
    }
}

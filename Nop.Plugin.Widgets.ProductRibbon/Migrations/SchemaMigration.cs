using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widgets.ProductRibbon.Domain;

namespace Nop.Plugin.Widgets.ProductRibbon.Migrations
{
    /// <summary>
    /// ProductRibbon ve ProductRibbonMapping veritabanı tablolarını FluentMigrator ile oluşturan migration sınıfı.
    /// AutoReversingMigration sayesinde eklenti kaldırıldığında tablolar otomatik silinir.
    /// </summary>
    [NopMigration("2026/08/03 14:30:00", "ProductRibbon Tablo Oluşturma")]
    public class SchemaMigration : AutoReversingMigration
    {
        public override void Up()
        {
            Create.TableFor<Domain.ProductRibbon>();
            Create.TableFor<ProductRibbonMapping>();
        }
    }
}

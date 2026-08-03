using FluentValidation;
using Nop.Plugin.Widgets.ProductRibbon.Models;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widgets.ProductRibbon.Validators
{
    public partial class ProductRibbonValidator : BaseNopValidator<ProductRibbonModel>
    {
        public ProductRibbonValidator(ILocalizationService localizationService)
        {
            RuleFor(model => model.Text)
                .NotEmpty()
                .WithMessageAwait(localizationService.GetResourceAsync("Plugins.Widgets.ProductRibbon.TextRequired"));
        }
    }
}

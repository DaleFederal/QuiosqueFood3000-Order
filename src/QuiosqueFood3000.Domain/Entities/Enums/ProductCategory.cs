using System.ComponentModel;

namespace QuiosqueFood3000.Domain.Entities.Enums
{
    public enum ProductCategory
    {
        [Description("Lanche")] Sandwich,
        [Description("Acompanhamento")] Side,
        [Description("Bebida")] Drink,
        [Description("Sobremesa")] Dessert
    }
}
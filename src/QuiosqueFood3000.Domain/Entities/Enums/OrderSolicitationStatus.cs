using System.ComponentModel;

namespace QuiosqueFood3000.Domain.Entities.Enums
{
    public enum OrderSolicitationStatus
    {
        [Description("Em identificação")] InIdentification,
        [Description("Em montagem")] InProgress,
        [Description("Finalizado")] Finished,
        [Description("Cancelado")] Canceled
    }
}


using System.ComponentModel;

namespace QuiosqueFood3000.Domain.Entities.Enums
{
    public enum OrderStatus
    {
        [Description("Emitido")] Emitted,
        [Description("Recebido")] Received,
        [Description("Em preparação")] InProgress,
        [Description("Pronto")] Ready,
        [Description("Finalizado")] Finished,
        [Description("Cancelado")] Cancelled
    }
}

using System.ComponentModel;

namespace QuiosqueFood3000.Domain.Entities.Enums
{
    public enum PaymentStatus
    {
        [Description("Pendente Pagamento")] PendingPayment,
        [Description("Pago")] Payed,
        [Description("Não pago")] NotPayed
    }
}

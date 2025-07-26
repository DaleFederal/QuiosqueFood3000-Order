using System.ComponentModel;

namespace QuiosqueFood3000.Domain.Entities.Enums
{
    public enum TypeOfIdentification
    {
        [Description("Anônimo")] Anonymous,
        [Description("Identificação por CPF")] CpfIdentification
    }
}

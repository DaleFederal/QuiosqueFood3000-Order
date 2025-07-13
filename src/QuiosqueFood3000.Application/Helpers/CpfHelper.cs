namespace QuiosqueFood3000.Api.Helpers
{
    public static class CpfHelper
    {
        public static bool IsValidCpf(string cpf)
        {
            int[] multiplier1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplier2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            string tempCpf;
            string verificationDigit;

            int sum;
            int modulo;

            cpf = RemoveSpecialCaracters(cpf);

            if (cpf.Length != 11)
            {
                return false;
            }

            tempCpf = cpf.Substring(0, 9);
            sum = 0;

            for (int i = 0; i < 9; i++)
            {
                sum += int.Parse(tempCpf[i].ToString()) * multiplier1[i];
            }
            modulo = sum % 11;

            if (modulo < 2)
            {
                modulo = 0;
            }
            else
            {
                modulo = 11 - modulo;
            }
            verificationDigit = modulo.ToString();
            tempCpf = tempCpf + modulo;
            sum = 0;
            for (int i = 0; i < 10; i++)
            {
                sum += int.Parse(tempCpf[i].ToString()) * multiplier2[i];
            }
            modulo = sum % 11;
            if (modulo < 2)
            {
                modulo = 0;
            }
            else
            {
                modulo = 11 - modulo;
            }
            verificationDigit = verificationDigit + modulo.ToString();
            return cpf.EndsWith(verificationDigit);
        }
        public static string RemoveSpecialCaracters(string str)
        {
            return str.Trim().Replace(".", "").Replace("-", "");
        }
    }
}

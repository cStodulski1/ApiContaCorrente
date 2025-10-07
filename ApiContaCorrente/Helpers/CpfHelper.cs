namespace ApiContaCorrente.Helpers
{
    public static class CpfHelper
    {
        public static bool IsCpfValid(string cpf)
        {
            string cpfLimpo = cpf.Trim().Replace(".", "").Replace("-", "");

            if (cpfLimpo.Length != 11)
                return false;

            bool todosDigitosIguais = true;
            for (int i = 1; i < 11; i++)
            {
                if (cpfLimpo[i] != cpfLimpo[0])
                {
                    todosDigitosIguais = false;
                    break;
                }
            }
            if (todosDigitosIguais)
                return false;

            int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            string tempCpf = cpfLimpo.Substring(0, 9);
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            int digitoVerificador1 = resto < 2 ? 0 : 11 - resto;

            string digito = digitoVerificador1.ToString();
            tempCpf = tempCpf + digito;

            int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            int digitoVerificador2 = resto < 2 ? 0 : 11 - resto;

            digito = digito + digitoVerificador2.ToString();

            return cpfLimpo.EndsWith(digito);
        }
    }
}

namespace ApiContaCorrente.Helpers
{
    public class SenhaEncrypt
    {
        //alterar isso aqui pra utilizar salt individual de cada user
        public static string EncriptarSenha(string senha)
        {
            string hashSenha = BCrypt.Net.BCrypt.EnhancedHashPassword(senha, 13);
            return hashSenha;
        }

        public static bool VerificarSenha(string senha, string senhaHashSalva)
        {
            bool senhaCorreta = BCrypt.Net.BCrypt.EnhancedVerify(senha, senhaHashSalva);
            return senhaCorreta;
        }
    }
}

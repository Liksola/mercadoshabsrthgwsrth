namespace Loja
{

    using Microsoft.Data.Sqlite;

    public partial class Login : Form
    {
        private SQLiteConnectionManager connectionManager;

        public Login()
        {
            InitializeComponent();
            string databasePath = "loja.db";
            connectionManager = new SQLiteConnectionManager(databasePath);
            connectionManager.CriarTabelaFuncionarios();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string usuario = txtUsuario.Text;
            string senha = txtSenha.Text;

            // Valida o login no banco de dados
            string query = $"SELECT * FROM Funcionarios WHERE Usuario = @Usuario AND Senha = @Senha";
            using (var connection = connectionManager.GetConnection())
            {
                connectionManager.OpenConnection(connection);
                using (var command = new SqliteCommand(query, connection))
                {
                    // Usando par�metros para evitar SQL Injection
                    command.Parameters.AddWithValue("@Usuario", usuario);
                    command.Parameters.AddWithValue("@Senha", senha);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            // Login v�lido, abre a tela principal
                            MainForm mainForm = new MainForm();
                            this.Hide();
                            mainForm.Show();
                        }
                        else
                        {
                            // Exibe uma mensagem de erro se o login for incorreto
                            MessageBox.Show("Usu�rio ou senha incorretos. Tente novamente.", "Erro de Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                connectionManager.CloseConnection(connection);
            }

        }
        private void btnFuncionarios_Click(object sender, EventArgs e)
        {
            // Mostra uma caixa de di�logo pedindo a senha de administrador
            string senhaAdm = Microsoft.VisualBasic.Interaction.InputBox("Digite a senha de administrador:", "Acesso Restrito", "");

            // Valida se a senha � correta (no exemplo, "123")
            if (senhaAdm == "123")
            {
                // Se a senha for correta, abre a tela de gerenciamento de funcion�rios
                ConsultaFuncionariosForm consultaFuncionariosForm = new ConsultaFuncionariosForm();
                consultaFuncionariosForm.ShowDialog();
            }
            else
            {
                MessageBox.Show("Senha incorreta. Acesso negado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }

}

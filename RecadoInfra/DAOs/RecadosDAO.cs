using MySql.Data.MySqlClient;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RecadoModel;

namespace RecadosInfra.DAOs
{
    public class RecadosDAO
    {
        const string connectionString = "Server=localhost; User ID= root ; Password= Senha123!; Database= camillao";

        public async Task InserirAsync(RecModel recado)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                string sql = @"INSERT INTO recado (mensagem, remetente, destinatario) VALUES (@Mensagem, @Remetente, @Destinatario)";
                await conexao.ExecuteAsync(sql, recado);
            }
        }

        public async Task AlterarAsync(RecModel recado)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                string sql = @"UPDATE recado SET mensagem = @Mensagem, remetente = @Remetente, destinatario = @Destinatario WHERE id = @Id";
                await conexao.ExecuteAsync(sql, recado);
            }
        }

        public async Task DeletarAsync(int id)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                string sql = @"DELETE FROM recado WHERE id = @Id";
                await conexao.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task<RecModel> BuscarPorIdAsync(int id)
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                string sql = @"SELECT * FROM recado WHERE id = @Id";
                return await conexao.QueryFirstOrDefaultAsync<RecModel>(sql, new { Id = id });
            }
        }

        public async Task<List<RecModel>> ListarTodosAsync()
        {
            using (var conexao = new MySqlConnection(connectionString))
            {
                await conexao.OpenAsync();
                string sql = @"SELECT * FROM recado";
                var result = await conexao.QueryAsync<RecModel>(sql);
                return result.ToList();
            }
        }
    }
}
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Npgsql;


namespace Ftec.ProjetoWeb.Produtos.Persistencia
{
    public class CategoriaRepositorio : ICategoriaRepositorio
    {
        private string stringConexao;

        public CategoriaRepositorio(string stringConexao)
        {
            this.stringConexao = stringConexao;
        }

        public Response<Categoria> CriarCategoria(Categoria categoria)
        {
            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction())
                {
                    try
                    {
                        var comando = new NpgsqlCommand();
                        comando.Connection = conexao;
                        comando.Transaction = transacao;
                        comando.CommandText = "INSERT INTO categoria (nome, descricao, parent_id) VALUES (@nome, @descricao, @parent_id)";

                        comando.Parameters.AddWithValue("@nome", categoria.Nome);
                        comando.Parameters.AddWithValue("@descricao", categoria.Descricao);
                        comando.Parameters.AddWithValue(
                            "@parent_id",
                            categoria.Parent_Id ?? (object)DBNull.Value
                        );

                        comando.ExecuteNonQuery();
                        transacao.Commit();

                        return new Response<Categoria>
                        {
                            Sucesso = true,
                            Data = categoria,
                            Message = "Categoria criada com sucesso."
                        };
                    }
                    catch (Exception ex)
                    {
                        transacao.Rollback();
                        return new Response<Categoria>
                        {
                            Sucesso = false,
                            Data = null,
                            Message = $"Erro ao criar categoria: {ex.Message}"
                        };
                        throw ex;
                    }
                }
            }
        }
        public Response<Categoria> AlterarCategoria(Categoria categoria)
        {
            var verificacao = this.ObterPorId(categoria.Id);
            if (verificacao != null)
            {
                using (var conexao = new NpgsqlConnection(stringConexao))
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        try
                        {
                            var comando = new NpgsqlCommand();
                            comando.Connection = conexao;
                            comando.Transaction = transacao;

                            comando.CommandText = "UPDATE categoria SET nome = @nome, descricao = @descricao, parent_id = @parent_id WHERE id = @id";
                            comando.Parameters.AddWithValue("@id", categoria.Id);
                            comando.Parameters.AddWithValue("@nome", categoria.Nome);
                            comando.Parameters.AddWithValue("@descricao", categoria.Descricao);
                            comando.Parameters.AddWithValue(
                                "@parent_id",
                                categoria.Parent_Id ?? (object)DBNull.Value
                            );
                            comando.ExecuteNonQuery();
                            transacao.Commit();
                            return new Response<Categoria>
                            {
                                Sucesso = true,
                                Data = categoria,
                                Message = "Categoria alterada com sucesso."
                            };
                        }
                        catch (Exception ex)
                        {
                            transacao.Rollback();
                            return new Response<Categoria>
                            {
                                Sucesso = false,
                                Data = null,
                                Message = $"Erro ao alterar categoria: {ex.Message}"
                            };
                        }
                    }
                }
            }
            else
            {
                return new Response<Categoria>
                {
                    Sucesso = false,
                    Data = null,
                    Message = "Erro ao alterar categoria."
                };
            }
        }

        public bool ExcluirCategoria(int id)
        {
            var verificacao = this.ObterPorId(id);
            if (verificacao != null)
            {
                using (var conexao = new NpgsqlConnection(stringConexao))
                {
                    conexao.Open();
                    using (var transacao = conexao.BeginTransaction())
                    {
                        try
                        {
                            var comando = new NpgsqlCommand();
                            comando.Connection = conexao;
                            comando.Transaction = transacao;
                            comando.CommandText = "DELETE FROM categoria WHERE id = @id";
                            comando.Parameters.AddWithValue("@id", id);
                            comando.ExecuteNonQuery();
                            transacao.Commit();
                            return true;
                        }
                        catch (Exception ex)
                        {
                            transacao.Rollback();
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }
        }

        public Categoria ObterPorId(int id)
        {
            Categoria categoria = null;
            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText = "SELECT id, nome, descricao, parent_id FROM categoria WHERE id = @id";
                comando.Parameters.AddWithValue("@id", id);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        categoria = new Categoria();
                        categoria.Id = reader.GetInt32(0);
                        categoria.Nome = reader.GetString(1);
                        categoria.Descricao = reader.GetString(2);
                        categoria.Parent_Id = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                    }
                }
            }
            return categoria;
        }

        public List<Categoria> ObterTodos()
        {
            List<Categoria> categorias = new List<Categoria>();
            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();
                var comando = new NpgsqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "SELECT id, nome, descricao, parent_id FROM categoria";
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria();
                        categoria.Id = reader.GetInt32(0);
                        categoria.Nome = reader.GetString(1);
                        categoria.Descricao = reader.GetString(2);
                        categoria.Parent_Id = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                        categorias.Add(categoria);
                    }
                }
            }
            return categorias;
        }

        public List<Categoria> ObterPorTexto(string texto)
        {
            List<Categoria> categorias = new List<Categoria>();
            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();
                var comando = new NpgsqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "SELECT id, nome, descricao, parent_id FROM categoria WHERE nome ILIKE @texto OR descricao ILIKE @texto";
                comando.Parameters.AddWithValue("@texto", $"%{texto}%");
                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Categoria categoria = new Categoria();
                        categoria.Id = reader.GetInt32(0);
                        categoria.Nome = reader.GetString(1);
                        categoria.Descricao = reader.GetString(2);
                        categoria.Parent_Id = reader.IsDBNull(3) ? null : reader.GetInt32(3);
                        categorias.Add(categoria);
                    }
                }
            }
            return categorias;
        }
    }
}

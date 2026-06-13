using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Npgsql;

namespace Ftec.ProjetoWeb.Produtos.Persistencia
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {

        private string stringConexao;
        public ProdutoRepositorio(string strConexao)
        {
            stringConexao = strConexao;
        }
        public Response<Produto> CriarProduto(Produto produto)
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

                        comando.CommandText =
                            "INSERT INTO " +
                            "public.produtos(id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, idImagemPrincipal, descricao, disponivel, destaque,  excluido) " +
                            "VALUES (@id, @codigo, @nome, @preco, @quantidadeEstoque, @estoqueMinimoVenda, @idCategoria, @idImagemPrincipal, @descricao, @disponivel, @destaque, @excluido);";
                        comando.Parameters.AddWithValue("id", produto.Id);
                        comando.Parameters.AddWithValue("codigo", produto.Codigo);
                        comando.Parameters.AddWithValue("nome", produto.Nome);
                        comando.Parameters.AddWithValue("preco", produto.Preco);
                        comando.Parameters.AddWithValue("quantidadeEstoque", produto.QuantidadeEstoque);
                        comando.Parameters.AddWithValue("estoqueMinimoVenda", produto.EstoqueMinimoVenda);
                        comando.Parameters.AddWithValue("idCategoria", produto.IdCategoria);
                        comando.Parameters.AddWithValue("idImagemPrincipal", produto.IdImagemPrincipal);
                        comando.Parameters.AddWithValue("descricao", produto.Descricao);
                        comando.Parameters.AddWithValue("disponivel", produto.Disponivel);
                        comando.Parameters.AddWithValue("excluido", false);
                        comando.Parameters.AddWithValue("destaque", produto.Destaque);
                        comando.ExecuteNonQuery();

                        transacao.Commit();
                        return new Response<Produto>
                        {
                            Sucesso = true,
                            Data = produto,
                            Message = "Produto criado com sucesso"
                        };
                    }
                    catch (Exception e)
                    {
                        transacao.Rollback();
                        return new Response<Produto>
                        {
                            Sucesso = false,
                            Data = null,
                            Message = $"Erro ao criar produto: {e.Message}"
                        };
                        throw e;
                    }
                }
            }
        }
        public Response<Produto> AlterarProduto(Produto produto)
        {

            var verificacao = this.ObtemPorId(produto.Id.ToString());
            if (!verificacao.Excluido)
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

                            comando.CommandText =
                                "UPDATE public.produtos SET " +
                                "codigo = @codigo, nome = @nome, preco = @preco, quantidadeEstoque = @quantidadeEstoque, estoqueMinimoVenda = @estoqueMinimoVenda, idCategoria = @idCategoria, idImagemPrincipal = @idImagemPrincipal, descricao = @descricao, disponivel = @disponivel, destaque = @destaque " +
                                "WHERE id = @id;";


                            comando.Parameters.AddWithValue("id", produto.Id);
                            comando.Parameters.AddWithValue("codigo", produto.Codigo);
                            comando.Parameters.AddWithValue("nome", produto.Nome);
                            comando.Parameters.AddWithValue("preco", produto.Preco);
                            comando.Parameters.AddWithValue("quantidadeEstoque", produto.QuantidadeEstoque);
                            comando.Parameters.AddWithValue("estoqueMinimoVenda", produto.EstoqueMinimoVenda);
                            comando.Parameters.AddWithValue("idCategoria", produto.IdCategoria);
                            comando.Parameters.AddWithValue("idImagemPrincipal", produto.IdImagemPrincipal);
                            comando.Parameters.AddWithValue("descricao", produto.Descricao);
                            comando.Parameters.AddWithValue("disponivel", produto.Disponivel);
                            comando.Parameters.AddWithValue("destaque", produto.Destaque);
                            comando.ExecuteNonQuery();

                            transacao.Commit();

                            return new Response<Produto>
                            {
                                Sucesso = true,
                                Data = produto,
                                Message = "Produto alterado com sucesso"
                            };

                        }
                        catch (Exception e)
                        {
                            transacao.Rollback();
                            return new Response<Produto>
                            {
                                Sucesso = false,
                                Data = null,
                                Message = $"Erro ao alterar produto: {e.Message}"
                            };
                        }
                    }
                }
            }
            else
            {
                return new Response<Produto>
                {
                    Sucesso = false,
                    Data = null,
                    Message = $"Erro ao alterar produto: Não é possível alterar um produto excluído"
                };
            }
        }
        public bool ExcluirProduto(string id)
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
                        comando.CommandText =
                            "UPDATE public.produtos " +
                            "SET excluido = true " +
                            "WHERE id = @id;";

                        comando.Parameters.AddWithValue("id", Guid.Parse(id));

                        comando.ExecuteNonQuery();

                        transacao.Commit();

                        // opcional: valida se realmente encontrou o produto
                        return true;

                    }
                    catch (Exception e)
                    {
                        transacao.Rollback();
                        return false;
                    }
                }
            }
        }
        public Produto ObtemPorCodigo(string codigo)
        {
            Produto produto = null;

            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText =
                    "SELECT id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, idImagemPrincipal, descricao, disponivel, excluido, destaque " +
                    "FROM public.produtos " +
                    "WHERE codigo = @codigo;";

                comando.Parameters.AddWithValue("codigo", codigo);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        produto = new Produto();
                        produto.Id = Guid.Parse(reader["id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Nome = reader["nome"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);
                        produto.QuantidadeEstoque = Convert.ToInt32(reader["quantidadeestoque"]);
                        produto.EstoqueMinimoVenda = Convert.ToInt32(reader["estoqueminimovenda"]);
                        produto.IdCategoria = Convert.ToInt32(reader["idcategoria"]);
                        produto.IdImagemPrincipal = Guid.Parse(reader["idImagemPrincipal"].ToString());
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Disponivel = Convert.ToBoolean(reader["disponivel"]);
                        produto.Excluido = Convert.ToBoolean(reader["excluido"]);
                        produto.Destaque = Convert.ToBoolean(reader["destaque"]);
                    }
                }
            }

            return produto;
        }
        public Produto ObtemPorId(string id)
        {
            Produto produto = null;

            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText =
                    "SELECT id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, idImagemPrincipal, descricao, disponivel, excluido, destaque " +
                    "FROM public.produtos " +
                    "WHERE id = @id;";

                comando.Parameters.AddWithValue("id", Guid.Parse(id));

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        produto = new Produto();
                        produto.Id = Guid.Parse(reader["id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Nome = reader["nome"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);
                        produto.QuantidadeEstoque = Convert.ToInt32(reader["quantidadeestoque"]);
                        produto.EstoqueMinimoVenda = Convert.ToInt32(reader["estoqueminimovenda"]);
                        produto.IdCategoria = Convert.ToInt32(reader["idcategoria"]);
                        produto.IdImagemPrincipal = Guid.Parse(reader["idImagemPrincipal"].ToString());
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Disponivel = Convert.ToBoolean(reader["disponivel"]);
                        produto.Excluido = Convert.ToBoolean(reader["excluido"]);
                        produto.Destaque = Convert.ToBoolean(reader["destaque"]);
                    }
                }
            }

            return produto;
        }
        public List<Produto> ProcurarPorTexto(string texto)
        {
            List<Produto> list = new List<Produto>();

            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText =
                    "SELECT id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, idImagemPrincipal, descricao, disponivel, excluido, destaque " +
                    "FROM public.produtos " +
                    "WHERE codigo ILIKE @texto OR nome ILIKE @texto OR descricao ILIKE @texto and excluido = false;";

                comando.Parameters.AddWithValue("texto", $"%{texto}%");

                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var produto = new Produto();

                        produto.Id = Guid.Parse(reader["id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Nome = reader["nome"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);
                        produto.QuantidadeEstoque = Convert.ToInt32(reader["quantidadeestoque"]);
                        produto.EstoqueMinimoVenda = Convert.ToInt32(reader["estoqueminimovenda"]);
                        produto.IdCategoria = Convert.ToInt32(reader["idcategoria"]);
                        produto.IdImagemPrincipal = Guid.Parse(reader["idImagemPrincipal"].ToString());
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Disponivel = Convert.ToBoolean(reader["disponivel"]);
                        produto.Excluido = Convert.ToBoolean(reader["excluido"]);
                        produto.Destaque = Convert.ToBoolean(reader["destaque"]);

                        list.Add(produto);
                    }
                }
            }

            return list;
        }
        public List<Produto> ListaTodosProdutos()
        {
            List<Produto> list = new List<Produto>();
            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "SELECT id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, idImagemPrincipal, descricao, disponivel, excluido, destaque " +
                    "FROM public.produtos WHERE excluido = false;";

                using (var reader = comando.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Produto produto = new Produto();
                        produto.Id = Guid.Parse(reader["id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Nome = reader["nome"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);
                        produto.QuantidadeEstoque = Convert.ToInt32(reader["quantidadeestoque"]);
                        produto.EstoqueMinimoVenda = Convert.ToInt32(reader["estoqueminimovenda"]);
                        produto.IdCategoria = Convert.ToInt32(reader["idcategoria"]);
                        produto.IdImagemPrincipal = Guid.Parse(reader["idImagemPrincipal"].ToString());
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Disponivel = Convert.ToBoolean(reader["disponivel"]);
                        produto.Excluido = Convert.ToBoolean(reader["excluido"]);
                        produto.Destaque = Convert.ToBoolean(reader["destaque"]);

                        list.Add(produto);
                    }
                }
            }

            return list;
        }
    }
}

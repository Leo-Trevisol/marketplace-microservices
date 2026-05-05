using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Persistencia {
    public class ProdutoRepositorio : IProdutoRepositorio{

        private string stringConexao;

        public ProdutoRepositorio(string strConexao) {
            stringConexao = strConexao;
        }
        public void CriarProduto(Produto produto) {
            using (var conexao = new NpgsqlConnection(stringConexao)) {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction()) {
                    try {
                        var comando = new NpgsqlCommand();
                        comando.Connection = conexao;
                        comando.Transaction = transacao;

                        comando.CommandText = 
                            "INSERT INTO " +
                            "public.produto(id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, descricao, disponivel) " +
                            "VALUES (@id, @codigo, @nome, @preco, @quantidadeEstoque, @estoqueMinimoVenda, @idCategoria, @descricao, @disponivel);";
                        comando.Parameters.AddWithValue("id", produto.Id);
                        comando.Parameters.AddWithValue("codigo", produto.Codigo);
                        comando.Parameters.AddWithValue("nome", produto.Nome);
                        comando.Parameters.AddWithValue("preco", produto.Preco);
                        comando.Parameters.AddWithValue("quantidadeEstoque", produto.QuantidadeEstoque);
                        comando.Parameters.AddWithValue("estoqueMinimoVenda", produto.EstoqueMinimoVenda);
                        comando.Parameters.AddWithValue("idCategoria", produto.IdCategoria);
                        comando.Parameters.AddWithValue("descricao", produto.Descricao);
                        comando.Parameters.AddWithValue("disponivel", produto.Disponivel);
                        comando.ExecuteNonQuery();

                        transacao.Commit();
                    } catch (Exception e) {
                        transacao.Rollback();
                        throw e;
                    }
                }
            }
        }
        public void AlterarProduto(Produto produto) {
            using (var conexao = new NpgsqlConnection(stringConexao)) {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction()) {
                    try {
                        var comando = new NpgsqlCommand();
                        comando.Connection = conexao;
                        comando.Transaction = transacao;

                        comando.CommandText =
                            "UPDATE public.produto SET " +
                            "codigo = @codigo, nome = @nome, preco = @preco, quantidadeEstoque = @quantidadeEstoque, estoqueMinimoVenda = @estoqueMinimoVenda, idCategoria = @idCategoria, descricao = @descricao, disponivel = @disponivel " +
                            "WHERE id = @id;";

                        comando.Parameters.AddWithValue("id", produto.Id);
                        comando.Parameters.AddWithValue("codigo", produto.Codigo);
                        comando.Parameters.AddWithValue("nome", produto.Nome);
                        comando.Parameters.AddWithValue("preco", produto.Preco);
                        comando.Parameters.AddWithValue("quantidadeEstoque", produto.QuantidadeEstoque);
                        comando.Parameters.AddWithValue("estoqueMinimoVenda", produto.EstoqueMinimoVenda);
                        comando.Parameters.AddWithValue("idCategoria", produto.IdCategoria);
                        comando.Parameters.AddWithValue("descricao", produto.Descricao);
                        comando.Parameters.AddWithValue("disponivel", produto.Disponivel);
                        comando.ExecuteNonQuery();

                        transacao.Commit();

                    } catch (Exception e) {
                        transacao.Rollback();
                        throw e;
                    }
                }
            }
        }
        public void ExcluirProduto(string codigo) {
            using (var conexao = new NpgsqlConnection(stringConexao)) {
                conexao.Open();
                using (var transacao = conexao.BeginTransaction()) {
                    try {
                        var comando = new NpgsqlCommand();
                        comando.Connection = conexao;
                        comando.Transaction = transacao;

                        comando.CommandText = "DELETE FROM public.produto WHERE codigo = @codigo;";
                        comando.Parameters.AddWithValue("codigo", codigo);
                        comando.ExecuteNonQuery();

                        transacao.Commit();
                    } catch (Exception e) {
                        transacao.Rollback();
                        throw e;
                    }
                }
            }
        }
        public Produto ObtemPorCodigo(string codigo) {
            Produto produto = null;

            using (var conexao = new NpgsqlConnection(stringConexao)) {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText = 
                    "SELECT id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, descricao, disponivel " +
                    "FROM public.produto " +
                    "WHERE codigo = @codigo;";

                comando.Parameters.AddWithValue("codigo", codigo);

                using (var reader = comando.ExecuteReader()) {
                    if (reader.Read()) {
                        produto = new Produto();
                        produto.Id = Guid.Parse(reader["id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Nome = reader["nome"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);
                        produto.QuantidadeEstoque = Convert.ToInt32(reader["quantidadeestoque"]);
                        produto.EstoqueMinimoVenda = Convert.ToInt32(reader["estoqueminimovenda"]);
                        produto.IdCategoria = Guid.Parse(reader["idcategoria"].ToString());
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Disponivel = Convert.ToBoolean(reader["disponivel"]);
                    }
                }
            }

            return produto;
        }
        public List<Produto> ProcurarPorTexto(string texto) {
            List<Produto> list = new List<Produto>();

            using (var conexao = new NpgsqlConnection(stringConexao)) {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText =
                    "SELECT id, codigo, nome, preco, quantidadeEstoque, estoqueMinimoVenda, idCategoria, descricao, disponivel " +
                    "FROM public.produto " +
                    "WHERE codigo LIKE @texto OR nome LIKE @texto OR descricao LIKE @texto;";

                comando.Parameters.AddWithValue("texto", $"%{texto}%");

                using (var reader = comando.ExecuteReader()) {
                    while (reader.Read()) {
                        var produto = new Produto();

                        produto.Id = Guid.Parse(reader["id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Nome = reader["nome"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);
                        produto.QuantidadeEstoque = Convert.ToInt32(reader["quantidadeestoque"]);
                        produto.EstoqueMinimoVenda = Convert.ToInt32(reader["estoqueminimovenda"]);
                        produto.IdCategoria = Guid.Parse(reader["idcategoria"].ToString());
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Disponivel = Convert.ToBoolean(reader["disponivel"]);

                        list.Add(produto);
                    }
                }
            }

            return list;
        }
        public List<Produto> ListaTodosProdutos() {
            List<Produto> list = new List<Produto>();
            using (var conexao = new NpgsqlConnection(stringConexao)) {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;
                comando.CommandText = "SELECT id, codigo, descricao, preco FROM public.produto;";

                using (var reader = comando.ExecuteReader()) {
                    while (reader.Read()) {
                        Produto produto = new Produto();
                        produto.Id = Guid.Parse(reader["Id"].ToString());
                        produto.Codigo = reader["codigo"].ToString();
                        produto.Descricao = reader["descricao"].ToString();
                        produto.Preco = Convert.ToDecimal(reader["preco"]);

                        list.Add(produto);
                    }
                }
            }

            return list;
        }
    }
}

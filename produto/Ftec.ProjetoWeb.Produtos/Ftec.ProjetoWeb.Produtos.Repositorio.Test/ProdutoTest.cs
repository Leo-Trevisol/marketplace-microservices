using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Ftec.ProjetoWeb.Produtos.Persistencia;
using Microsoft.Testing.Platform.Configurations;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Ftec.ProjetoWeb.Produtos.Repositorio.Test
{
    [TestClass]
    public sealed class ProdutoTest
    {
        ProdutoRepositorio repositorio;
        public ProdutoTest(IConfiguration config) {
            repositorio = new ProdutoRepositorio(config["strConexao"]);
        }
        private Produto CriarProdutoTeste() {
            return new Produto {
                Id = Guid.NewGuid(),
                Codigo = "PROD-" + new Random().Next(1000, 9999),
                Nome = "Cadeira Ergonômica Teste",
                Descricao = "Produto de teste",
                Preco = 100.50m,
                QuantidadeEstoque = 5,
                EstoqueMinimoVenda = 3,
                IdCategoria = Guid.NewGuid(),
                Disponivel = true
            };
        }

        [TestMethod]
        public void TestInserirProduto() {
            var produto = CriarProdutoTeste();
            try {
                repositorio.CriarProduto(produto);
                Assert.IsTrue(true, "Produto inserido com sucesso.");
            } catch (Exception ex) {
                Assert.Fail($"Exceção lançada durante a inserção: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestAlterarProduto() {
            var produto = CriarProdutoTeste();

            repositorio.CriarProduto(produto);

            produto.Descricao = "Produto alterado";
            produto.Preco = 200.00m;
            produto.EstoqueMinimoVenda = 10;

            try {
                repositorio.AlterarProduto(produto);
                Assert.IsTrue(true, "Produto alterado com sucesso.");
            } catch (Exception ex) {
                Assert.Fail($"Exceção lançada durante a alteração: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestExcluirProduto() {
            var produto = CriarProdutoTeste();

            repositorio.CriarProduto(produto);

            try {
                repositorio.ExcluirProduto(produto.Codigo);
                Assert.IsTrue(true, "Produto excluído com sucesso.");
            } catch (Exception ex) {
                Assert.Fail($"Exceção lançada durante a exclusão: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestObterProduto() {
            var produto = CriarProdutoTeste();

            repositorio.CriarProduto(produto);

            try {
                var produtoObtido = repositorio.ObtemPorCodigo(produto.Codigo);
                Assert.IsNotNull(produtoObtido, "Produto obtido com sucesso.");
            } catch (Exception ex) {
                Assert.Fail($"Exceção lançada durante a obtenção: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestProcurarProdutoPorTexto() {
            var produto = CriarProdutoTeste();
            repositorio.CriarProduto(produto);
            try {
                var produtosEncontrados = repositorio.ProcurarPorTexto("Cadeira");
                Assert.IsTrue(produtosEncontrados.Any(p => p.Codigo == produto.Codigo), "Produto encontrado por texto.");
            } catch (Exception ex) {
                Assert.Fail($"Exceção lançada durante a busca por texto: {ex.Message}");
            }
        }

        [TestMethod]
        public void TestListarProdutos() {
            var produto = CriarProdutoTeste();
            repositorio.CriarProduto(produto);
            try {
                var produtos = repositorio.ListaTodosProdutos();
                Assert.IsTrue(produtos.Any(p => p.Codigo == produto.Codigo), "Produto listado com sucesso.");
            } catch (Exception ex) {
                Assert.Fail($"Exceção lançada durante a listagem: {ex.Message}");
            }
        }
    }
}



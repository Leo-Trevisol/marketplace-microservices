using Ftec.ProjetoWeb.Produtos.Aplicacao.Adapter;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Ftec.ProjetoWeb.Produtos.Persistencia;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Aplicacao {
    public class ProdutoAplicacao {

        IProdutoRepositorio produtoRepositorio;

        public ProdutoAplicacao(string strConexao) {
            produtoRepositorio = new ProdutoRepositorio(strConexao);
        }

        public Response<Produto> AdicionarProduto(ProdutoDTO produto) {
            Produto prod = ProdutoAdapter.ModelToEntity(produto);

            if (string.IsNullOrEmpty(prod.Codigo))
                throw new Exception("O código do produto é obrigatório.");

            if (string.IsNullOrEmpty(prod.Nome))
                throw new Exception("O nome do produto é obrigatório.");

            if (prod.Preco <= 0)
                throw new Exception("O preço do produto deve ser maior que zero.");

            if (prod.QuantidadeEstoque < 0)
                throw new Exception("A quantidade de estoque não pode ser menor que zero.");

            if (prod.EstoqueMinimoVenda <= 0)
                throw new Exception("A quantidade de estoque mínimo não pode ser menor que zero.");

            if (prod.QuantidadeEstoque < prod.EstoqueMinimoVenda)
                prod.Disponivel = false;

            var response = produtoRepositorio.CriarProduto(prod);
            return response;
        }
        public Response<Produto> AlterarProduto(ProdutoDTO produto) {
            Produto prod = ProdutoAdapter.ModelToEntity(produto);

            if (string.IsNullOrEmpty(prod.Codigo))
                throw new Exception("O código do produto é obrigatório.");

            if (string.IsNullOrEmpty(prod.Nome))
                throw new Exception("O nome do produto é obrigatório.");

            if (prod.QuantidadeEstoque < 0)
                throw new Exception("A quantidade de estoque não pode ser menor que zero.");

            if (prod.EstoqueMinimoVenda <= 0)
                throw new Exception("A quantidade de estoque mínimo não pode ser menor que zero.");

            if (prod.QuantidadeEstoque < prod.EstoqueMinimoVenda)
                prod.Disponivel = false;

            var response = produtoRepositorio.AlterarProduto(prod);

            return response;
        }
        public bool ExcluirProduto(string codigo) {
            if (string.IsNullOrEmpty(codigo))
                throw new Exception("O código do produto é obrigatório para exclusão.");

            return produtoRepositorio.ExcluirProduto(codigo);
        }
        public ProdutoDTO ObterProduto(string codigo) {
            Produto prod = produtoRepositorio.ObtemPorCodigo(codigo);
            if (prod == null)
                throw new Exception("Produto não encontrado.");

            return ProdutoAdapter.EntityToModel(prod);
        }
        public List<ProdutoDTO> ProcurarPorTexto(string texto) {
            if(string.IsNullOrEmpty(texto))
                throw new Exception("Texto de busca não informado.");

            List<Produto> produtos = produtoRepositorio.ProcurarPorTexto(texto);
            List<ProdutoDTO> dtos = new List<ProdutoDTO>();
            if(dtos == null) {
                throw new Exception("Nenhum produto encontrado. Altere o termo de busca, ou revise o texto informado.");
            } else {
                foreach (Produto prod in produtos)
                    dtos.Add(ProdutoAdapter.EntityToModel(prod));
                return dtos;
            }
        }
        public List<ProdutoDTO> ListarProdutos() {
            List<Produto> produtos = produtoRepositorio.ListaTodosProdutos();
            List<ProdutoDTO> dtos = new List<ProdutoDTO>();
            foreach (Produto prod in produtos)
                dtos.Add(ProdutoAdapter.EntityToModel(prod));
            return dtos;
        }
    }
}

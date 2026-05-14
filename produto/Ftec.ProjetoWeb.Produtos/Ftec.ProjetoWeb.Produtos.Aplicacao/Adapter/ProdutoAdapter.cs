using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Aplicacao.Adapter
{
    public static class ProdutoAdapter
    {
        public static Produto ModelToEntity(ProdutoDTO model) {
            if(model == null) {
                return null;
            } 

            Produto entity = new Produto();

            entity.Codigo = model.Codigo;
            entity.Nome = model.Nome;
            entity.Preco = model.Preco;
            entity.QuantidadeEstoque = model.QuantidadeEstoque;
            entity.EstoqueMinimoVenda = model.EstoqueMinimoVenda;
            entity.IdCategoria = model.IdCategoria;
            entity.IdImagemPrincipal = model.IdImagemPrincipal;
            entity.Descricao = model.Descricao;
            entity.Disponivel = model.Disponivel;
            entity.Excluido = model.Excluido;

            return entity;
        }

        public static ProdutoDTO EntityToModel(Produto entity) {

            if (entity == null) {
                return null;
            }

            ProdutoDTO model = new ProdutoDTO();

            model.Codigo = entity.Codigo;
            model.Nome = entity.Nome;
            model.Preco = entity.Preco;
            model.QuantidadeEstoque = entity.QuantidadeEstoque;
            model.EstoqueMinimoVenda = entity.EstoqueMinimoVenda;
            model.IdCategoria = entity.IdCategoria;
            model.IdImagemPrincipal = entity.IdImagemPrincipal;
            model.Descricao = entity.Descricao;
            model.Disponivel = entity.Disponivel;
            model.Excluido = entity.Excluido;

            return model;
        }

       
    }
}



using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;

namespace Ftec.ProjetoWeb.Produtos.Aplicacao.Adapter
{
    public static class CategoriaAdapter
    {
        public static Categoria ModelToEntity(CategoriaDTO model)
        {
            if (model == null)
            {
                return null;
            }

            Categoria entity = new Categoria();

            entity.Id = model.Id;
            entity.Nome = model.Nome;
            entity.Descricao = model.Descricao;
            entity.Parent_Id = model.Parent_Id;

            return entity;
        }

        public static CategoriaDTO EntityToModel(Categoria entity)
        {
            if (entity == null)
            {
                return null;
            }
            CategoriaDTO model = new CategoriaDTO();

            model.Id = entity.Id;
            model.Nome = entity.Nome;
            model.Descricao = entity.Descricao;
            model.Parent_Id = entity.Parent_Id;

            return model;
        }
    }
}


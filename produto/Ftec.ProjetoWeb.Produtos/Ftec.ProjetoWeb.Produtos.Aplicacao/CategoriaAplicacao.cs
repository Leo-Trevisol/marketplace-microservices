using Ftec.ProjetoWeb.Produtos.Aplicacao.Adapter;
using Ftec.ProjetoWeb.Produtos.Aplicacao.DTO;
using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Ftec.ProjetoWeb.Produtos.Persistencia;


namespace Ftec.ProjetoWeb.Produtos.Aplicacao
{
    public class CategoriaAplicacao
    {
        ICategoriaRepositorio categoriaRepositorio;

        public CategoriaAplicacao(string strConexao)
        {
            categoriaRepositorio = new CategoriaRepositorio(strConexao);
        }

        public Response<Categoria> AdicionarCategoria(CategoriaDTO categoria)
        {
            Categoria cat = CategoriaAdapter.ModelToEntity(categoria);
            if (string.IsNullOrEmpty(cat.Nome))
            {
                throw new Exception("O nome da categoria não pode ser vazio.");
            }

            var response = categoriaRepositorio.CriarCategoria(cat);

            return response;
        }

        public Response<Categoria> AlterarCategoria(CategoriaDTO categoria)
        {
            Categoria cat = CategoriaAdapter.ModelToEntity(categoria);
            if (string.IsNullOrEmpty(cat.Nome))
            {
                throw new Exception("O nome da categoria não pode ser vazio.");
            }

            var response = categoriaRepositorio.AlterarCategoria(cat);
            return response;
        }

        public bool ExcluirCategoria(int id)
        {
            if (id == 0 || id == null)
            {
                throw new Exception("O id da categoria não pode ser nulo.");
            }

            return categoriaRepositorio.ExcluirCategoria(id);
        }

        public CategoriaDTO ObterCategoria(int id)
        {
            if (id == 0 || id == null)
            {
                throw new Exception("O id da categoria não pode ser nulo.");
            }
            Categoria categoria = categoriaRepositorio.ObterPorId(id);

            return CategoriaAdapter.EntityToModel(categoria);
        }

        public List<CategoriaDTO> ObterCategoriasPorTexto(string texto)
        {
            if (string.IsNullOrEmpty(texto))
            {
                throw new Exception("Texto de busca não informado.");
            }

            List<Categoria> categorias = categoriaRepositorio.ObterPorTexto(texto);
            List<CategoriaDTO> dtos = new List<CategoriaDTO>();
            if (dtos == null)
            {
                throw new Exception("Nenhuma categoria encontrada para o texto informado.");
            }
            else
            {
                foreach (Categoria cat in categorias)
                {
                    dtos.Add(CategoriaAdapter.EntityToModel(cat));
                }
            }
            return dtos;
        }

        public List<CategoriaDTO> ObterCategorias()
        {
            List<Categoria> categorias = categoriaRepositorio.ObterTodos();
            List<CategoriaDTO> dtos = new List<CategoriaDTO>();
            foreach (Categoria cat in categorias)
            {
                dtos.Add(CategoriaAdapter.EntityToModel(cat));
            }
            return dtos;
        }

        public List<CategoriaArvoreDTO> ObterArvoreCategorias()
        {
            var categorias = categoriaRepositorio.ObterTodos();

            return MontarArvore(categorias, null);
        }

        private List<CategoriaArvoreDTO> MontarArvore(List<Categoria> categorias, int? parentId)
        {
            return categorias
           .Where(c => c.Parent_Id == parentId)
           .Select(c => new CategoriaArvoreDTO
           {
               Id = c.Id,
               Nome = c.Nome,
               Filhos = MontarArvore(categorias, c.Id)
           })
           .ToList();
        }
    }
}

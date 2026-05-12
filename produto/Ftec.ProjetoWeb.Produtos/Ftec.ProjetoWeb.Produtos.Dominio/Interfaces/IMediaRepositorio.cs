using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ftec.ProjetoWeb.Produtos.Dominio.Interfaces {
    public interface IMediaRepositorio {

        Response<Media> InserirMedia(Media media);
        Response<Media> DeletarMedia(Guid idMedia);
        Media ObterMedia(Guid idMedia);

    }
}

using Ftec.ProjetoWeb.Produtos.Dominio.Entidade;
using Ftec.ProjetoWeb.Produtos.Dominio.Enum;
using Ftec.ProjetoWeb.Produtos.Dominio.Interfaces;
using Npgsql;

namespace Ftec.ProjetoWeb.Produtos.Persistencia
{
    public class MediaRepositorio : IMediaRepositorio
    {
        private string stringConexao;
        private string _caminhoUpload;
        public MediaRepositorio(string strConexao, string caminhoUpload)
        {
            stringConexao = strConexao;
            _caminhoUpload = caminhoUpload;
        }

        public Response<Media> InserirMedia(Media media)
        {
            try
            {
                using (var conexao = new NpgsqlConnection(stringConexao))
                {
                    conexao.Open();

                    using (var transacao = conexao.BeginTransaction())
                    {
                        var comando = new NpgsqlCommand();
                        comando.Connection = conexao;
                        comando.Transaction = transacao;

                        comando.CommandText = @"
                        INSERT INTO media
                        (id, nomeArquivo, nomeUnico, caminhoArquivo, tipoArquivo, extensao)
                        VALUES
                        (@id, @nomearquivo, @nomeunico, @caminhoarquivo, @tipoarquivo, @extensao)
                    ";

                        comando.Parameters.AddWithValue("@id", media.Id);
                        comando.Parameters.AddWithValue("@nomearquivo", media.NomeArquivo);
                        comando.Parameters.AddWithValue("@nomeunico", media.NomeUnico);
                        comando.Parameters.AddWithValue("@caminhoarquivo", media.CaminhoArquivo);
                        comando.Parameters.AddWithValue("@tipoarquivo", media.TipoArquivo.ToString());
                        comando.Parameters.AddWithValue("@extensao", media.Extensao);
                        //comando.Parameters.AddWithValue("@dataupload", DateTime.Now);

                        comando.ExecuteNonQuery();

                        transacao.Commit();

                        return new Response<Media>
                        {
                            Sucesso = true,
                            Data = media,
                            Message = "Mídia salva com sucesso"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Response<Media>
                {
                    Sucesso = false,
                    Data = null,
                    Message = $"Erro ao salvar mídia: {ex.Message}"
                };
            }
        }
        public Response<Media> DeletarMedia(Guid idMedia)
        {
            try
            {
                using (var conexao = new NpgsqlConnection(stringConexao))
                {
                    conexao.Open();

                    var comandoSelect = new NpgsqlCommand();
                    comandoSelect.Connection = conexao;

                    comandoSelect.CommandText = @"
                        SELECT ""caminhoArquivo""
                        FROM media
                        WHERE id = @id
                    ";

                    comandoSelect.Parameters.AddWithValue("@id", idMedia);

                    string caminhoArquivo = string.Empty;

                    using (var reader = comandoSelect.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            caminhoArquivo = reader.GetString(0);
                        }
                        else
                        {
                            return new Response<Media>(false, null, "Media inexistente");
                        }
                    }

                    var caminhoFisico = Path.Combine(_caminhoUpload, caminhoArquivo.TrimStart('/'));

                    if (File.Exists(caminhoFisico))
                    {
                        File.Delete(caminhoFisico);
                    }

                    var comandoDelete = new NpgsqlCommand();
                    comandoDelete.Connection = conexao;

                    comandoDelete.CommandText = "DELETE FROM media WHERE id = @id";
                    comandoDelete.Parameters.AddWithValue("@id", idMedia);

                    comandoDelete.ExecuteNonQuery();

                    return new Response<Media>(true, null, "Sucesso ao excluir media!");
                }
            }
            catch (Exception ex)
            {
                return new Response<Media>(false, null, $"ERRO ao excluir media! {ex.Message}"); ;
            }
        }
        public Media ObterMedia(Guid idMedia)
        {
            if (idMedia == Guid.Empty)
                return null;

            using (var conexao = new NpgsqlConnection(stringConexao))
            {
                conexao.Open();

                var comando = new NpgsqlCommand();
                comando.Connection = conexao;

                comando.CommandText = @"
                    SELECT id, nomeArquivo, nomeUnico, ""caminhoArquivo"", tipoArquivo, extensao, dataUpload
                    FROM media
                    WHERE id = @id
                ";

                comando.Parameters.AddWithValue("@id", idMedia);

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Media
                        {
                            Id = reader.GetGuid(0),
                            NomeArquivo = reader.GetString(1),
                            NomeUnico = reader.GetString(2),
                            CaminhoArquivo = reader.GetString(3),
                            TipoArquivo = Enum.Parse<TipoArquivo>(reader.GetString(4)),
                            Extensao = reader.GetString(5),
                            DataUpload = reader.GetDateTime(6)
                        };
                    }
                }
            }

            return null;
        }
    }
}

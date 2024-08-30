using Sistema.Models;
namespace Sistema.Repositories.Interfaces

{
    public interface IProdutoRepository
    {
        Task<List<ProdutoModel>> BuscarTodosProdutos();
        Task<ProdutoModel> BuscarProdutoPorId(int id);
        Task<List<ProdutoModel>> BuscarProdutoPorUsuario(string usuario);
        Task<List<ProdutoModel>> AdicionarProdutosLista(List<ProdutoModel> lista, string idUsuario);
        Task<ProdutoModel> AdicionarProduto(ProdutoModel produto, string idUsuario);
        Task<ProdutoModel> AtualizarProduto(ProdutoModel produtoModel, int id);
        Task<bool> DeletarProduto(int id);
    }
}

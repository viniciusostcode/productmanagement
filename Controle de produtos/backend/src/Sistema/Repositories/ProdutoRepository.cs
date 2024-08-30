using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Sistema.Data;
using Sistema.Models;
using Sistema.Models.Enums;
using Sistema.Repositories.Interfaces;
using System.Runtime.CompilerServices;

namespace Sistema.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly ProdutoSystemDbContext _dbContextProduto;

        private readonly UserManager<ApplicationUser> _userManager;
        public ProdutoRepository(ProdutoSystemDbContext dbContextTransaction, UserManager<ApplicationUser> userManager)
        {
            _dbContextProduto = dbContextTransaction;
            _userManager = userManager;
        }

        public async Task<List<ProdutoModel>> AdicionarProdutosLista(List<ProdutoModel> lista, string idUsuario)
        {
            List<ProdutoModel> ProdutosLista = new List<ProdutoModel>();

            if (Equals(lista.Count, 0)) throw new Exception("Dados vazios");

            foreach (ProdutoModel produtoModel in ProdutosLista)
            {
                ProdutoModel produto = new ProdutoModel();

                produto.Preco = produtoModel.Preco;
                produto.Quantidade = produtoModel.Quantidade;
                produto.Data = DateTime.Now;
                produto.Produto = produtoModel.Produto;

                ApplicationUser user = await _userManager.FindByIdAsync(idUsuario);

                produto.Usuario = user;

                if (!Enum.TryParse(produtoModel.Situacao, out SituacaoEnum situacao)) throw new Exception("A situação é inválida.");

                produto.Situacao = situacao.ToString();

                ProdutosLista.Add(produto);
            }

            await _dbContextProduto.Produtos.AddRangeAsync(ProdutosLista);
            await _dbContextProduto.SaveChangesAsync();

            return ProdutosLista;
        }
        public async Task<List<ProdutoModel>> BuscarTodosProdutos()
        {
            List<ProdutoModel>? resultado = await _dbContextProduto.Produtos.Include(x=> x.Usuario).ToListAsync();
            return resultado;
        }

        public async Task<ProdutoModel> BuscarProdutoPorId(int id)
        {

            ProdutoModel? resultado = await _dbContextProduto.Produtos.Include(x => x.Usuario).FirstOrDefaultAsync(x => x.Id == id);

            return resultado;
        }
        public async Task<ProdutoModel> AdicionarProduto(ProdutoModel produtoModel, string userName)
        {
            try
            {
                ProdutoModel produto = new ProdutoModel();

                if (Equals(produtoModel, null)) throw new Exception("Dados vazios");

                produto.Preco = produtoModel.Preco;
                produto.Quantidade = produtoModel.Quantidade;
                produto.Data = DateTime.Now;
                produto.Produto = produtoModel.Produto;

                ApplicationUser user = await _userManager.FindByNameAsync(userName);

                produto.IdUsuario = user.Id;

                produto.Usuario = user;

                if (!Enum.TryParse(produtoModel.Situacao, out SituacaoEnum situacao)) throw new Exception("A situação é inválida.");

                produto.Situacao = situacao.ToString();

                await _dbContextProduto.Produtos.AddAsync(produto);

                await _dbContextProduto.SaveChangesAsync();

                return produtoModel;

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao salvar a entidades " + ex.Message);
            }
        }

        public async Task<bool> DeletarProduto(int id)
        {
            ProdutoModel produto = await BuscarProdutoPorId(id);

            if (produto.Equals(null)) throw new Exception($"produto não encontrado para o id: {id}");

            _dbContextProduto.Produtos.Remove(produto);

            await _dbContextProduto.SaveChangesAsync();

            return true;
        }


        public async Task<ProdutoModel> AtualizarProduto(ProdutoModel produtoModel, int id)
        {
            try
            {
                ProdutoModel produto = await BuscarProdutoPorId(id);

                if (produto.Equals(null)) throw new Exception($"Produto não encontrado pelo id: {id}");

                produto.Preco = produtoModel.Preco;
                produto.Quantidade = produtoModel.Quantidade;
                produto.Data = produtoModel.Data;
                produto.Produto = produtoModel.Produto;
                produto.Situacao = produtoModel.Situacao;

                await _dbContextProduto.SaveChangesAsync();

                return produtoModel;

            }
            catch (Exception ex)
            {
                throw new Exception("Ocorreu um erro ao atualizar a entidade: " + ex.Message);
            }
        }

        public async Task<List<ProdutoModel>> BuscarProdutoPorUsuario(string usuario)
        {
            List<ProdutoModel>? resultado = await _dbContextProduto.Produtos.Include(x => x.Usuario).Where(x => x.Usuario.UserName == usuario).ToListAsync();

            return resultado;
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Sistema.Models;
using Sistema.Repositories.Interfaces;

namespace Sistema.Controllers
{
    [Route("produtos")]
    [ApiController]
    public class ProdutoController : ControllerBase
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoController(IProdutoRepository produtoRepository)
        {
            _produtoRepository = produtoRepository;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProdutoModel>>> BuscarTodosProdutos()
        {
            try
            {
                List<ProdutoModel> produtos = await _produtoRepository.BuscarTodosProdutos();

                if (produtos != null) return Ok(produtos);

                return NotFound("Result not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProdutoModel>> BuscarProdutoPorId(int id)
        {
            try
            {
                ProdutoModel produto = await _produtoRepository.BuscarProdutoPorId(id);

                if (produto != null) return Ok(produto);

                return NotFound("Result not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        
        [HttpGet("buscar/{usuario}")]
        public async Task<ActionResult<List<ProdutoModel>>> BuscarProdutoPorUsuario(string usuario)
        {
            try
            {
                List<ProdutoModel> produto = await _produtoRepository.BuscarProdutoPorUsuario(usuario);

                if (produto != null) return Ok(produto);

                return NotFound("Result not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }

        [HttpPost("adicionar/{usuario}")]
        public async Task<ActionResult<ProdutoModel>> AdicionarProduto([FromBody] ProdutoModel produtoModel, string usuario)
        {
            ProdutoModel produto = await _produtoRepository.AdicionarProduto(produtoModel, usuario);

            return Ok(produto);
        }

        [HttpPost("lista")]
        public async Task<ActionResult<ProdutoModel>> AdicionarProdutosLista([FromBody] List<ProdutoModel> produtoModel, string idUsuario)
        {
            List<ProdutoModel> produto = await _produtoRepository.AdicionarProdutosLista(produtoModel,idUsuario);

            return Ok(produto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ProdutoModel>> AtualizarProduto(int id, [FromBody] ProdutoModel produtoModel)
        {
            ProdutoModel produto = await _produtoRepository.AtualizarProduto(produtoModel, id);

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ProdutoModel>> DeletarProduto(int id)
        {
            bool deleted = await _produtoRepository.DeletarProduto(id);

            return Ok(deleted);
        }

    }
}

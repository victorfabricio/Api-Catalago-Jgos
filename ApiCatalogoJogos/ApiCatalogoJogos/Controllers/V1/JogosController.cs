using ApiCatalogoJogos.Exceptions;
using ApiCatalogoJogos.InputModel;
using ApiCatalogoJogos.Services;
using ApiCatalogoJogos.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoJogos.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class JogosController : ControllerBase
    {
        private readonly IJogoService _jogoService;

        public JogosController(IJogoService jogoService)
        {
            _jogoService = jogoService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagina"></param>
        /// <param name="quantidade"></param>
        /// <returns></returns>

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JogoViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var jogos = await _jogoService.Obter(pagina, quantidade);

            if (jogos.Count() == 0)
                return NoContent();

            return Ok(jogos);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idJogo"></param>
        /// <returns></returns>
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<JogoViewModel>> Obter([FromRoute] Guid idJogo)
        {
            var jogo = await _jogoService.Obter(idJogo);

            if (jogo == null)
                return NoContent();

            return Ok();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jogoInputModel"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<JogoViewModel>> InserirJogo([FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                var jogo = await _jogoService.Inserir(jogoInputModel);
                return Ok(jogo);
            }
            catch (JogoJaCadastradoException ex)            
            {
                return UnprocessableEntity("Já existe um jogo com este nome para esta produtora");
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idJogo"></param>
        /// <param name="jogoInputModel"></param>
        /// <returns></returns>
        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarJogo(Guid idJogo,[FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, jogoInputModel);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)            
            {
                return NotFound("Não existe este jogo");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idJogo"></param>
        /// <param name="preco"></param>
        /// <returns></returns>
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> Atualizarjogo([FromRoute] Guid idJogo,[FromRoute] double preco)
        {
            try
            {
                await _jogoService.Atualizar(idJogo, preco);
                return Ok();
            }
            catch (JogoNaoCadastradoException ex)            
            {
                return NotFound("Não existe este jogo");
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idJogo"></param>
        /// <returns></returns>
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarJogo([FromRoute] Guid idJogo)
        {
            try
            {
                await _jogoService.Remover(idJogo);

                return Ok();
            }
            catch (JogoNaoCadastradoException ex)            
            {
                return NotFound("Não existe este jogo");
            }            
        }
    }
}

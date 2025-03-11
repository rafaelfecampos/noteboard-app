using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RecadosInfra.DAOs;
using RecadoModel;

namespace Recados.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecadoesController : ControllerBase
    {
        private readonly RecadosDAO _dao;

        public RecadoesController()
        {
            _dao = new RecadosDAO();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecModel>>> GetRecados()
        {
            var recados = await _dao.ListarTodosAsync();
            return Ok(recados);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecModel>> GetRecado(int id)
        {
            var recado = await _dao.BuscarPorIdAsync(id);
            if (recado == null)
            {
                return NotFound();
            }
            return Ok(recado);
        }

        [HttpGet("mensagem/{id}")]
        public async Task<ActionResult<string>> GetMensagem(int id)
        {
            var recado = await _dao.BuscarPorIdAsync(id);
            if (recado == null)
            {
                return NotFound();
            }
            return Ok(recado.Mensagem);
        }

        [HttpGet("remetente={remetente}")]
        public async Task<ActionResult<List<RecModel>>> GetRecadoByRemetente(string remetente)
        {
            var recados = (await _dao.ListarTodosAsync()).Where(r => r.Remetente.ToLower().Contains(remetente.ToLower())).ToList();
            if (!recados.Any())
            {
                return NotFound();
            }
            return Ok(recados);
        }

        [HttpGet("destinatario={destinatario}")]
        public async Task<ActionResult<List<RecModel>>> GetMensagemByDestinatario(string destinatario)
        {
            var recados = (await _dao.ListarTodosAsync())
                   .Where(r => r.Destinatario.ToLower().Contains(destinatario.ToLower()))
                   .ToList();
            if (!recados.Any())
            {
                return NotFound();
            }
            return Ok(recados);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRecados(int id, RecModel recado)
        {
            if (id != recado.Id)
            {
                return BadRequest();
            }

            try
            {
                await _dao.AlterarAsync(recado);
            }
            catch (Exception)
            {
                if (await _dao.BuscarPorIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<RecModel>> PostRecados(RecModel recado)
        {
            await _dao.InserirAsync(recado);
            return CreatedAtAction(nameof(GetRecado), new { id = recado.Id }, recado);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecados(int id)
        {
            var recado = await _dao.BuscarPorIdAsync(id);
            if (recado == null)
            {
                return NotFound();
            }

            await _dao.DeletarAsync(id);
            return NoContent();
        }
    }
}

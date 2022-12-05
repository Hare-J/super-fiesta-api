using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FinalProject_API.Data;
using FinalProject_API.Models;
using FinalProject_API.Models.DTOs;

namespace FinalProject_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrimaryTypesController : ControllerBase
    {
        private readonly PokemonApiContext _context;

        public PrimaryTypesController(PokemonApiContext context)
        {
            _context = context;
        }

        // GET: api/PrimaryTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrimaryTypeDTO>>> GetPrimaryTypes()
        {
            return await _context.PrimaryTypes
                .Select(a => new PrimaryTypeDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    RowVersion = a.RowVersion
                }).ToListAsync();
        }

        // GET: api/PrimaryTypes
        [HttpGet("inc")]//api/trainers/inc
        public async Task<ActionResult<IEnumerable<PrimaryTypeDTO>>> GetPrimaryTypesInc()
        {
            return await _context.PrimaryTypes
                .Include(a => a.Pokemons)
                .Select(a => new PrimaryTypeDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    RowVersion = a.RowVersion,
                    Pokemons = a.Pokemons.Select(aPokemon => new PokemonDTO
                    {
                        ID = aPokemon.ID,
                        Pokedex= aPokemon.Pokedex,
                        Name = aPokemon.Name,
                        HP = aPokemon.Attack,
                        Attack = aPokemon.Attack
                    }).ToList()
                }).ToListAsync();
        }

        // GET: api/PrimaryTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PrimaryTypeDTO>> GetPrimaryType(int id)
        {
            var trainerDTO = await _context.PrimaryTypes
                .Select(a => new PrimaryTypeDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    RowVersion = a.RowVersion
                }).FirstOrDefaultAsync(a => a.ID == id);

            if (trainerDTO == null)
            {
                return NotFound(new { message = "Error: PrimaryType record not found." });
            }

            return trainerDTO;
        }

        // GET: api/PrimaryTypes/5
        [HttpGet("inc/{id}")]//api/arttypes/inc/5
        public async Task<ActionResult<PrimaryTypeDTO>> GetPrimaryTypeInc(int id)
        {
            var trainerDTO = await _context.PrimaryTypes
                .Include(a => a.Pokemons)
                .Select(a => new PrimaryTypeDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    RowVersion = a.RowVersion,
                    Pokemons = a.Pokemons.Select(aPokemon => new PokemonDTO
                    {
                        ID = aPokemon.ID,
                        Pokedex = aPokemon.Pokedex,
                        Name = aPokemon.Name,
                        HP = aPokemon.HP,
                        Attack = aPokemon.Attack
                    }).ToList()
                }).FirstOrDefaultAsync(a => a.ID == id);

            if (trainerDTO == null)
            {
                return NotFound(new { message = "Error: PrimaryType record not found." });
            }

            return trainerDTO;
        }

        // PUT: api/PrimaryTypes/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPrimaryType(int id, PrimaryType primaryType)
        {
            if (id != primaryType.ID)
            {
                return BadRequest();
            }

            _context.Entry(primaryType).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PrimaryTypeExists(id))
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

        // POST: api/PrimaryTypes
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PrimaryType>> PostPrimaryType(PrimaryType primaryType)
        {
            _context.PrimaryTypes.Add(primaryType);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPrimaryType", new { id = primaryType.ID }, primaryType);
        }

        // DELETE: api/PrimaryTypes/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PrimaryType>> DeletePrimaryType(int id)
        {
            var primaryType = await _context.PrimaryTypes.FindAsync(id);
            if (primaryType == null)
            {
                return NotFound();
            }

            _context.PrimaryTypes.Remove(primaryType);
            await _context.SaveChangesAsync();

            return primaryType;
        }

        private bool PrimaryTypeExists(int id)
        {
            return _context.PrimaryTypes.Any(e => e.ID == id);
        }
    }
}

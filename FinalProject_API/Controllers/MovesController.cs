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
    public class MovesController : ControllerBase
    {
        private readonly PokemonApiContext _context;

        public MovesController(PokemonApiContext context)
        {
            _context = context;
        }

        // GET: api/Moves
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MoveDTO>>> GetMoves()
        {
            return await _context.Moves
                .Include(m => m.Pokemon)
                .Include(m => m.PrimaryType)
                .Select(a => new MoveDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    TypeID = a.TypeID,
                    RowVersion = a.RowVersion,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    Pokemon = new PokemonDTO
                    {
                        ID = a.Pokemon.ID,
                        Name = a.Pokemon.Name,
                        Pokedex = a.Pokemon.Pokedex,
                        HP = a.Pokemon.HP,
                        Attack = a.Pokemon.Attack
                    }
                })
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MoveDTO>> GetMove(int id)
        {
            var pokemonDTO = await _context.Moves
                .Include(m => m.Pokemon)
                .Include(m => m.PrimaryType)
                .Select(a => new MoveDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    TypeID = a.TypeID,
                    RowVersion = a.RowVersion,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    Pokemon = new PokemonDTO
                    {
                        ID = a.Pokemon.ID,
                        Name = a.Pokemon.Name,
                        Pokedex = a.Pokemon.Pokedex,
                        HP = a.Pokemon.HP,
                        Attack = a.Pokemon.Attack
                    }
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (pokemonDTO == null)
            {
                return NotFound(new { message = "Error: Move record not found." });
            }

            return pokemonDTO;
        }

        // Get a random move from the database
        [HttpGet("GetRandom")]
        public async Task<ActionResult<MoveDTO>> GetRandomMove()
        {
            Random random = new Random();
            int[] moveIDs = _context.Moves.Select(a => a.ID).ToArray();
            int moveIDCount = moveIDs.Count();
            int id = moveIDs[random.Next(moveIDCount)];

            var moveDTO = await _context.Moves
                .Include(m => m.Pokemon)
                .Include(m => m.PrimaryType)
                .Select(a => new MoveDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    TypeID = a.TypeID,
                    RowVersion = a.RowVersion,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    Pokemon = new PokemonDTO
                    {
                        ID = a.Pokemon.ID,
                        Name = a.Pokemon.Name,
                        Pokedex = a.Pokemon.Pokedex,
                        HP = a.Pokemon.HP,
                        Attack = a.Pokemon.Attack
                    }
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (moveDTO == null)
            {
                return NotFound(new { message = "Error: Move record not found." });
            }

            return moveDTO;
        }

        // GET: api/MovesByTrainer
        [HttpGet("ByPokemon/{id}")]
        public async Task<ActionResult<IEnumerable<MoveDTO>>> GetMovesByPokemon(int id)
        {
            var moves = await _context.Moves
                .Include(m => m.Pokemon)
                .Include(m => m.PrimaryType)
                .Select(a => new MoveDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    TypeID = a.TypeID,
                    RowVersion = a.RowVersion,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    Pokemon = new PokemonDTO
                    {
                        ID = a.Pokemon.ID,
                        Name = a.Pokemon.Name,
                        Pokedex = a.Pokemon.Pokedex,
                        HP = a.Pokemon.HP,
                        Attack = a.Pokemon.Attack
                    }
                })
                .ToListAsync();

            if (moves.Count() == 0)
            {
                return NotFound(new { message = "Error: No Moves in that Pokemon." });
            }

            return moves;
        }

        // PUT: api/Moves/5 - Update
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMove(int id, MoveDTO moveDTO)
        {
            if (id != moveDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Move." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var moveToUpdate = await _context.Moves.FindAsync(id);
            //Check that you got it
            if (moveToUpdate == null)
            {
                return NotFound(new { message = "Error: Move record not found." });
            }

            //Wow, we have a chance to check for concurrency even before bothering
            //the database!  Of course, it will get checked again in the database just in case
            //it changes after we pulled the record.  
            //Note using SequenceEqual becuase it is an array after all.
            if (moveDTO.RowVersion != null)
            {
                if (!moveToUpdate.RowVersion.SequenceEqual(moveDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Move has been changed by another user.  Try editing the record again." });
                }
            }

            moveToUpdate.ID = moveDTO.ID;
            moveToUpdate.Name = moveDTO.Name;
            moveToUpdate.TypeID = moveDTO.TypeID;
            moveToUpdate.PokemonID = moveDTO.PokemonID;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(moveToUpdate).Property("RowVersion").OriginalValue = moveDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MoveExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Move has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Move has been updated by another user.  Back out and try editing the record again." });
                }
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Pokedex Number." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // POST: api/Moves - INSERT
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<MoveDTO>> PostMove(MoveDTO moveDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Move move = new Move
            {
                ID = moveDTO.ID,
                Name = moveDTO.Name,
                TypeID = moveDTO.TypeID,
                PokemonID = moveDTO.PokemonID,
            };

            try
            {
                _context.Moves.Add(move);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                moveDTO.ID = move.ID;
                moveDTO.RowVersion = move.RowVersion;

                return CreatedAtAction(nameof(GetMove), new { id = move.ID }, moveDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Move Email." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Moves/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMove(int id)
        {
            var move = await _context.Moves.FindAsync(id);
            if (move == null)
            {
                return BadRequest(new { message = "Delete Error: Move has already been removed." });
            }
            try
            {
                _context.Moves.Remove(move);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Move." });
            }
        }

        private bool MoveExists(int id)
        {
            return _context.Moves.Any(e => e.ID == id);
        }
    }
}

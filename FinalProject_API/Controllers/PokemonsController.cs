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
    public class PokemonsController : ControllerBase
    {
        private readonly PokemonApiContext _context;

        public PokemonsController(PokemonApiContext context)
        {
            _context = context;
        }

        // GET: api/Pokemons
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetPokemons()
        {
            return await _context.Pokemons
                .Include(a => a.Trainer)
                .Include(a => a.PrimaryType)
                //.Include(a => a.Moves)
                .Select(a => new PokemonDTO
                {
                    ID = a.ID,
                    Pokedex = a.Pokedex,
                    Name = a.Name,
                    HP = a.HP,
                    Attack = a.Attack,
                    RowVersion = a.RowVersion,
                    PrimaryTypeID = a.PrimaryTypeID,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    TrainerID = a.TrainerID,
                    Trainer = new TrainerDTO
                    {
                        ID = a.Trainer.ID,
                        Name = a.Trainer.Name,
                        Badges = a.Trainer.Badges
                    }

                })
                .ToListAsync();
        }

        // GET: api/Pokemons/5
        // Search by Pokedex Number
        [HttpGet("{dex}")]
        public async Task<ActionResult<PokemonDTO>> GetPokemon(int dex)
        {
            var pokemonDTO = await _context.Pokemons
                .Include(a => a.Trainer)
                .Include(a => a.PrimaryType)
                .Select(a => new PokemonDTO
                {
                    ID = a.ID,
                    Pokedex = a.Pokedex,
                    Name = a.Name,
                    HP = a.HP,
                    Attack = a.Attack,
                    RowVersion = a.RowVersion,
                    PrimaryTypeID = a.PrimaryTypeID,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    TrainerID = a.TrainerID,
                    Trainer = new TrainerDTO
                    {
                        ID = a.Trainer.ID,
                        Name = a.Trainer.Name,
                        Badges = a.Trainer.Badges
                    }
                })
                .FirstOrDefaultAsync(p => p.Pokedex == dex);

            if (pokemonDTO == null)
            {
                return NotFound(new { message = "Error: Pokemon record not found." });
            }

            return pokemonDTO;
        }

        // GET: api/Pokemons/5
        // Get a random pokemon from the database
        [HttpGet("GetRandom")]
        public async Task<ActionResult<PokemonDTO>> GetRandomPokemon()
        {
            Random random = new Random();
            int[] pokemonIDs = _context.Pokemons.Select(a => a.ID).ToArray();
            int pokemonIDCount = pokemonIDs.Count();
            int id = pokemonIDs[random.Next(pokemonIDCount)];

            var pokemonDTO = await _context.Pokemons
                .Include(a => a.Trainer)
                .Include(a => a.PrimaryType)
                .Select(a => new PokemonDTO
                {
                    ID = a.ID,
                    Pokedex = a.Pokedex,
                    Name = a.Name,
                    HP = a.HP,
                    Attack = a.Attack,
                    RowVersion = a.RowVersion,
                    PrimaryTypeID = a.PrimaryTypeID,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    TrainerID = a.TrainerID,
                    Trainer = new TrainerDTO
                    {
                        ID = a.Trainer.ID,
                        Name = a.Trainer.Name,
                        Badges = a.Trainer.Badges
                    }
                })
                .FirstOrDefaultAsync(p => p.ID == id);

            if (pokemonDTO == null)
            {
                return NotFound(new { message = "Error: Pokemon record not found." });
            }

            return pokemonDTO;
        }

        // GET: api/PokemonsByTrainer
        [HttpGet("ByTrainer/{id}")]
        public async Task<ActionResult<IEnumerable<PokemonDTO>>> GetPokemonsByTrainer(int id)
        {
            var pokemons = await _context.Pokemons
                .Include(a => a.Trainer)
                .Include(a => a.PrimaryType)
                .Where(a => a.TrainerID == id)
                .Select(a => new PokemonDTO
                {
                    ID = a.ID,
                    Pokedex = a.Pokedex,
                    Name = a.Name,
                    HP = a.HP,
                    Attack = a.Attack,
                    RowVersion = a.RowVersion,
                    PrimaryTypeID = a.PrimaryTypeID,
                    PrimaryType = new PrimaryTypeDTO
                    {
                        ID = a.PrimaryType.ID,
                        Name = a.PrimaryType.Name
                    },
                    TrainerID = a.TrainerID,
                    Trainer = new TrainerDTO
                    {
                        ID = a.Trainer.ID,
                        Name = a.Trainer.Name,
                        Badges = a.Trainer.Badges
                    }
                })
                .ToListAsync();

            if (pokemons.Count() == 0)
            {
                return NotFound(new { message = "Error: No Pokemons in that Trainer." });
            }

            return pokemons;
        }

        // PUT: api/Pokemons/5 - Update
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPokemon(int id, PokemonDTO pokemonDTO)
        {
            if (id != pokemonDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Pokemon." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var pokemonToUpdate = await _context.Pokemons.FindAsync(id);
            //Check that you got it
            if (pokemonToUpdate == null)
            {
                return NotFound(new { message = "Error: Pokemon record not found." });
            }

            //Wow, we have a chance to check for concurrency even before bothering
            //the database!  Of course, it will get checked again in the database just in case
            //it changes after we pulled the record.  
            //Note using SequenceEqual becuase it is an array after all.
            if (pokemonDTO.RowVersion != null)
            {
                if (!pokemonToUpdate.RowVersion.SequenceEqual(pokemonDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Pokemon has been changed by another user.  Try editing the record again." });
                }
            }

            pokemonToUpdate.ID = pokemonDTO.ID;
            pokemonToUpdate.Pokedex = pokemonDTO.Pokedex;
            pokemonToUpdate.Name = pokemonDTO.Name;
            pokemonToUpdate.HP = pokemonDTO.HP;
            pokemonToUpdate.Attack = pokemonDTO.Attack;
            pokemonToUpdate.RowVersion = pokemonDTO.RowVersion;
            pokemonToUpdate.PrimaryTypeID = pokemonDTO.PrimaryTypeID;
            pokemonToUpdate.TrainerID = pokemonDTO.TrainerID;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(pokemonToUpdate).Property("RowVersion").OriginalValue = pokemonDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PokemonExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Pokemon has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Pokemon has been updated by another user.  Back out and try editing the record again." });
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

        // POST: api/Pokemons - INSERT
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<PokemonDTO>> PostPokemon(PokemonDTO pokemonDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Pokemon pokemon = new Pokemon
            {
                ID = pokemonDTO.ID,
                Pokedex = pokemonDTO.Pokedex,
                Name = pokemonDTO.Name,
                HP = pokemonDTO.HP,
                Attack = pokemonDTO.Attack,
                RowVersion = pokemonDTO.RowVersion,
                PrimaryTypeID = pokemonDTO.PrimaryTypeID,
                TrainerID = pokemonDTO.TrainerID
            };

            try
            {
                _context.Pokemons.Add(pokemon);
                await _context.SaveChangesAsync();

                //Assign Database Generated values back into the DTO
                pokemonDTO.ID = pokemon.ID;
                pokemonDTO.RowVersion = pokemon.RowVersion;

                return CreatedAtAction(nameof(GetPokemon), new { id = pokemon.ID }, pokemonDTO);
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("UNIQUE"))
                {
                    return BadRequest(new { message = "Unable to save: Duplicate Pokedex Number.." });
                }
                else
                {
                    return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        // DELETE: api/Pokemons/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePokemon(int id)
        {
            var pokemon = await _context.Pokemons.FindAsync(id);
            if (pokemon == null)
            {
                return BadRequest(new { message = "Delete Error: Pokemon has already been removed." });
            }
            try
            {
                _context.Pokemons.Remove(pokemon);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Delete Error: Unable to delete Pokemon." });
            }
        }

        private bool PokemonExists(int id)
        {
            return _context.Pokemons.Any(e => e.ID == id);
        }
    }
}

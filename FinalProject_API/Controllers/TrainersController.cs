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
    public class TrainersController : ControllerBase
    {
        private readonly PokemonApiContext _context;

        public TrainersController(PokemonApiContext context)
        {
            _context = context;
        }

        // GET: api/Trainers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TrainerDTO>>> GetTrainers()
        {
            return await _context.Trainers
                .Select(a => new TrainerDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Badges = a.Badges,
                    RowVersion = a.RowVersion
                }).ToListAsync();
        }

        // GET: api/Trainers
        [HttpGet("inc")]//api/trainers/inc
        public async Task<ActionResult<IEnumerable<TrainerDTO>>> GetTrainersInc()
        {
            return await _context.Trainers
                .Include(a => a.Pokemons)
                .Select(a => new TrainerDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Badges = a.Badges,
                    RowVersion = a.RowVersion,
                    Pokemons = a.Pokemons.Select(aPokemon => new PokemonDTO
                    {
                        ID = aPokemon.ID,
                        Pokedex = aPokemon.Pokedex,
                        Name = aPokemon.Name,
                        HP = aPokemon.Attack,
                        Attack = aPokemon.Attack
                    }).ToList()
                }).ToListAsync();
        }

        // GET: api/Trainers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TrainerDTO>> GetTrainer(int id)
        {
            var trainerDTO = await _context.Trainers
                .Select(a => new TrainerDTO
                {
                    ID = a.ID,
                    Name = a.Name,
                    Badges = a.Badges,
                    RowVersion = a.RowVersion
                }).FirstOrDefaultAsync(a => a.ID == id);

            if (trainerDTO == null)
            {
                return NotFound(new { message = "Error: Trainer record not found." });
            }

            return trainerDTO;
        }

        // GET: api/Trainers/5
        [HttpGet("inc/{id}")]//api/arttypes/inc/5
        public async Task<ActionResult<TrainerDTO>> GetTrainerInc(int id)
        {
            var trainerDTO = await _context.Trainers
                .Include(a => a.Pokemons)
                .Select(a => new TrainerDTO
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
                return NotFound(new { message = "Error: Trainer record not found." });
            }

            return trainerDTO;
        }

        // PUT: api/Trainers/5 - UPDATE
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrainer(int id, TrainerDTO trainerDTO)
        {
            if (id != trainerDTO.ID)
            {
                return BadRequest(new { message = "Error: ID does not match Trainer." });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Get the record you want to update
            var trainerToUpdate = await _context.Trainers.FindAsync(id);

            //Check that you got it
            if (trainerToUpdate == null)
            {
                return NotFound(new { message = "Error: Trainer record not found." });
            }

            //Wow, we have a chance to check for concurrency even before bothering
            //the database!  Of course, it will get checked again in the database just in case
            //it changes after we pulled the record.  
            //Note using SequenceEqual becuase it is an array after all.
            if (trainerDTO.RowVersion != null)
            {
                if (!trainerToUpdate.RowVersion.SequenceEqual(trainerDTO.RowVersion))
                {
                    return Conflict(new { message = "Concurrency Error: Trainer has been changed by another user.  Back out and try editing the record again." });
                }
            }


            trainerToUpdate.ID = trainerDTO.ID;
            trainerToUpdate.Name = trainerDTO.Name;
            trainerToUpdate.Badges = trainerDTO.Badges;
            trainerToUpdate.RowVersion = trainerDTO.RowVersion;

            //Put the original RowVersion value in the OriginalValues collection for the entity
            _context.Entry(trainerToUpdate).Property("RowVersion").OriginalValue = trainerDTO.RowVersion;

            try
            {
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainerExists(id))
                {
                    return Conflict(new { message = "Concurrency Error: Trainer has been Removed." });
                }
                else
                {
                    return Conflict(new { message = "Concurrency Error: Trainer has been updated by another user.  Back out and try editing the record again." });
                }
            }
        }

        // POST: api/Trainers - INSERT
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TrainerDTO>> PostTrainer(TrainerDTO trainerDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Trainer trainer = new Trainer
            {
                ID = trainerDTO.ID,
                Name = trainerDTO.Name
            };

            try
            {
                _context.Trainers.Add(trainer);
                await _context.SaveChangesAsync();
                //Assign Database Generated values back into the DTO
                trainerDTO.ID = trainer.ID;
                trainerDTO.RowVersion = trainer.RowVersion;
                return CreatedAtAction(nameof(GetTrainer), new { id = trainer.ID }, trainerDTO);
            }
            catch (DbUpdateException)
            {
                return BadRequest(new { message = "Unable to save changes to the database. Try again, and if the problem persists see your system administrator." });
            }
        }

        // DELETE: api/Trainers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTrainer(int id)
        {
            var trainer = await _context.Trainers.FindAsync(id);
            if (trainer == null)
            {
                return NotFound(new { message = "Delete Error: Trainer has already been removed." });
            }
            try
            {
                _context.Trainers.Remove(trainer);
                await _context.SaveChangesAsync();
                return NoContent();
            }
            catch (DbUpdateException dex)
            {
                if (dex.GetBaseException().Message.Contains("FOREIGN KEY constraint failed"))
                {
                    return BadRequest(new { message = "Delete Error: Remember, you cannot delete a type of art that has pokemons." });
                }
                else
                {
                    return BadRequest(new { message = "Delete Error: Unable to delete Trainer. Try again, and if the problem persists see your system administrator." });
                }
            }
        }

        private bool TrainerExists(int id)
        {
            return _context.Trainers.Any(e => e.ID == id);
        }
    }
}

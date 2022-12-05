using FinalProject_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Data
{
    public static class PokemonSeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {

            using (var context = new PokemonApiContext(
                serviceProvider.GetRequiredService<DbContextOptions<PokemonApiContext>>()))
            {
                //Create sample data with some random values
                Random random = new Random();
                
                Random randomB = new Random();
                //int randBRange = randB.Next(0, 8);

                string[] trainers = new string[] { "Red", "Dawn", "Brock", "Agatha", "Gary" };
                string[] types = new string[] { "Normal", "Fire", "Water", "Grass", "Rock", "Ground", "Fighting", "Electric", "Dragon", "Ice", "Ghost", "Dark", "Poison", "Steel", "Flying", "Fairy" };

                //Trainers
                if (!context.Trainers.Any())
                {
                    //loop through the array of Trainer names
                    foreach (string t in trainers)
                    {
                        Trainer trainer = new Trainer()
                        {
                            Name = t,
                            Badges = randomB.Next(0, 8)
                        };
                        context.Trainers.Add(trainer);
                    }
                    context.SaveChanges();
                }

                /*if (!context.PokemonTypes.Any())
                {
                    //loop through the array of types
                    foreach (string t in types)
                    {
                        PokemonType type = new PokemonType()
                        {
                            Name = t
                        };
                        context.PokemonTypes.Add(type);
                    }
                    context.SaveChanges();
                }*/
                
                if (!context.PrimaryTypes.Any())
                {
                    //loop through the array of types
                    foreach (string t in types)
                    {
                        PrimaryType type = new PrimaryType()
                        {
                            Name = t
                        };
                        context.PrimaryTypes.Add(type);
                    }
                    context.SaveChanges();
                }

                /*if (!context.Moves.Any())
                {
                    List<Move> moves = new List<Move>();

                    moves.Add(new Move { Name = "Tackle", TypeID = 7, PokemonID = 1 });
                    moves.Add(new Move { Name = "Shadow Ball", TypeID = 1, PokemonID = 2 });
                    moves.Add(new Move { Name = "Thrash", TypeID = 7, PokemonID = 3 });
                    moves.Add(new Move { Name = "Hydro Pump", TypeID = 4, PokemonID = 4 });
                    moves.Add(new Move { Name = "Thief", TypeID = 13, PokemonID = 5 });
                    moves.Add(new Move { Name = "Leech Seed", TypeID = 9, PokemonID = 6 });
                    moves.Add(new Move { Name = "Gust", TypeID = 12, PokemonID = 7 });
                    moves.Add(new Move { Name = "Dive", TypeID = 4, PokemonID = 1 });
                    moves.Add(new Move { Name = "Psybeam", TypeID = 1, PokemonID = 2 });
                    moves.Add(new Move { Name = "Dream Eater", TypeID = 10, PokemonID = 3 });
                    moves.Add(new Move { Name = "Will O' Wisp", TypeID = 10, PokemonID = 4 });
                    moves.Add(new Move { Name = "Ember", TypeID = 2, PokemonID = 5 });
                    moves.Add(new Move { Name = "Sleep Powder", TypeID = 9, PokemonID = 6 });
                    moves.Add(new Move { Name = "Rest", TypeID = 7, PokemonID = 7 });
                    moves.Add(new Move { Name = "Supersonic", TypeID = 5, PokemonID = 1 });
                    moves.Add(new Move { Name = "Snowstorm", TypeID = 8, PokemonID = 2 });
                    moves.Add(new Move { Name = "Confuse Ray", TypeID = 10, PokemonID = 3 });

                    context.Moves.AddRange(moves);
                    context.SaveChanges();
                }*/

                //Create collections of the primary keys
                int[] trainerIDs = context.Trainers.Select(a => a.ID).ToArray();
                int trainerIDCount = trainerIDs.Count();

                //Pokemons
                if (!context.Pokemons.Any())
                {
                    List<Pokemon> pokemon = new List<Pokemon>();

                    // Add pokemon to list and assign to random trainer
                    pokemon.Add(new Pokemon { Pokedex = 6, Name = "Charizard", HP = 78, Attack = 84, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 4 });
                    pokemon.Add(new Pokemon { Pokedex = 3, Name = "Venasaur", HP = 80, Attack = 82, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 13 });
                    pokemon.Add(new Pokemon { Pokedex = 95, Name = "Onix", HP = 35, Attack = 45, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 1 });
                    pokemon.Add(new Pokemon { Pokedex = 94, Name = "Gengar", HP = 60, Attack = 65, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 11 });
                    pokemon.Add(new Pokemon { Pokedex = 130, Name = "Gyarados", HP = 95, Attack = 125, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 3 });
                    pokemon.Add(new Pokemon { Pokedex = 25, Name = "Pikachu", HP = 35, Attack = 55, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 8 });
                    pokemon.Add(new Pokemon { Pokedex = 128, Name = "Tauros", HP = 75, Attack = 100, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 5 });
                    pokemon.Add(new Pokemon { Pokedex = 105, Name = "Marowak", HP = 60, Attack = 80, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 6 });
                    pokemon.Add(new Pokemon { Pokedex = 82, Name = "Magneton", HP = 50, Attack = 60, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 8 });
                    pokemon.Add(new Pokemon { Pokedex = 141, Name = "Kabutops", HP = 60, Attack = 115, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 3 });
                    pokemon.Add(new Pokemon { Pokedex = 91, Name = "Cloyster", HP = 50, Attack = 95, TrainerID = trainerIDs[random.Next(trainerIDCount)], PrimaryTypeID = 3 });


                    //Now add your list into the DbSet
                    context.Pokemons.AddRange(pokemon);
                    context.SaveChanges();
                }
            }
        }
    }
}

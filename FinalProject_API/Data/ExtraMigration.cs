using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject_API.Data
{
    public static class ExtraMigration
    {
        public static void Steps(MigrationBuilder migrationBuilder)
        {
            //Pokemon Table Triggers for Concurrency
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPokemonTimestampOnUpdate
                    AFTER UPDATE ON Pokemons
                    BEGIN
                        UPDATE Pokemons
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetPokemonTimestampOnInsert
                    AFTER INSERT ON Pokemons
                    BEGIN
                        UPDATE Pokemons
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");

            //Trainer Table Triggers for Concurrency
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetTrainerTimestampOnUpdate
                    AFTER UPDATE ON Trainers
                    BEGIN
                        UPDATE Trainers
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
            migrationBuilder.Sql(
                @"
                    CREATE TRIGGER SetTrainerTimestampOnInsert
                    AFTER INSERT ON Trainers
                    BEGIN
                        UPDATE Trainers
                        SET RowVersion = randomblob(8)
                        WHERE rowid = NEW.rowid;
                    END
                ");
        }
    }

}

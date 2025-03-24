using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MTAIntranetAngular.API.Data;
using MTAIntranetAngular.API.Data.Models;
using MTAIntranetAngular.Utility;
using Microsoft.AspNetCore.Authorization;

namespace MTAIntranetAngular.API.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class SqltablesController : ControllerBase
    {
        private readonly MtaresourceMonitoringContext _context;

        public SqltablesController(MtaresourceMonitoringContext context)
        {
            _context = context;
        }

        [Route("Monitor")]
        public async Task<ActionResult<IEnumerable<Sqltable>>> Monitor()
        {
            var tables = _context.Sqltables.ToList();
            foreach (Sqltable table in tables)
            {

                try
                {
                    // Build the connection string dynamically
                    string connectionString = $"Server={table.ServerName};Database={table.DatabaseName};Integrated Security=True;TrustServerCertificate=True";

                    using (var connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"SELECT TOP 1 * FROM {table.TableName}";  // Simple existence check

                        using (var command = new SqlCommand(query, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                table.PreviousState = table.CurrentState;
                                table.CurrentState = reader.HasRows ? "Healthy" : "Unhealthy";
                                table.LastCheck = DateTime.Now;
                                table.LastHealthy = reader.HasRows ? DateTime.Now : table.LastHealthy;
                                //await _context.SaveChangesAsync();
                            }
                        }
                    }
                    table.LastCheck = DateTime.Now;
                }
                //try
                //{
                //    // Check if the table contains any records
                //    bool hasData = false;
                        
                //    string query = $"SELECT TOP 1 * FROM {table.TableName}";

                //    var result = await _context.Sqltables
                //        .FromSqlRaw(query)
                //        .AsNoTracking()
                //        .ToListAsync();

                //    table.PreviousState = table.CurrentState;
                //    table.CurrentState = hasData ? "Healthy" : "Unhealthy";
                //    table.LastCheck = DateTime.Now;

                //    if (!hasData && (DateTime.Now - table.LastHealthy).TotalMinutes >= table.AcceptableDowntime)
                //    {
                //        var err = $"SQL Table {table.TableName} on {table.ServerName} is empty or inaccessible.";
                //        EmailConfiguration.SendSQLTableFailure(table);
                //        table.LastEmailSent = DateTime.Now;
                //    }
                //    else if (hasData)
                //    {
                //        table.LastHealthy = DateTime.Now;
                //    }
                //}
                catch (Exception e)
                {
                    var err = $"Error reading {table.TableName} on {table.ServerName}: {e.Message}";
                    table.PreviousState = table.CurrentState;
                    table.CurrentState = "Unhealthy";
                    table.LastCheck = DateTime.Now;

                    EmailConfiguration.SendSQLTableFailure(table);
                    table.LastEmailSent = DateTime.Now;
                }

                // Initial connection success
                if (table.PreviousState == "Unknown" && table.CurrentState == "Healthy")
                {
                    EmailConfiguration.SendSQLTableInitSuccess(table);
                    table.LastEmailSent = DateTime.Now;
                    table.LastHealthy = DateTime.Now;
                }
                // Initial failure
                else if (table.PreviousState == "Unknown" && table.CurrentState == "Unhealthy")
                {
                    EmailConfiguration.SendSQLTableFailure(table);
                    table.LastEmailSent = DateTime.Now;
                }
                // Table went from healthy to unhealthy
                else if (table.PreviousState == "Healthy" && table.CurrentState == "Unhealthy")
                {
                    EmailConfiguration.SendSQLTableFailure(table);
                    table.LastEmailSent = DateTime.Now;
                }
                // Table recovered
                else if (table.PreviousState == "Unhealthy" && table.CurrentState == "Healthy")
                {
                    EmailConfiguration.SendSQLTableRestored(table);
                    table.LastEmailSent = DateTime.Now;
                    table.LastHealthy = DateTime.Now;
                }
                // Send periodic failure reminders
                else if (table.EmailFrequency != 0 && table.PreviousState == "Unhealthy" && table.CurrentState == "Unhealthy" &&
                         (table.LastEmailSent.AddMinutes(Convert.ToDouble(table.EmailFrequency)) <= DateTime.Now))
                {
                    EmailConfiguration.SendSQLTableFailure(table);
                    table.LastEmailSent = DateTime.Now;
                }
            }

            _context.SaveChanges();
            return await _context.Sqltables.ToListAsync();
        }


        // GET: api/Sqltables
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sqltable>>> GetSqltables()
        {
            return await _context.Sqltables.ToListAsync();
        }

        // GET: api/Sqltables/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sqltable>> GetSqltable(int id)
        {
            var sqltable = await _context.Sqltables.FindAsync(id);

            if (sqltable == null)
            {
                return NotFound();
            }

            return sqltable;
        }

        // PUT: api/Sqltables/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSqltable(int id, Sqltable sqltable)
        {
            if (id != sqltable.Id)
            {
                return BadRequest();
            }

            _context.Entry(sqltable).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SqltableExists(id))
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

        // POST: api/Sqltables
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sqltable>> PostSqltable(Sqltable sqltable)
        {
            _context.Sqltables.Add(sqltable);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSqltable", new { id = sqltable.Id }, sqltable);
        }

        // DELETE: api/Sqltables/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSqltable(int id)
        {
            var sqltable = await _context.Sqltables.FindAsync(id);
            if (sqltable == null)
            {
                return NotFound();
            }

            _context.Sqltables.Remove(sqltable);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SqltableExists(int id)
        {
            return _context.Sqltables.Any(e => e.Id == id);
        }

        public async Task<bool> TableExistsAsync(string database, string tableName)
        {
            var query = $@"
            SELECT COUNT(*) 
            FROM INFORMATION_SCHEMA.TABLES 
            WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = @tableName";

            var count = await _context.Database.ExecuteSqlRawAsync(query,
                new SqlParameter("@tableName", tableName));

            return count > 0;
        }

        //public async Task<List<dynamic>> ReadFromTableAsync(string database, string tableName)
        //{
        //    var query = $"SELECT TOP 10 * FROM {database}.dbo.{tableName}";

        //    return await _context.Sqltables
        //        .FromSqlRaw(query)
        //        .AsNoTracking()
        //        .ToListAsync();
        //}

    }
}

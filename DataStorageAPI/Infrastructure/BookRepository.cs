using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using DataStorageAPI.Models;
using DataStorageAPI.Infrastructure;
using Npgsql;
using NpgsqlTypes;
using System.Linq;

namespace DataStorageAPI.Providers
{
    public class BookRepository: IRepository<Book>
    {
        string ConnectionString;

        public BookRepository(string connString)
        {
            ConnectionString = connString;
        }

        public async Task<IEnumerable<Book>> GetAll()
        {
            var list = new List<Book>();
            
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM books", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                        while (reader.Read())
                        {
                            list.Add(new Book(
                                reader.GetGuid(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetInt32(4)));
                        }
                }
            }

            return list;
        }

        public async Task<IEnumerable<Guid>> GetAllIDs()
        {
            var list = new List<Guid>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var command = new NpgsqlCommand("SELECT guid FROM books", connection))
                {
                    using (var reader = await command.ExecuteReaderAsync())
                        while (reader.Read())
                            list.Add(reader.GetGuid(0));
                }
            }

            return list;
        }

        public async Task Add(Book model)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string cmdText = "INSERT INTO books(name, author, isbn, year) VALUES (@Name, @Author, @ISBN, @Year)";

                using (var command = new NpgsqlCommand(cmdText, connection))
                {
                    command.Parameters.Add("@Name", NpgsqlDbType.Varchar).Value = model.Name;
                    command.Parameters.Add("@Author", NpgsqlDbType.Varchar).Value = model.Author;
                    command.Parameters.Add("@ISBN", NpgsqlDbType.Varchar).Value = model.ISBN;
                    command.Parameters.Add("@Year", NpgsqlDbType.Integer).Value = model.Year;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Book> GetById(Guid guid)
        {
            Book book = null;

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var command = new NpgsqlCommand("SELECT * FROM books WHERE guid = @guid", connection))
                {
                    command.Parameters.Add("guid", NpgsqlDbType.Uuid).Value = guid;

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (reader.Read())
                        {
                            book = new Book(
                                reader.GetGuid(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetInt32(4));
                        }
                    }
                }
            }

            return book;
        }

        public async Task Update(Guid guid, Book model)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                string cmdTxt = "UPDATE books SET " +
                    "name = @name, author = @author, isbn = @isbn, year = @year " +
                    "WHERE guid = @guid";

                using (var command = new NpgsqlCommand(cmdTxt, connection))
                {
                    command.Parameters.Add("@name", NpgsqlDbType.Varchar).Value = model.Name;
                    command.Parameters.Add("@author", NpgsqlDbType.Varchar).Value = model.Author;
                    command.Parameters.Add("@isbn", NpgsqlDbType.Varchar).Value = model.ISBN;
                    command.Parameters.Add("@year", NpgsqlDbType.Integer).Value = model.Year;
                    command.Parameters.Add("@guid", NpgsqlDbType.Uuid).Value = guid;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task Remove(Guid guid)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                if (connection.State == ConnectionState.Closed)
                    connection.Open();

                using (var command = new NpgsqlCommand("DELETE FROM books WHERE guid = @Guid", connection))
                {
                    command.Parameters.Add("@Guid", NpgsqlDbType.Uuid).Value = guid;

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}

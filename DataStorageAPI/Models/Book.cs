using System;
using System.ComponentModel.DataAnnotations;

namespace DataStorageAPI.Models
{
    public class Book
    {
        public Book(Guid guid, string name, string author, string isbn, int year)
        {
            this.GUID = guid;
            this.Name = name;
            this.Author = author;
            this.ISBN = isbn;
            this.Year = year;
        }

        public Guid GUID { get; set; }

        [Required(ErrorMessage = "Name cannot be empty.")]
        [StringLength(128, ErrorMessage = "Name cannot be longer than 128 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Author cannot be empty.")]
        [StringLength(128, ErrorMessage = "Author name cannot be longer than 128 characters.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "ISBN cannot be empty.")]
        [StringLength(17, ErrorMessage = "ISBN cannot be longer than 17 characters.")]
        public string ISBN { get; set; }

        [Required(ErrorMessage = "Year cannot be empty.")]
        public int Year { get; set; }
    }
}

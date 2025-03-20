using System.Globalization;
using ExamenLibros.Data;
using ExamenLibros.Models;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExamenLibros.Repositories
{
    public class RepositoryLibros
    {
        private LibrosContext context;
        public RepositoryLibros(LibrosContext context)
        {
            this.context = context;
        }

        

        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            Usuario user = await this.context.Usuarios
                .Where(x => x.IdUsuario == idUsuario).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Usuario> LogInUsuarioAsync(string nombre, string pass)
        {
            Usuario user = await this.context.Usuarios
                .Where(x => x.Nombre == nombre && x.Pass == pass).FirstOrDefaultAsync();
            return user;
        }

        public async Task<List<Libro>> GetLibrosAsync()
        {
            return await context.Libros.ToListAsync();
        }

        public async Task<List<Genero>> GetGenerosAsync()
        {
            return await context.Generos.ToListAsync();
        }

        public async Task<List<Libro>> GetLibrosSessionAsync(List<int> idSession)
        {
            if (idSession == null || idSession.Count == 0)
            {
                return new List<Libro>();
            }

            return await context.Libros
                .Where(c => idSession.Contains(c.IdLibro))
                .ToListAsync();
        }
    }
}

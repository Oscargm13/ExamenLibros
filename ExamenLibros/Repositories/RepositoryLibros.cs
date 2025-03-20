using System.Globalization;
using ExamenLibros.Data;
using ExamenLibros.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;
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

        public async Task<List<Pedido>> GetPedidosAsync()
        {
            return await context.Pedidos.ToListAsync();
        }

        public async Task<Pedido> FindPedido(int idPedido)
        {
            return await this.context.Pedidos
                .Where(x => x.IdUsuario == idPedido).FirstOrDefaultAsync();
        }

        public async Task<int> TramitarCompra(List<Libro> libros, int idUsuario, int pedidos, int idFactura)
        {          
            foreach(Libro libro in libros)
            {
                //int ultimoPeidoId = pedidos.Count();
                Pedido nuevoPedido = new Pedido
                {
                    IdPedido = pedidos + 1,
                    IdFactura = idFactura,
                    Fecha = DateTime.Now.Date,
                    IdLibro = libro.IdLibro,
                    IdUsuario = idUsuario,
                    Cantidad = 1
                };
                context.Pedidos.Add(nuevoPedido);
                await context.SaveChangesAsync();
            }
            return 1;
        }

        public async Task<Usuario> FindUsuarioAsync(int idUsuario)
        {
            Usuario user = await this.context.Usuarios
                .Where(x => x.IdUsuario == idUsuario).FirstOrDefaultAsync();
            return user;
        }

        public async Task<Libro> FindLibro(int idLibro)
        {
            Libro libro = await this.context.Libros
                .Where(x => x.IdLibro == idLibro).FirstOrDefaultAsync();
            return libro;
        }

        public async Task<Usuario> LogInUsuarioAsync(string nombre, string pass)
        {
            Usuario user = await this.context.Usuarios
                .Where(x => x.Email == nombre && x.Pass == pass).FirstOrDefaultAsync();
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

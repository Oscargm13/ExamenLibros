using ExamenLibros.Extensions;
using ExamenLibros.Filters;
using ExamenLibros.Models;
using ExamenLibros.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ExamenLibros.Controllers
{
    public class LibrosController : Controller
    {
        private RepositoryLibros repo;
        public LibrosController(RepositoryLibros repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> Compra()
        {
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            List<Libro> libros = await this.repo.GetLibrosSessionAsync(ids);
            List<Pedido> pedidos = await this.repo.GetPedidosAsync();
            Pedido ultimoPedido = await this.repo.FindPedido(pedidos.Count);

            this.repo.TramitarCompra(libros, usuario.IdUsuario, pedidos.Count(), ultimoPedido.IdFactura);
            return RedirectToAction("Perfil");
        }

        public async Task<IActionResult> Details(int id)
        {
            Libro libro = await this.repo.FindLibro(id);
            return View(libro);
        }

        public async Task<IActionResult> Carrito(int? id)
        {
            List<int> ids = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
            if (id != null)
            {
                ids.Remove(id.Value);
                //ES POSIBLE QUE YA NO TENGAMOS EMPLEADOS EN SESSION 
                if (ids.Count == 0)
                {
                    //ELIMINAMOS DE SESSION NUESTRA KEY 
                    HttpContext.Session.Remove("IDSLIBROS");
                }
                else
                {
                    //ACTUALIZAMOS SESSION CON EL EMPLEADO YA ELIMINADO 
                    HttpContext.Session.SetObject("IDSLIBROS", ids);
                }
            }
            List<Libro> libros = await this.repo.GetLibrosSessionAsync(ids);
            return View(libros);
        }

        [AuthorizeLibros]
        public async Task<IActionResult> Perfil()
        {
            Usuario usuario = HttpContext.Session.GetObject<Usuario>("USUARIO");
            return View(usuario);
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id != null)
            {
                List<int> idsLibros;
                int exist = 0;
                if (HttpContext.Session.GetObject<List<int>>("IDSLIBROS") == null)
                {
                    idsLibros = new List<int>();
                }
                else
                {
                    idsLibros = HttpContext.Session.GetObject<List<int>>("IDSLIBROS");
                    foreach (var item in idsLibros)
                    {
                        if (item == id)
                        {
                            exist = 1;
                        }
                    }
                }
                if (exist == 0)
                {
                    idsLibros.Add(id.Value);
                }
                else
                {
                    exist = 0;
                }

                HttpContext.Session.SetObject("IDSLIBROS", idsLibros);
                ViewData["MENSAJE"] = "Libros  almacenados: " + idsLibros.Count;
            }
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }
    }
}

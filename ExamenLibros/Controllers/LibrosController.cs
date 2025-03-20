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
                List<int> idsCubos;
                int exist = 0;
                if (HttpContext.Session.GetObject<List<int>>("IDSCUBOS") == null)
                {
                    idsCubos = new List<int>();
                }
                else
                {
                    idsCubos = HttpContext.Session.GetObject<List<int>>("IDSCUBOS");
                    foreach (var item in idsCubos)
                    {
                        if (item == id)
                        {
                            exist = 1;
                        }
                    }
                }
                if (exist == 0)
                {
                    idsCubos.Add(id.Value);
                }
                else
                {
                    exist = 0;
                }

                HttpContext.Session.SetObject("IDSCUBOS", idsCubos);
                ViewData["MENSAJE"] = "Cubos  almacenados: " + idsCubos.Count;
            }
            List<Libro> libros = await this.repo.GetLibrosAsync();
            return View(libros);
        }
    }
}

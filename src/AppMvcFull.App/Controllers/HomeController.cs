using AppMvcFull.App.Extensions;
using AppMvcFull.App.Models;
using AppMvcFull.Business.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AppMvcFull.App.Controllers
{
    public class HomeController : BaseController
    {

        public HomeController(
            INotification notification,
            ILogger<HomeController> logger,
            IWebHostEnvironment env
            ) : base(notification, env, logger)
        {
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [Route("erro/{id:length(3,3)}")]
        public IActionResult HandleErrorDevelopment(int id)
        {
            var teste = Request.HttpContext;
            _logger.LogInformation($"Error Code {id}");

            var modelErro = new ErrorViewModel();

            if (id == StatusCodes.Status500InternalServerError)
            {
                modelErro.MessageText = "Ocorreu um erro! Tente novamente mais tarde ou contate nosso suporte.";
                modelErro.Title = "Ocorreu um erro!";
                modelErro.ErrorCode = id;
            }
            else if (id == StatusCodes.Status404NotFound)
            {
                modelErro.MessageText = "A página que está procurando não existe! <br />Em caso de dúvidas entre em contato com nosso suporte";
                modelErro.Title = "Ops! Página não encontrada.";
                modelErro.ErrorCode = id;
            }
            else if (id == StatusCodes.Status403Forbidden)
            {
                modelErro.MessageText = "Você não tem permissão para fazer isto.";
                modelErro.Title = "Acesso Negado";
                modelErro.ErrorCode = id;
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            return View("Error", modelErro);
        }


    }
}

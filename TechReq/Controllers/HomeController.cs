using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TeachRed.Domain.Models;
using TeachRed.Domain.ViewModels.LoginAndRegistration;
using TeachRed.Service;
using TeachRed.Service.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;


namespace TeachRed.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        // Лучше внедрять IMapper через конструктор (DI), но пока оставим как у вас
        MapperConfiguration _mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public HomeController(IAccountService accountService)
        {
            _accountService = accountService;
            _mapper = _mapperConfiguration.CreateMapper();
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailViewModel model)
        {
            // Маппинг и вызов сервиса
            var user = _mapper.Map<Users>(model);
            // Предполагается, что response.Data содержит ClaimsIdentity при успехе
            var response = await _accountService.ConfirmEmail(user, model.Code, model.ConfirmCode);

            if (response.StatusCode == TeachRed.Domain.Enum.StatusCode.Ok)
            {
                // ЯВНАЯ ПРОВЕРКА и АВТОРИЗАЦИЯ
                if (response.Data is ClaimsIdentity identity)
                {
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(identity) // Используем полученные Claims
                    );

                    // УСПЕХ: Возвращаем однозначный сигнал success = true
                    return Json(new { success = true, description = "Регистрация и вход выполнены." });
                }

                // Крайний случай, если сервис вернул ОК, но данные (Claims) отсутствуют
                return Json(new { success = false, errors = new[] { "Ошибка сессии: нет данных для входа." } });
            }

            // Ошибка
            var msg = response.Description;
            return Json(new { success = false, errors = new[] { msg } });
        }

        // ------------------------------------------------------------------
        // --- Обработка выхода (Logout) ---
        // ------------------------------------------------------------------
        [HttpGet]
        public IActionResult AboutUs()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Map()
        {
            return View();
        }

        [HttpGet] // Важно: это GET запрос, так как мы переходим по ссылке
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home"); // Обычно перенаправляют на главную
        }

        // ------------------------------------------------------------------
        // --- Обработка входа (Login) --- (Рисунок 140)
        // ------------------------------------------------------------------
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var users = _mapper.Map<Users>(model);
                var response = await _accountService.Login(users);

                if (response.StatusCode == TeachRed.Domain.Enum.StatusCode.Ok)
                {
                    // ЯВНАЯ ПРОВЕРКА и АВТОРИЗАЦИЯ
                    if (response.Data is ClaimsIdentity identity)
                    {
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity) // Используем полученные Claims
                        );

                        // УСПЕХ: Возвращаем однозначный сигнал success = true
                        return Json(new { success = true, description = "Вход выполнен." });
                    }
                    return Json(new { success = false, errors = new[] { "Ошибка сессии: нет данных для входа." } });
                }

                // Ошибка сервиса (неверный пароль)
                ModelState.AddModelError("", response.Description);
            }

            // Сбор ошибок валидации модели
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            // Возвращаем JSON с success = false и списком ошибок
            return Json(new { success = false, errors = errors });
        }

        // ------------------------------------------------------------------
        // --- Обработка регистрации (Register) --- (Рисунок 141)
        // ------------------------------------------------------------------
        // HomeController.cs

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<Users>(model);

                // Вызываем сервис. Он отправляет почту и возвращает код (BaseResponse<string>)
                var response = await _accountService.Register(user);

                if (response.StatusCode == TeachRed.Domain.Enum.StatusCode.Ok)
                {
                    // ВАЖНО: Возвращаем JSON именно в таком формате, какой ждет ваш JS
                    return Json(new
                    {
                        statusCode = 200,
                        description = "Код отправлен",
                        data = response.Data // Здесь лежит код подтверждения
                    });
                }

                return Json(new { statusCode = 500, description = response.Description });
            }
            return Json(new { statusCode = 400, description = "Ошибка валидации данных" });
        }
        // 1. Метод, который вызывает ошибку 404 (На него ведет ваша кнопка)
        public async Task AuthenticationGoogle()
        {
            // Эта команда перенаправляет пользователя на сайт Google
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties()
            {
                // Куда вернуться после успешного входа (см. метод ниже)
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        // 2. Метод, куда Google вернет пользователя после ввода пароля
        public async Task<IActionResult> GoogleResponse()
        {
            // Получаем данные, которые прислал Google
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return RedirectToAction("Index"); // Если не вышло — на главную
            }

            // Здесь можно получить Email и Имя пользователя из result.Principal.Claims
            // var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;

            // Перенаправляем на главную страницу (вы уже вошли)
            return RedirectToAction("Index");
        }
    }
}
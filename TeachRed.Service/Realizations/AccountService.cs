using AutoMapper;
using FluentValidation;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Security.Claims;
using TeachRed.Domain;
using TeachRed.Domain.Enum;
using TeachRed.Domain.Helpers;
using TeachRed.Domain.Models;
using TeachRed.Domain.Response;
using TeachRed.Service.Interfaces;
using TechReq.DAL.Interfaces;
using TechReq.Domain.ModelsDb;

namespace TeachRed.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IBaseStorage<UsersDb> _userStorage;
        private readonly IMapper _mapper;

        private UsersValidators _validationRules { get; set; }

        MapperConfiguration _mapperConfiguration = new MapperConfiguration(p =>
        {
            p.AddProfile<AppMappingProfile>();
        });

        public AccountService(IBaseStorage<UsersDb> userStorage)
        {
            _userStorage = userStorage;
            _mapper = _mapperConfiguration.CreateMapper();
            _validationRules = new UsersValidators();
        }

        // ------------------------------------------------------------------
        // Метод Register (Исправлен возврат ClaimsIdentity)
        // ------------------------------------------------------------------

        public async Task<BaseResponse<ClaimsIdentity>> IsCreatedAccount(Users model)
        {
            try
            {
                // ИСПРАВЛЕНИЕ: Убрали var userDb = new UsersDb(); - это вызывало ошибку

                // Проверяем, есть ли пользователь
                if (await _userStorage.GetAll().FirstOrDefaultAsync(x => x.Email == model.Email) == null)
                {
                    model.Password = "google";
                    model.CreatedAt = DateTime.Now;

                    // Исправление Role
                    if (!Enum.IsDefined(typeof(Role), model.Role)) model.Role = Role.User;

                    // Маппер сам создаст нужный объект UsersDb со всеми полями
                    var newUserDb = _mapper.Map<UsersDb>(model);

                    await _userStorage.Add(newUserDb);

                    var resultRegister = AuthenticateUserHelper.Authenticate(model);

                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Data = resultRegister,
                        Description = "Объект добавился",
                        StatusCode = StatusCode.Ok
                    };
                }

                var resultLogin = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = resultLogin,
                    Description = "Объект уже был создан",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
        public async Task SendEmail(string email, string confirmationCode)
        {
            // ВНИМАНИЕ: Укажи здесь путь к ТВОЕМУ файлу с паролем на твоем компьютере
            // Или просто замени эту логику на var password = "твой_пароль_приложения";
            //string path = @"D:\Users\YourUser\Desktop\password.txt";
            string password = "olvufoccnzbrbgqz";
            string myEmail = "techreq.official@gmail.com";

            var emailMessage = new MimeMessage();

            // Настройка отправителя (Имя и Почта)
            emailMessage.From.Add(new MailboxAddress("Сервис TechReq", "techreq.service@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = "Код подтверждения регистрации TechReq";

            // HTML-тело письма (дизайн под автосервис)
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = "<!DOCTYPE html>" +
                "<html>" +
                "<head>" +
                "<style>" +
                    "body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #e9ecef; margin: 0; padding: 0; }" +
                    ".container { max-width: 600px; margin: 40px auto; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 15px rgba(0,0,0,0.1); overflow: hidden; }" +
                    ".header { background-color: #2c3e50; padding: 30px; text-align: center; }" +
                    ".header h1 { color: #ffffff; margin: 0; font-size: 28px; letter-spacing: 1px; }" +
                    ".header p { color: #bdc3c7; margin: 5px 0 0; font-size: 14px; }" +
                    ".content { padding: 30px; text-align: center; color: #333; }" +
                    ".message { font-size: 16px; line-height: 1.6; margin-bottom: 25px; color: #555; }" +
                    ".code-box { background-color: #f8f9fa; border: 2px dashed #3498db; border-radius: 8px; padding: 15px; display: inline-block; margin-bottom: 20px; }" +
                    ".code { font-size: 32px; font-weight: bold; color: #2c3e50; letter-spacing: 5px; font-family: monospace; }" +
                    ".footer { background-color: #f1f1f1; padding: 15px; text-align: center; font-size: 12px; color: #888; }" +
                "</style>" +
                "</head>" +
                "<body>" +
                    "<div class='container'>" +
                        "<div class='header'>" +
                            "<h1>TechReq</h1>" +
                            "<p>Профессиональное обслуживание вашего авто</p>" +
                        "</div>" +
                        "<div class='content'>" +
                            "<div class='message'>" +
                                "<p>Здравствуйте!</p>" +
                                "<p>Спасибо за регистрацию в системе TechReq. Чтобы подтвердить свой Email и получить доступ к записи на сервис, введите этот код:</p>" +
                            "</div>" +
                            "<div class='code-box'>" +
                                "<span class='code'>" + confirmationCode + "</span>" +
                            "</div>" +
                            "<p style='font-size: 14px; color: #999;'>Если вы не регистрировались на нашем сайте, просто проигнорируйте это письмо.</p>" +
                        "</div>" +
                        "<div class='footer'>" +
                            "&copy; 2025 TechReq Service. Все права защищены." +
                        "</div>" +
                    "</div>" +
                "</body>" +
                "</html>"
            };

            // Чтение пароля и отправка (как на фото)
            // Убедись, что файл существует по указанному пути 'path'
            {

                using (var client = new SmtpClient())
                {
                    // Используем Gmail (как на фото)
                    await client.ConnectAsync("smtp.gmail.com", 465, true);

                    // ТУТ ВАЖНО: Укажи ту почту, с которой отправляешь (Gmail)
                    // Пароль берется из файла, но логин нужно прописать тут
                    await client.AuthenticateAsync(myEmail, password);
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                }
            }
        }

        public async Task<BaseResponse<ClaimsIdentity>> ConfirmEmail(Users model, string code, string confirmCode)
        {
            try
            {
                // 1. Проверка: совпадает ли введенный код с отправленным
                if (code != confirmCode)
                {
                    throw new Exception("Неверный код! Регистрация не выполнена.");
                }

                // 2. Подготовка данных пользователя
                // Если у вас нет поля PathImage в модели Users, закомментируйте эту строку
                // model.PathImage = "/images/user.png"; 

                model.CreatedAt = DateTime.Now;

                // 3. Валидация (если валидатор подключен в конструкторе сервиса)
                await _validationRules.ValidateAndThrowAsync(model);
                model.Password = HashPasswordHelper.HashPassword(model.Password);


                // 4. Маппинг в модель базы данных (Users -> UsersDb)
                var userDb = _mapper.Map<UsersDb>(model);

                // 5. Добавление в базу данных
                await _userStorage.Add(userDb);

                // 6. Аутентификация (создание Claims для куки)
                // Используем model, так как она содержит нужные данные (Role, Name и т.д.)
                var result = AuthenticateUserHelper.Authenticate(model);

                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result,
                    Description = "Объект добавился",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = ex.Message,
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        // Обрати внимание: возвращаемый тип теперь BaseResponse<string>
        public async Task<BaseResponse<string>> Register(Users model)
        {
            try
            {
                // --- [ИЗМЕНЕНИЕ 1] Генерация кода ---
                Random random = new Random();
                string confirmationCode = $"{random.Next(10)}{random.Next(10)}{random.Next(10)}{random.Next(10)}";

                await _validationRules.ValidateAndThrowAsync(model);

                // --- [ИЗМЕНЕНИЕ 2] Проверка на существование (изменился тип возврата) ---
                var existingUser = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (existingUser != null)
                {
                    return new BaseResponse<string>()
                    {
                        Description = "Пользователь с такой почтой уже есть",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // Логику сохранения в БД оставляем, чтобы пользователь создался
                model.Password = HashPasswordHelper.HashPassword(model.Password);
                model.CreatedAt = DateTime.Now;

                if (!Enum.IsDefined(typeof(Role), model.Role))
                {
                    model.Role = Role.User;
                }

                var userDb = _mapper.Map<UsersDb>(model);
                await _userStorage.Add(userDb);

                // --- [ИЗМЕНЕНИЕ 3] Отправка письма ---
                // Убедись, что метод SendEmail у тебя добавлен в этом классе
                await SendEmail(model.Email, confirmationCode);

                // --- [ИЗМЕНЕНИЕ 4] Возвращаем код и статус ОК ---
                return new BaseResponse<string>()
                {
                    Data = confirmationCode,
                    Description = "Письмо отправлено",
                    StatusCode = StatusCode.Ok
                };
            }
            // --- [ИЗМЕНЕНИЕ 5] Блоки catch теперь возвращают string ---
            catch (ValidationException ex)
            {
                var errorMessages = string.Join("; ", ex.Errors.Select(e => e.ErrorMessage));
                return new BaseResponse<string>()
                {
                    Description = errorMessages,
                    StatusCode = StatusCode.BadRequest
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<string>()
                {
                    Description = $"[Register] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }


        // ------------------------------------------------------------------
        // Метод Login (Исправлена ошибка Value cannot be null)
        // ------------------------------------------------------------------
        public async Task<BaseResponse<ClaimsIdentity>> Login(Users model)
        {
            try
            {
                // 1. Поиск пользователя в БД
                var userDb = await _userStorage.GetAll()
                    .FirstOrDefaultAsync(x => x.Email == model.Email);

                if (userDb == null)
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFound
                    };
                }

                // 2. Проверка пароля
                if (userDb.Password != HashPasswordHelper.HashPassword(model.Password))
                {
                    return new BaseResponse<ClaimsIdentity>()
                    {
                        Description = "Неверный пароль или почта",
                        StatusCode = StatusCode.BadRequest
                    };
                }

                // 3. Маппинг в доменную модель (ТУТ ПОЯВЛЯЕТСЯ ИМЯ И РОЛЬ)
                var users = _mapper.Map<Users>(userDb);

                // 4. ГЛАВНОЕ ИСПРАВЛЕНИЕ:
                // Мы передаем в Authenticate объект 'users' (полный, из БД), 
                // а не 'model' (пустой, с формы входа).
                var result = AuthenticateUserHelper.Authenticate(users);

                // 5. Успешный ответ
                return new BaseResponse<ClaimsIdentity>()
                {
                    Data = result, // Возвращаем ClaimsIdentity (result), это нужно контроллеру
                    Description = "Успешный вход",
                    StatusCode = StatusCode.Ok
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ClaimsIdentity>()
                {
                    Description = $"[Login] - {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}


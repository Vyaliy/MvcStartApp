using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using MvcStartApp.Models.AppContext;
using MvcStartApp.Models.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using MvcStartApp.Models.DB;
using Microsoft.EntityFrameworkCore.Internal;

namespace WebApplication2.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private static IRequestRepository _requestRepository;
        private static IWebHostEnvironment environment;

        /// <summary>
        ///  Middleware-компонент должен иметь конструктор, принимающий RequestDelegate
        /// </summary>
        public LoggingMiddleware(RequestDelegate next, IWebHostEnvironment env, IRequestRepository requestRepository)
        {
            _next = next;
            environment = env;
            _requestRepository = requestRepository;
        }

        /// <summary>
        ///  Необходимо реализовать метод Invoke  или InvokeAsync
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            LogConsole(context);
            await LogFile(context);
            LogDB(context);

            // Передача запроса далее по конвейеру
            await _next.Invoke(context);
        }

        private static async Task LogFile(HttpContext context)
        {
            // Строка для публикации в лог
            string logMessage = $"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}{Environment.NewLine}";

            // Путь до лога (опять-таки, используем свойства IWebHostEnvironment)
            string logFilePath = Path.Combine(environment.ContentRootPath, "Logs", "RequestLog.txt");

            // Используем асинхронную запись в файл
            await File.AppendAllTextAsync(logFilePath, logMessage);
        }

        private static void LogConsole(HttpContext context)
        {
            // Для логирования данных о запросе используем свойста объекта HttpContext
            Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
        }

        private static async void LogDB(HttpContext context)
        {
            await _requestRepository.AddRequest(context.Request.GetDisplayUrl());
        }
    }
}

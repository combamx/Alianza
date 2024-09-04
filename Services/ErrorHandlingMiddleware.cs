using System.Net;

namespace Alianza.Services
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware ( RequestDelegate next )
        {
            _next = next;
        }

        public async Task InvokeAsync ( HttpContext context )
        {
            try
            {
                // Intentar ejecutar el siguiente middleware en la cadena
                await _next ( context );
            }
            catch (Exception ex)
            {
                // Capturar excepciones y manejar el error
                await HandleExceptionAsync ( context , ex );
            }
        }

        private Task HandleExceptionAsync ( HttpContext context , Exception exception )
        {
            // Configurar el código de estado HTTP y la respuesta
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

            // Crear el objeto ResponseData con el mensaje de error
            var response = new ResponseData
            {
                Data = null ,
                Message = $"Se ha producido un error en el servidor: {exception.Message}" ,
                Status = context.Response.StatusCode
            };

            // Serializar la respuesta a JSON
            return context.Response.WriteAsJsonAsync ( response );
        }
    }
}

﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Net.Http;
using Utils.CustomExceptions;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        //NOTES
        // At the time of designing this middleware, I wanted to centralize error handling for business exceptions and unexpected exceptions.
        // However, I realized that this middleware is not suitable because you lose the ability to return specific error responses for different exceptions.
        // This means we'd have to handle all exceptions in a generic way, which is not ideal for APIs that need to provide specific error messages.
        // Therefore, I decided to hold off on using this middleware for now.

        try
        {
            // Passes control to the next middleware/controller
            await _next(context);
        }
        catch (BusinessException ex)
        {
            _logger.LogError(ex, "Business rule violation occurred.");
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json"; // Set content type explicitly
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { ex.Message })); // Use WriteAsync with JsonSerializer
            //await context.Response.WriteAsJsonAsync(new { ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred.");
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json"; // Set content type explicitly
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { Message = "Internal server error" })); // Use WriteAsync with JsonSerializer
            //await context.Response.WriteAsJsonAsync(new { Message = "Internal server error" });
        }
    }
}

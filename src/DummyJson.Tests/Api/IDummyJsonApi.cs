using System.Text.Json.Nodes;
using DummyJson.Tests.Api.Request;
using DummyJson.Tests.Api.Response;
using Refit;

namespace DummyJson.Tests.Api;

public interface IDummyJsonApi
{
    [Post("/auth/login")]
    Task<IApiResponse<LoginResponse>> LoginAsync([Body] LoginRequest request);

    [Get("/auth/me")]
    Task<IApiResponse<UserResponse>> GetCurrentUserAsync([Authorize("Bearer")] string accessToken);

    [Get("/users/filter")]
    Task<IApiResponse<UserListResponse>> FilterUsersAsync(string key, string value, int limit);

    [Post("/users/add")]
    Task<IApiResponse<UserResponse>> AddUserAsync([Body] AddUserRequest request);

    [Get("/products/search")]
    Task<IApiResponse<ProductListResponse>> SearchProductsAsync(string q);

    [Get("/products")]
    Task<IApiResponse<JsonObject>> GetProductsAsync(int limit, int skip, string select);

    [Put("/products/{id}")]
    Task<IApiResponse<ProductResponse>> UpdateProductAsync(int id, [Body] UpdateProductRequest request);

    [Post("/carts/add")]
    Task<IApiResponse<CartResponse>> AddCartAsync([Body] AddCartRequest request);

    [Delete("/carts/{id}")]
    Task<IApiResponse<CartResponse>> DeleteCartAsync(int id);
}

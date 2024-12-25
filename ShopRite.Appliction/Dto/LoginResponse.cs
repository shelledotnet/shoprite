namespace ShopRite.Application.Dto
{
    public record LoginResponse(string? Token, string? Username, string? Email, 
        List<string>? Role, DateTime ValidTo, DateTime ValidFrom, string? RefreshToken, DateTime RefreshTokenExpireDate);


  
}

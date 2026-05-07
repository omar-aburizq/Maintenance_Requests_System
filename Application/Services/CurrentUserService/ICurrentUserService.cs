namespace Application.Services.CurrentUserService
{
    public interface ICurrentUserService
    {
        Guid? UserId { get; }
        string? Name { get; }
        string? Email { get; }
        string? MobilePhone { get; }
        string? Role { get; }
    }
}

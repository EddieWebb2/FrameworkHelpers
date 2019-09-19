namespace FrameworkHelpers.WebUserApi.Interfaces
{
    public interface ILoginRequest
    {
        string Email { get; set; }
        string Password { get; set; }
    }
}

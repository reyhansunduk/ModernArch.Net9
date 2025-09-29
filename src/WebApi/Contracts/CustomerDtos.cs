namespace WebApi.Contracts
{
    public record CustomerCreateDto(string FirstName, string LastName, string Email);
    public record CustomerUpdateDto(string FirstName, string LastName, string Email);
    public record CustomerDto(long Id, string FirstName, string LastName, string Email, bool IsActive);
}

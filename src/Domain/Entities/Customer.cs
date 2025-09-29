namespace Domain.Entities;

public class Customer
{
    public long Id { get; private set; }
    public string FirstName { get; private set; } = default!;
    public string LastName { get; private set; } = default!;
    public string Email { get; private set; } = default!;
    public bool IsActive { get; private set; } = true;

    private Customer() { } 

    public Customer(string firstName, string lastName, string email)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
    }

    public void Update(string firstName, string lastName, string email)
    {
        SetFirstName(firstName);
        SetLastName(lastName);
        SetEmail(email);
    }

    public void Deactivate() => IsActive = false;

    void SetFirstName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("FirstName boş olamaz");
        FirstName = name.Trim();
    }

    void SetLastName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("LastName boş olamaz");
        LastName = name.Trim();
    }

    void SetEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains("@"))
            throw new ArgumentException("Geçerli bir Email giriniz");
        Email = email.Trim();
    }
}

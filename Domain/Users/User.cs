using Domain.Persons;
using DomainBase.Domain;

namespace Domain.Users;

public class User : AggregateRoot<Guid?>
{
    public string Username {get; private set;}
    public string Password {get; private set;}
    public Person Person {get; private set;}
    public Userrole Role {get; private set;}

    public User(string username, string password, Person person) : base(null)
    {
        Username = username;
        Password = password;
        Person = person;
    }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	public User(): base(null)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
	{
	}

    public bool Login(string password)
    {
        return Password == password;
    }

    public override void Validate()
    {
        if (string.IsNullOrEmpty(Username))
			throw new ArgumentNullException("Name", "User must have valid Username.");

        Person.Validate();
    }
}
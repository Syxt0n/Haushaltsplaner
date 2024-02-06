using Domain.Persons;
using Domain.Shared;
using DomainBase.Domain;

namespace Domain.Users;

public class User : AggregateRoot<Guid?>
{
    public string Username {get; private set;}
    public string Password {get; private set;}
    public Person Person {get; private set;}
    public Userrole Role {get; private set;}

    public User(string username, string password, Person person, Userrole role) : base(null)
    {
        Username = username;
        Password = password;
        Person = person;
        Role = role;
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

    public void ChangeUsername(string username)
    {
        this.Username = username;
        Validate();
        this.AddDomainEvent(new UserUsernameChangedEvent(this));
    }

    public void ChangePassword(string password)
    {
        this.Password = password;
        Validate();
        this.AddDomainEvent(new UserPasswordChangedEvent(this));
    }

    public void ChangeUserRole(Userrole role)
    {
        this.Role = role;
        Validate();
        this.AddDomainEvent(new UserRoleChangedEvent(this));
    }

    public override void Validate()
    {
        if (string.IsNullOrEmpty(Username))
			throw new ArgumentNullException("Name", "User must have valid Username.");

        Person.Validate();
    }
}
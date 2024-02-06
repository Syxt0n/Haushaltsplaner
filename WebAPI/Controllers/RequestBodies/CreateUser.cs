using Domain;
using Domain.Persons;

namespace WebAPI.Controllers.RequstBodies;

public class CreateUser : IValidatable
{
	public string Username {get; set;} = "";
	public string Password {get; set;} = "";
	public Person Person {get; set;}
	public Userrole userrole;

    public bool Validate()
    {
		Person.Validate();
		return String.IsNullOrEmpty(Username) || string.IsNullOrEmpty(Password);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;
using Domain.Shared;

namespace Domain.Choreplans;

public class Chore : ValueObject
{
    public string Name { get; } = "";
    public string Description { get; } = "";

    public Chore(string name, string description)
    {
        Name = name;
        Description = description;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }
}

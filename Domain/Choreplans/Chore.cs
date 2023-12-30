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

    public Chore()
    {}

    public Chore(string name, string description)
    {
        Name = name;
        Description = description;

        Validate();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Name;
        yield return Description;
    }

    public override void Validate()
    {
        if (string.IsNullOrEmpty(Name))
            throw new ArgumentNullException("Name", "Chore must have valid Name.");

        if (string.IsNullOrEmpty(Description))
            throw new ArgumentNullException("Description", "Chore must have valid Description.");
    }
}

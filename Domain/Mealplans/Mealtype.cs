using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DomainBase.Domain;

namespace Domain.Mealplans;
public class Mealtype : ValueObject
{
	public string Value { get; } = "";

	public Mealtype()
	{}

	public Mealtype(string value)
	{
		Value = value;

		Validate();
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return Value;
	}

    public override void Validate()
    {
        if (string.IsNullOrEmpty(Value))
			throw new ArgumentNullException("Value", "Mealtype must have valid Value.");
    }
}

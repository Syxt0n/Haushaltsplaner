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

	public Mealtype(string value)
	{
		this.Value = value;
	}

	protected override IEnumerable<object> GetEqualityComponents()
	{
		yield return this.Value;
	}
}

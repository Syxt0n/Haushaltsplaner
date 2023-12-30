using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace DomainBase.Domain;
public abstract class ValueObject
{
	protected abstract IEnumerable<object> GetEqualityComponents();

	public abstract void Validate();

	public override bool Equals(object? obj)
	{
		if (obj == null || obj.GetType() != GetType()) return false;
		if (ReferenceEquals(this, obj)) return true;

		var other = (ValueObject)obj;

		return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
	}

	public override int GetHashCode()
	{
		return GetEqualityComponents()
			.Aggregate(1, (current, obj) =>
			{
				unchecked
				{
					return current * 23 + (obj?.GetHashCode() ?? 0);
				}
			});
	}

	public static bool operator ==(ValueObject a, ValueObject b)
	{
		if (ReferenceEquals(a, null) && ReferenceEquals(b, null))
			return true;

		if (ReferenceEquals(a, null) || ReferenceEquals(b, null))
			return false;

		return a.Equals(b);
	}

	public static bool operator !=(ValueObject a, ValueObject b)
	{
		return !(a == b);
	}

}
